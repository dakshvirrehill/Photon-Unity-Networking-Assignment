using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkWaitMenu : Menu
{
    public override void Start()
    {
        base.Start();
        GameManager.Instance.mNetworkCallbacks.mConnectedToMaster.AddListener(ConnectedToMaster);
        GameManager.Instance.mNetworkCallbacks.mJoinedRoom.AddListener(JoinedRoom);
        GameManager.Instance.mNetworkCallbacks.mLeftRoom.AddListener(LeftRoom);
        GameManager.Instance.mNetworkCallbacks.mJoinRoomFailed.AddListener(JoinedRoomFailed);
        GameManager.Instance.mNetworkCallbacks.mCreateRoomFailed.AddListener(CreateRoomFailed);
    }

    void OnDestroy()
    {
        if(!GameManager.IsValidSingleton())
        {
            return;
        }
        GameManager.Instance.mNetworkCallbacks.mConnectedToMaster.RemoveListener(ConnectedToMaster);
        GameManager.Instance.mNetworkCallbacks.mJoinedRoom.RemoveListener(JoinedRoom);
        GameManager.Instance.mNetworkCallbacks.mLeftRoom.RemoveListener(LeftRoom);
        GameManager.Instance.mNetworkCallbacks.mJoinRoomFailed.RemoveListener(JoinedRoomFailed);
        GameManager.Instance.mNetworkCallbacks.mCreateRoomFailed.RemoveListener(CreateRoomFailed);
    }


    void ConnectedToMaster()
    {
        if(!gameObject.activeInHierarchy)
        {
            return;
        }
        MenuManager.Instance.ShowMenu(GameManager.Instance.mLobby);
        MenuManager.Instance.HideMenu(GameManager.Instance.mNetworkwaitMenu);
    }

    void JoinedRoom()
    {
        if(!gameObject.activeInHierarchy)
        {
            return;
        }
        GameManager.Instance.StartGame();
        MenuManager.Instance.HideMenu(GameManager.Instance.mNetworkwaitMenu);
    }

    void JoinedRoomFailed(short returnCode, string message)
    {
        GameManager.Instance.JoinOrCreateFailed();
        ErrorMessageMenu aMenu = MenuManager.Instance.GetMenu<ErrorMessageMenu>(GameManager.Instance.mErrorMessageMenu);
        aMenu.SetErrorMessage("Joining Room Failed", message, 1.0f);
        if (!aMenu.gameObject.activeInHierarchy)
        {
            MenuManager.Instance.ShowMenu(GameManager.Instance.mErrorMessageMenu);
        }
        MenuManager.Instance.ShowMenu(GameManager.Instance.mLobby);
        MenuManager.Instance.HideMenu(GameManager.Instance.mNetworkwaitMenu);
    }

    void CreateRoomFailed(short returnCode, string message)
    {
        GameManager.Instance.JoinOrCreateFailed();
        ErrorMessageMenu aMenu = MenuManager.Instance.GetMenu<ErrorMessageMenu>(GameManager.Instance.mErrorMessageMenu);
        aMenu.SetErrorMessage("Creating Room Failed", message, 1.0f);
        if (!aMenu.gameObject.activeInHierarchy)
        {
            MenuManager.Instance.ShowMenu(GameManager.Instance.mErrorMessageMenu);
        }
        MenuManager.Instance.ShowMenu(GameManager.Instance.mLobby);
        MenuManager.Instance.HideMenu(GameManager.Instance.mNetworkwaitMenu);
    }

    void LeftRoom()
    {
        if(!gameObject.activeInHierarchy)
        {
            return;
        }
        MenuManager.Instance.ShowMenu(GameManager.Instance.mLobby);
        MenuManager.Instance.HideMenu(GameManager.Instance.mNetworkwaitMenu);
    }
}
