using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;

public class NetworkCallbackHandler : MonoBehaviourPunCallbacks
{
    public readonly UnityEvent mConnectedToMaster = new UnityEvent();
    public readonly RoomListEvent mRoomListUpdate = new RoomListEvent();
    public readonly UnityEvent mJoinedRoom = new UnityEvent();
    public readonly UnityEvent mLeftRoom = new UnityEvent();
    public readonly JoinRoomFailedEvent mJoinRoomFailed = new JoinRoomFailedEvent();
    public readonly JoinRoomFailedEvent mCreateRoomFailed = new JoinRoomFailedEvent();

    public override void OnConnectedToMaster()
    {
        mConnectedToMaster.Invoke();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        mRoomListUpdate.Invoke(roomList);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Lobby Joined");
    }

    public override void OnJoinedRoom()
    {
        mJoinedRoom.Invoke();
    }

    public override void OnLeftRoom()
    {
        mLeftRoom.Invoke();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        mJoinRoomFailed.Invoke(returnCode, message);
    }

    public override void OnCreatedRoom()
    {
        
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        mCreateRoomFailed.Invoke(returnCode, message);
    }

}
