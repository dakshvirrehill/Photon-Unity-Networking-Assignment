using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Realtime;

public class SceneLoadedEvent : UnityEvent<List<string>> { }

public class RoomListEvent : UnityEvent<List<RoomInfo>> { }

public class JoinRoomFailedEvent : UnityEvent<short, string> { }