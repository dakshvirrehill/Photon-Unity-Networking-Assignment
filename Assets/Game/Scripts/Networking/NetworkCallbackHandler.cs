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

    public override void OnConnectedToMaster()
    {
        mConnectedToMaster.Invoke();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        mRoomListUpdate.Invoke(roomList);
    }

}
