using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public struct PlayerHUDData
{
    public Image mLifeImage;
    public TextMeshProUGUI mNickname;
}


public class HUD : Menu
{
    [SerializeField] PlayerHUDData mPlayerOneData;
    [SerializeField] PlayerHUDData mPlayerTwoData;

    public void PlayerOneInit(string pNickname)
    {
        mPlayerOneData.mNickname.text = pNickname;
        mPlayerOneData.mLifeImage.fillAmount = 1;
    }

    public void PlayerTwoInit(string pNickname)
    {
        mPlayerTwoData.mNickname.text = pNickname;
        mPlayerTwoData.mLifeImage.fillAmount = 1;
    }

    public void UpdatePlayerOneHealth(float pFillAmount)
    {
        mPlayerOneData.mLifeImage.fillAmount = pFillAmount;
    }
    public void UpdatePlayerTwoHealth(float pFillAmount)
    {
        mPlayerTwoData.mLifeImage.fillAmount = pFillAmount;
    }

}
