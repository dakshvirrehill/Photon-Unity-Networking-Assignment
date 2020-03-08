using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class LobbyUI : Menu
{

    public readonly Dictionary<string, RoomSelector> mRoomSelectors = new Dictionary<string, RoomSelector>();

    public override void Start()
    {
        base.Start();
        GameManager.Instance.mNetworkCallbacks.mRoomListUpdate.AddListener(OnRoomListUpdate);
    }


    void OnDestroy()
    {
        if(!GameManager.IsValidSingleton())
        {
            return;
        }
        GameManager.Instance.mNetworkCallbacks.mRoomListUpdate.RemoveListener(OnRoomListUpdate);
    }


    void OnRoomListUpdate(List<RoomInfo> pRooms)
    {

    }

}
