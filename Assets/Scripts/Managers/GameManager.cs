using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string gamePhase;

    #region PUBLIC_METHODS
    public void TwitchInputHandler(MessageData messageData)
    {
        switch (messageData.message)
        {
            case "j":
                TowerManager.Instance.JoinGame(messageData.nickname);
                break;
            case "join":
                TowerManager.Instance.JoinGame(messageData.nickname);
                break;
            case "f":
                TowerManager.Instance.SelectMove(messageData.nickname, 0);
                break;
            case "fire":
                TowerManager.Instance.SelectMove(messageData.nickname, 0);
                break;
            case "i":
                TowerManager.Instance.SelectMove(messageData.nickname, 1);
                break;
            case "ice":
                TowerManager.Instance.SelectMove(messageData.nickname, 1);
                break;
            case "w":
                TowerManager.Instance.SelectMove(messageData.nickname, 2);
                break;
            case "wind":
                TowerManager.Instance.SelectMove(messageData.nickname, 2);
                break;
            case "1":
                TowerManager.Instance.AssignModifier(1);
                break;
            case "2":
                TowerManager.Instance.AssignModifier(2);
                break;
            default:

                break;
        }
    }
    #endregion

    public void ResetGame()
    {
        TowerManager.Instance.ResetTowers();
    }

    #region UNITY_METHODS
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        gamePhase = "Lobby";

    }
    #endregion
}
