using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Realtime;

public class SceneLoadedEvent : UnityEvent<List<string>> { }

public class RoomListEvent : UnityEvent<List<RoomInfo>> { }

public class JoinRoomFailedEvent : UnityEvent<short, string> { }


public class GenEvents
{
    public static readonly byte SecondPlayerReadyEvent = 1;
    public static readonly byte StartGame = 2;
    public static readonly byte GameEnd = 3;
}
