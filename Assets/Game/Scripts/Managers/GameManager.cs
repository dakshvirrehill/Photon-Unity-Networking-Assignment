using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector] public bool mConnected = false;
    [HideInInspector] public bool mConnecting = false;
    [HideInInspector] public bool mJoiningOrCreatingRoom = false;
    [HideInInspector] public bool mJoinedRoom = false;
    public NetworkCallbackHandler mNetworkCallbacks;
    readonly TypedLobby mGameLobby = new TypedLobby("NinjaBrawl", LobbyType.Default);

    #region Menu Classifiers
    [Header("Menu Classifiers")]
    public MenuClassifier mMainMenu;
    public MenuClassifier mLobby;
    public MenuClassifier mHUD;
    public MenuClassifier mNetworkwaitMenu;
    public MenuClassifier mGameOverMenu;
    public MenuClassifier mErrorMessageMenu;
    #endregion

    #region Scene References
    [Header("Scene References")]
    public SceneReference mUIScene;
    public SceneReference mGameScene;
    #endregion


    void Start()
    {
        mNetworkCallbacks.mConnectedToMaster.AddListener(OnConnectedToMaster);
        MultiSceneManager.Instance.LoadScene(mUIScene);
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
        PhotonNetwork.JoinLobby(mGameLobby);
    }

    public void ConnectToMaster()
    {
        mConnecting = true;
        mConnected = false;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void JoinRoom(string pRoomName)
    {
        mJoiningOrCreatingRoom = true;
        mJoinedRoom = false;
        PhotonNetwork.JoinRoom(pRoomName);
    }

    public void CreateRoom(string pRoomName)
    {
        mJoiningOrCreatingRoom = true;
        mJoinedRoom = false;
        RoomOptions aRoomOptions = new RoomOptions();
        aRoomOptions.MaxPlayers = 2;
        aRoomOptions.IsOpen = aRoomOptions.IsVisible = true;
        PhotonNetwork.CreateRoom(pRoomName, aRoomOptions, mGameLobby);
    }


    public void StartGame()
    {
        mJoiningOrCreatingRoom = false;
        mJoinedRoom = true;
        MultiSceneManager.Instance.LoadScene(mGameScene);
    }

    public void JoinOrCreateFailed()
    {
        mJoiningOrCreatingRoom = false;
        mJoinedRoom = false;
    }

    public void EndGame()
    {
        mJoinedRoom = false;
        PhotonNetwork.LeaveRoom();
        MultiSceneManager.Instance.UnloadScene(mGameScene);
        MenuManager.Instance.HideMenu(mHUD);
        MenuManager.Instance.HideMenu(mGameOverMenu);
        MenuManager.Instance.ShowMenu(mNetworkwaitMenu);
    }


}
