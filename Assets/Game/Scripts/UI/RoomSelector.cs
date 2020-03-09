using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomSelector : MonoBehaviour, IInitable
{
    public RectTransform mSelfTransform;
    [SerializeField] GameObject mSelectedImage;
    [HideInInspector] public bool mSelected = false;
    [SerializeField] TextMeshProUGUI mRoomName;
    [SerializeField] UnityEngine.UI.Button mRoomBtn;
    LobbyUI mLobbyUI;

    public void Init()
    {
        SetLobbyReference();
    }

    void SetLobbyReference()
    {
        if (mLobbyUI == null)
        {
            mLobbyUI = GetComponentInParent<LobbyUI>();
        }
    }

    void OnEnable()
    {
        SetLobbyReference();
        mRoomBtn.interactable = true;
    }

    void OnDisable()
    {
        mSelectedImage.SetActive(false);
        mSelected = false;
        mRoomName.text = "";
        mRoomBtn.interactable = false;
    }

    public void SelectUnselectRoom()
    {
        if(mLobbyUI == null)
        {
            return;
        }
        mRoomBtn.interactable = false;
        if(mLobbyUI.mSelectedRoom != null)
        {
            bool aReturn = mLobbyUI.mSelectedRoom.gameObject.GetInstanceID() == gameObject.GetInstanceID();
            mLobbyUI.mSelectedRoom.UnSelectRoom();
            if(aReturn)
            {
                mRoomBtn.interactable = true;
                return;
            }
        }
        mSelected = !mSelected;
        mSelectedImage.SetActive(mSelected);
        if (mSelected)
        {
            mLobbyUI.mSelectedRoom = this;
            mLobbyUI.mLobbyNameImpField.text = mRoomName.text;
        }
        mRoomBtn.interactable = true;
    }
    public void UnSelectRoom()
    {
        if(!mSelected)
        {
            return;
        }
        mSelected = false;
        mLobbyUI.mSelectedRoom = null;
        mSelectedImage.SetActive(false);
        mLobbyUI.mLobbyNameImpField.text = "";
    }

    public void SetRoomName(string pName)
    {
        mRoomName.text = pName;
    }

    public string GetRoomName()
    {
        return mRoomName.text;
    }

}
