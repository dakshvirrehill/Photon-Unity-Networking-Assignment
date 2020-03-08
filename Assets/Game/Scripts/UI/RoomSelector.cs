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
        mSelected = !mSelected;
        mSelectedImage.SetActive(mSelected);
        if(mLobbyUI.mSelectedRoom != null)
        {
            mLobbyUI.mSelectedRoom.UnSelectRoom();
            mLobbyUI.mSelectedRoom = this;
            mLobbyUI.mLobbyNameImpField.SetTextWithoutNotify(mRoomName.text);
        }
        mRoomBtn.interactable = true;
    }
    public void UnSelectRoom()
    {
        mSelected = false;
        mSelectedImage.SetActive(false);
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
