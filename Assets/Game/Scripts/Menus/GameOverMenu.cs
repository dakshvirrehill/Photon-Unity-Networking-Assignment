using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : Menu
{
    [SerializeField] TMPro.TextMeshProUGUI mGameOverTxt;
    [SerializeField] UnityEngine.UI.Image mGameOverPanel;


    public void SetWinLoss(bool pWon)
    {
        if (AudioManager.Instance.IsSoundPlaying("GameMusic"))
        {
            AudioManager.Instance.FadeSound("GameMusic", 0.2f, true);
        }
        Color aColor;
        if (pWon)
        {
            mGameOverTxt.text = "You Won!!!";
            aColor = Color.green;
            aColor.a = 0.3f;
            mGameOverPanel.color = aColor;
            AudioManager.Instance.PlayMusic("VictorySound", false, 0.1f);
            AudioManager.Instance.FadeSound("VictorySound", 0.1f, false, 0.1f);
        }
        else
        {
            mGameOverTxt.text = "You Lost!!!";
            aColor = Color.red;
            aColor.a = 0.3f;
            mGameOverPanel.color = aColor;
            AudioManager.Instance.PlayMusic("LossSound", false, 0.1f);
            AudioManager.Instance.FadeSound("LossSound", 0.1f, false, 0.1f);
        }
    }



    public void BackToLobby()
    {
        GameManager.Instance.EndGame();
    }
}
