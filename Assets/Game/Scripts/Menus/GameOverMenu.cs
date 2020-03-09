using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : Menu
{
    [SerializeField] TMPro.TextMeshProUGUI mGameOverTxt;
    [SerializeField] UnityEngine.UI.Image mGameOverPanel;


    public void SetWinLoss(bool pWon)
    {
        Color aColor;
        if (pWon)
        {
            mGameOverTxt.text = "You Won!!!";
            aColor = Color.green;
            aColor.a = 0.3f;
            mGameOverPanel.color = aColor;
        }
        else
        {
            mGameOverTxt.text = "You Lost!!!";
            aColor = Color.red;
            aColor.a = 0.3f;
            mGameOverPanel.color = aColor;
        }
    }



    public void BackToLobby()
    {
        GameManager.Instance.EndGame();
    }
}
