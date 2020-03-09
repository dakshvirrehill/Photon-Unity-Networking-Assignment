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
    bool[] mPlayedSound = new bool[] { false, false, false, false };
    void OnDisable()
    {
        if (!AudioManager.IsValidSingleton())
        {
            return;
        }
        if (AudioManager.Instance.IsSoundPlaying("GameMusic"))
        {
            AudioManager.Instance.FadeSound("GameMusic", 0.2f, true);
        }
        mPlayedSound = new bool[] { false,false,false,false};
    }

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
                if (!AudioManager.Instance.IsSoundPlaying("GameMusic"))
                {
                    AudioManager.Instance.PlayMusic("GameMusic", true, 0.1f);
                    AudioManager.Instance.FadeSound("GameMusic", 0.2f, false, 0.1f);
                }
            }
        }
    }

    public void SetTimer(int pTime)
    {
        if(pTime == 0)
        {
            mTimer.text = "START!";
            mStartTime = 0.3f;
            if(!mPlayedSound[pTime])
            {
                mPlayedSound[pTime] = true;
                AudioManager.Instance.PlaySFX("FinalTapSound");
            }
        }
        else
        {
            mTimer.text = pTime.ToString();
            if(!mPlayedSound[pTime])
            {
                mPlayedSound[pTime] = true;
                AudioManager.Instance.PlaySFX("TapSound");

            }
        }
    }

}
