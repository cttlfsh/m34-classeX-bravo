using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : UIManager
{
    public GameObject howToPlayPanel;
    public void GoBackButton()
    {
        gameObject.SetActive(false);
    }

    public void NextButtonPress()
    {
        howToPlayPanel.SetActive(false);
    }
    public void PlayButtonPress()
    {

    }
}
