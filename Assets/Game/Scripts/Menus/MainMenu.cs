using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : Menu
{
    [SerializeField] TMP_InputField mNicknameImpField;
    [SerializeField] Button mConnectButton;
    [SerializeField] TextMeshProUGUI mConnectButtonText;
    void OnEnable()
    {
        mConnectButton.interactable = (!string.IsNullOrEmpty(mNicknameImpField.text)
            && !GameManager.Instance.mConnecting);
        mConnectButtonText.text = "Connect";
    }

    public void OnValueChanged(string pInputFieldText)
    {
        mConnectButton.interactable = (!string.IsNullOrEmpty(mNicknameImpField.text)
                        && !GameManager.Instance.mConnecting);
    }

    public void ConnectToServer()
    {
        if(GameManager.Instance.mConnecting)
        {
            return;
        }
        mConnectButton.interactable = false;
        Photon.Pun.PhotonNetwork.LocalPlayer.NickName = mNicknameImpField.text;
        GameManager.Instance.ConnectToMaster();
        mConnectButtonText.text = "Connecting";
        MenuManager.Instance.ShowMenu(GameManager.Instance.mNetworkwaitMenu);
        MenuManager.Instance.HideMenu(GameManager.Instance.mMainMenu);
    }

}
