using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ErrorMessageMenu : Menu
{
    [SerializeField] TextMeshProUGUI mTitle;
    [SerializeField] TextMeshProUGUI mMessage;
    float mTimer = 0.5f;
    bool mActive = false;

    protected override void OnVisible()
    {
        mActive = true;
    }
    void Update()
    {
        if(!mActive)
        {
            return;
        }
        mTimer -= Time.deltaTime;
        if(mTimer <= 0.0f)
        {
            mActive = false;
            MenuManager.Instance.HideMenu(GameManager.Instance.mErrorMessageMenu);
        }
    }

    public void SetErrorMessage(string pTitle, string pMessage, float pTimer = 0.5f)
    {
        mTitle.text = pTitle;
        mMessage.text = pMessage;
        mTimer = pTimer;
    }


}
