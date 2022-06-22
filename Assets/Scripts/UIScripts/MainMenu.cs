using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : UIManager

{
    public GameObject howToPlayPanel;
    public void PressLobbyButton()
    {
        howToPlayPanel.SetActive(true);
        lobbyCanvas.SetActive(true);
    }
    public void PressLeaderboardButton()
    {
        leaderboardCanvas.SetActive(true);
    }
    public void PressExitButton()
    {
       Application.Quit();
    }
    public void PressCreditsButton()
    {
        creditsCanvas.SetActive(true);
    }

}
