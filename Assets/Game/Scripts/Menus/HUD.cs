using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public struct PlayerHUDData
{
    public Image mLifeImage;
    public TextMeshProUGUI mNickname;
}


public class HUD : Menu
{
    [SerializeField] PlayerHUDData mPlayerOneData;
    [SerializeField] PlayerHUDData mPlayerTwoData;



}
