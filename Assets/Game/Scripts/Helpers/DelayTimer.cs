using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class DelayTimer : MonoBehaviourPun,IPunObservable
{
    public float mTimer = 3.0f;

    void Start()
    {
        GameSceneHandler.Instance.mTimer = this;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        stream.Serialize(ref mTimer);
    }

    void Update()
    {
        if (mTimer <= 0)
        {
            return;
        }
        if (PhotonNetwork.IsMasterClient)
        {
            mTimer -= Time.deltaTime;
        }
    }


}
