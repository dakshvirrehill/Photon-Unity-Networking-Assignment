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
