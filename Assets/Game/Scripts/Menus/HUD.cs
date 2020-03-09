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
    [SerializeField] TextMeshProUGUI mTimer;

    float mStartTime = 0.0f;

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

    void Update()
    {
        if(mStartTime > 0)
        {
            mStartTime -= Time.deltaTime;
            if(mStartTime <= 0)
            {
                mTimer.text = "";
            }
        }
    }

    public void SetTimer(int pTime)
    {
        if(pTime == 0)
        {
            mTimer.text = "START!";
            mStartTime = 0.3f;
        }
        else
        {
            mTimer.text = pTime.ToString();
        }
    }

}
