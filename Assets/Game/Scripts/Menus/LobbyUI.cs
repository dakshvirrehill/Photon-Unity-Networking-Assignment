using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class LobbyUI : Menu
{
    public string mPoolName = "RoomSelect";
    [SerializeField] RectTransform mRoomListParent;
    [SerializeField] VerticalLayoutGroup mRoomListLayoutGroup;
    public TMPro.TMP_InputField mLobbyNameImpField;
    [SerializeField] Button mJoinLobbyBtn;
    [SerializeField] Button mCreateLobbyBtn;
    readonly Dictionary<string, RoomSelector> mRoomSelectors = new Dictionary<string, RoomSelector>();
    [HideInInspector] public RoomSelector mSelectedRoom = null;
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

    protected override void OnVisible()
    {
        OnValueChanged(mLobbyNameImpField.text);
    }

    void OnEnable()
    {
        if (!AudioManager.Instance.IsSoundPlaying("MenuMusic"))
        {
            AudioManager.Instance.PlayMusic("MenuMusic", true, 0.1f);
            AudioManager.Instance.FadeSound("MenuMusic", 0.2f, false, 0.1f);
        }
    }

    void OnDisable()
    {
        if (!AudioManager.IsValidSingleton())
        {
            return;
        }
        if (AudioManager.Instance.IsSoundPlaying("MenuMusic"))
        {
            AudioManager.Instance.FadeSound("MenuMusic", 0.2f, true);
        }
        mLobbyNameImpField.SetTextWithoutNotify("");
    }


    void OnRoomListUpdate(List<RoomInfo> pRooms)
    {
        List<string> aCurrentRooms = new List<string>(mRoomSelectors.Keys);
        Dictionary<string,RoomInfo> aLobbyRooms = new Dictionary<string, RoomInfo>();
        foreach(RoomInfo aRoom in pRooms)
        {
            aLobbyRooms.Add(aRoom.Name, aRoom);
        }
        foreach (string aCurRoom in aCurrentRooms)
        {
            if(!aLobbyRooms.ContainsKey(aCurRoom))
            {
                RemoveRoom(aCurRoom);
            }
            else if(aLobbyRooms[aCurRoom].RemovedFromList || !aLobbyRooms[aCurRoom].IsOpen
                || !aLobbyRooms[aCurRoom].IsVisible)
            {
                RemoveRoom(aCurRoom);
            }
            else
            {
                mRoomSelectors[aCurRoom].transform.SetAsFirstSibling();
            }
        }
        aCurrentRooms = new List<string>(aLobbyRooms.Keys);
        foreach(string aCurrentRoom in aCurrentRooms)
        {
            if (aLobbyRooms[aCurrentRoom].RemovedFromList || !aLobbyRooms[aCurrentRoom].IsOpen
                || !aLobbyRooms[aCurrentRoom].IsVisible)
            {
                continue;
            }
                if (!mRoomSelectors.ContainsKey(aCurrentRoom))
            {
                AddNewRoom(aLobbyRooms[aCurrentRoom]);
            }
        }
    }

    void RemoveRoom(string pRoomName)
    {
        mRoomSelectors[pRoomName].UnSelectRoom();
        mRoomListParent.sizeDelta -= new Vector2(0, mRoomSelectors[pRoomName].mSelfTransform.sizeDelta.y + mRoomListLayoutGroup.spacing);
        ObjectPoolingManager.Instance.ReturnPooledObject(mPoolName, mRoomSelectors[pRoomName].gameObject);
        mRoomSelectors[pRoomName].gameObject.SetActive(false);
        mRoomSelectors.Remove(pRoomName);
    }

    void AddNewRoom(RoomInfo pRoom)
    {
        GameObject aRoomSelectorObj = ObjectPoolingManager.Instance.GetPooledObject(mPoolName);
        RoomSelector aRoomSelector = aRoomSelectorObj.GetComponent<RoomSelector>();
        aRoomSelector.SetRoomName(pRoom.Name);
        mRoomSelectors.Add(pRoom.Name, aRoomSelector);
        mRoomListParent.sizeDelta += new Vector2(0, aRoomSelector.mSelfTransform.sizeDelta.y + mRoomListLayoutGroup.spacing);
        aRoomSelectorObj.SetActive(true);
        aRoomSelectorObj.transform.SetAsFirstSibling();
    }

    public void OnValueChanged(string pValue)
    {
        if(string.IsNullOrEmpty(mLobbyNameImpField.text))
        {
            mCreateLobbyBtn.interactable = false;
            mJoinLobbyBtn.interactable = false;
            if(mSelectedRoom != null)
            {
                mSelectedRoom.UnSelectRoom();
            }
            return;
        }
        if(mRoomSelectors.ContainsKey(mLobbyNameImpField.text))
        {
            if(mSelectedRoom != null)
            {
                if (mRoomSelectors[mLobbyNameImpField.text].gameObject.GetInstanceID() != mSelectedRoom.gameObject.GetInstanceID())
                {
                    mRoomSelectors[mLobbyNameImpField.text].SelectUnselectRoom();
                }
            }
            mCreateLobbyBtn.interactable = false;
            mJoinLobbyBtn.interactable = true;
            if(mSelectedRoom == null)
            {
                mRoomSelectors[mLobbyNameImpField.text].SelectUnselectRoom();
            }
            return;
        }
        if(mSelectedRoom != null)
        {
            mSelectedRoom.UnSelectRoom();
        }
        mCreateLobbyBtn.interactable = true;
        mJoinLobbyBtn.interactable = false;
    }

    public void JoinSelectedLobby()
    {
        if(GameManager.Instance.mJoinedRoom || GameManager.Instance.mJoiningOrCreatingRoom)
        {
            return;
        }
        if(mSelectedRoom == null)
        {
            return;
        }
        GameManager.Instance.JoinRoom(mSelectedRoom.GetRoomName());
        ShowNetworkWaitMenu();
    }

    public void CreateLobby()
    {
        if (GameManager.Instance.mJoinedRoom || GameManager.Instance.mJoiningOrCreatingRoom)
        {
            return;
        }
        GameManager.Instance.CreateRoom(mLobbyNameImpField.text);
        ShowNetworkWaitMenu();
    }

    void ShowNetworkWaitMenu()
    {
        MenuManager.Instance.ShowMenu(GameManager.Instance.mNetworkwaitMenu);
        MenuManager.Instance.HideMenu(GameManager.Instance.mLobby);
    }

}
