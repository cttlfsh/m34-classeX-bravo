using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

// Sanzio Pedersoli - 21/06/2022 - v001

#region DATA

/// <summary>   The data needeed to establish a connection to a bot, and to specify the channel the bot will write to.  </summary>
[Serializable]
public struct TwitchCredentials
{
    [Header("Streamer info:")]
    [Tooltip("The Twitch channel that will receive messages from this bot.")]
    public string channelName;

    [Header("Bot login:")]
    [Tooltip("The name of your bot channel, will be used to log in the bot.")]
    public string botName;
    [Tooltip("The authCode token of your bot channel, will be used to log in the bot.")]
    public string authCode;
}

/// <summary>   A package with useful information about a single Twitch chat message.   </summary>
public struct MessageData
{
    /// <summary>   The nickname of the author of the message.  </summary>
    public string nickname;
    /// <summary>   The string written by the user in the message.  </summary>
    public string message;

    /// <summary>   <see cref="MessageData"/> constructor.  </summary>
    /// <param name="author"> The nickname of the author of the message. </param>
    /// <param name="message"> The string written by the user in the message. </param>
    public MessageData(string author, string message)
    {
        nickname = author;
        this.message = message;
    }
}

[System.Serializable]
public class MessageDataEvent : UnityEvent<MessageData> {}

#endregion

/// <summary>   Singleton used to connect to a Twitch chat, receive events for each message in that chat, and send message to that chat.    </summary>
public class TwitchBot : MonoBehaviour
{

    #region PROPERTIES

    /// <summary>   Get the Singleton instance of this class.   </summary>
    public static TwitchBot Sing
    {
        get
        {
            if (sing == null)
            {
                sing = new TwitchBot();
            }
            return sing;
        }
    }

    #endregion

    #region FIELDS

    [Header("Twitch settings:")]
    [Tooltip("The credential that this class is currently using to connect with Twitch.")]
    public TwitchCredentials credentials;

    [Header("Events:")]
    [Tooltip("All the function that need to be called and receive the info when new message arrives.")]
    public MessageDataEvent functionsToBeCalled;

    /// <summary>   The Singleton instance of this class. </summary>
    private static TwitchBot sing;
    /// <summary>   The <see cref="TcpClient"/> of this connection. </summary>
    private TcpClient tcpClient;
    /// <summary>   The <see cref="StreamReader"/> used to read messages from Twitch.   </summary>
    private StreamReader reader;
    /// <summary>   The <see cref="StreamWriter"/> used to write messages to Twitch.   </summary>
    private StreamWriter writer;

    [Header("Bot settings:")]
    [Tooltip("If this is not ticked the connection will be maintained between different scenes.")]
    [SerializeField] private bool destroyOnLoad = true;
    [Tooltip("If ticked this bot will connect to the channel on start.")]
    [SerializeField] private bool autoConnect = true;
    [Tooltip("All message will need to start with this character to be considered as commands. Leave empty if you want to receive a call for every messages.")]
    [SerializeField] private string commandPrefix = "!";
    [Tooltip("This message will be sent by the bot in the chat, when a connection is established.")]
    [SerializeField] private string connectionMessage = "Connessione stabilita.";

    #endregion

    #region PUBLIC_METHODS

    /// <summary>   Start the connection with the server, using the current <see cref="credentials"/>.  </summary>
    public void Connect()
    {
        Connect(credentials);
    }

    /// <summary>   Start the connection with the server, using the passed credentials. </summary>
    /// <param name="credentials">  The credential that will be used to connect with the bot.   </param>
    public void Connect(TwitchCredentials credentials)
    {        
        tcpClient = new TcpClient("irc.chat.twitch.tv", 6667);
        reader = new StreamReader(tcpClient.GetStream());
        writer = new StreamWriter(tcpClient.GetStream()) { NewLine = "\r\n", AutoFlush = true };

        writer.WriteLine("PASS " + credentials.authCode);
        writer.WriteLine("NICK " + credentials.botName);
        writer.WriteLine("USER " + credentials.botName + " 8 * :" + credentials.botName);
        writer.WriteLine("JOIN #" + credentials.channelName);
    }

    /// <summary>   If the bot is connected, this will make it send a message in a specific chennel.    </summary>
    /// <param name="channelName">  The Twitch channel that will receive the message.   </param>
    /// <param name="messageToSend">    The text to be sent.    </param>
    public void SendChatMessage(string channelName, string messageToSend)
    {
        writer.WriteLine($"PRIVMSG #{channelName} :{messageToSend}");
    }

    #endregion

    #region PRIVATE_METHODS

    /// <summary>   Read the chat for new messages, and invoke the <see cref="functionsToBeCalled"/> actions for each message sent in the chat. </summary>
    private void ReadChat()
    {
        if (tcpClient.Available > 0)
        {
            string message = reader.ReadLine();
            
            if (message.Contains("PING"))
            {
                writer.WriteLine("PONG");
                writer.Flush();
                return;
            }

            if (message.Contains("PRIVMSG"))
            {                
                var prefixSplitPoint = message.IndexOf("!", 1);
                var author = message.Substring(0, prefixSplitPoint);
                author = author.Substring(1);               

                var messageSplitPoint = message.IndexOf(":", 1);
                message = message.Substring(messageSplitPoint + 1);

                if (commandPrefix == null) commandPrefix = "";
                if (commandPrefix != "")
                {                    
                    if (message.StartsWith(commandPrefix))
                    {
                        message = message.Substring(commandPrefix.Length);
                    }
                    else
                    {
                        return;
                    }
                }              
                               
                MessageData messageData = new MessageData(author, message);
                functionsToBeCalled.Invoke(messageData);                
            }
        }
    }

    #endregion

    #region UNITY_METHODS

    void Awake()
    {
        sing = this;
        if (!destroyOnLoad) DontDestroyOnLoad(this);            
        if (autoConnect) Connect();
        SendChatMessage(credentials.channelName, connectionMessage);
    }

    void Update()
    {
        if (tcpClient != null && tcpClient.Connected)ReadChat();        
    }
    
    #endregion

}
