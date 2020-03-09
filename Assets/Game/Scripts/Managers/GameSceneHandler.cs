using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class GameSceneHandler : Singleton<GameSceneHandler>
{
    [SerializeField] Transform mPlayerOneSpawnPosition;
    [SerializeField] Transform mPlayerTwoSpawnPosition;
    [SerializeField] string mPlayerPrefab = "Player";
    [HideInInspector] public bool mGameStarted = false;
    [HideInInspector] public int mPlayerId;
    [HideInInspector] public Player mLocalPlayer;
    [SerializeField] string mDelayTimer = "StartTimer";
    [HideInInspector] public DelayTimer mTimer = null;
    void Start()
    {
        int aMasterId = PhotonNetwork.CurrentRoom.MasterClientId;
        if(PhotonNetwork.IsMasterClient)
        {
            mPlayerId = aMasterId;
        }
        else
        {
            foreach(int aPlayerId in PhotonNetwork.CurrentRoom.Players.Keys)
            {
                if(aMasterId != aPlayerId)
                {
                    mPlayerId = aPlayerId;
                    break;
                }
            }
        }
        MenuManager.Instance.ShowMenu(GameManager.Instance.mNetworkwaitMenu);
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
        if (!PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.RaiseEvent(
                GenEvents.SecondPlayerReadyEvent,
                null,
                new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient },
                new SendOptions { Reliability = true });
        }
    }

    void OnDestroy()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    void Update()
    {
        if(!mGameStarted)
        {
            if(mTimer == null)
            {
                return;
            }
            mLocalPlayer.mCurrHUD.SetTimer((int)mTimer.mTimer);
            if(mTimer.mTimer <= 0)
            {
                mGameStarted = true;
                mLocalPlayer.mMovement.mActive = true;
            }
        }
    }



    void OnEvent(EventData pEventData)
    {
        if(pEventData.Code == GenEvents.SecondPlayerReadyEvent)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.RaiseEvent(
                    GenEvents.StartGame,
                    null,
                    new RaiseEventOptions { Receivers = ReceiverGroup.All },
                    new SendOptions { Reliability = true });
            }
        }
        else if(pEventData.Code == GenEvents.StartGame)
        {
            StartGameScene();
        }
    }

    void StartGameScene()
    {
        GameObject aPlayer = null;
        if (PhotonNetwork.IsMasterClient)
        {
            aPlayer = PhotonNetwork.Instantiate(
                mPlayerPrefab, 
                mPlayerOneSpawnPosition.position, 
                mPlayerOneSpawnPosition.rotation);
            GameObject aTimer = PhotonNetwork.Instantiate(
                mDelayTimer,
                Vector3.zero,
                Quaternion.identity
                );
        }
        else
        {
            aPlayer = PhotonNetwork.Instantiate(
                mPlayerPrefab,
                mPlayerTwoSpawnPosition.position,
                mPlayerTwoSpawnPosition.rotation);
        }
        mLocalPlayer = aPlayer.GetComponent<Player>();
    }


}
