using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomSelector : MonoBehaviour
{
    [SerializeField] RectTransform mSelfTransform;
    [SerializeField] GameObject mSelectedImage;
    [HideInInspector] public bool mSelected = false;
    [SerializeField] TextMeshProUGUI mRoomName;
    [HideInInspector] public LobbyUI mLobbyUI;

    public void SelectRoom()
    {
        if(mLobbyUI == null)
        {
            return;
        }

    }

}
