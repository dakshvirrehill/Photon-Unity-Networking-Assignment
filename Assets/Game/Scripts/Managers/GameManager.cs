using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector] public bool mConnected = false;
    [HideInInspector] public bool mConnecting = false;

    public NetworkCallbackHandler mNetworkCallbacks;


    #region Menu Classifiers
    [Header("Menu Classifiers")]
    public MenuClassifier mMainMenu;
    public MenuClassifier mLobby;
    public MenuClassifier mHUD;
    #endregion

    #region Scene References
    [Header("Scene References")]
    public SceneReference mUIScene;
    public SceneReference mGameScene;
    #endregion


    void Start()
    {
        mNetworkCallbacks.mConnectedToMaster.AddListener(OnConnectedToMaster);
    }

    void OnDestroy()
    {
        if(mNetworkCallbacks != null)
        {
            mNetworkCallbacks.mConnectedToMaster.RemoveListener(OnConnectedToMaster);
        }
    }

    void OnConnectedToMaster()
    {
        mConnecting = false;
        mConnected = true;
        PhotonNetwork.JoinLobby(new TypedLobby("NinjaBrawl", LobbyType.Default));
    }


    public void ConnectToMaster()
    {
        mConnecting = true;
        mConnected = false;
        PhotonNetwork.ConnectUsingSettings();
    }


}
