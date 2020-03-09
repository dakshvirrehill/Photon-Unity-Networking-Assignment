using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class Player : MonoBehaviourPun, IPunObservable
{
    public SpriteRenderer mRenderer;
    public Rigidbody2D mRigidbody;
    public Movement mMovement;
    public Animator mAnimator;

    public float mHealth = 10;
    public float mHitDamage = 1;
    [HideInInspector]public bool mAttacking = false;

    public float mAttackTimer = 0.2f;
    float mCurAttackTimer = 0.0f;

    void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            mRenderer.color = photonView.IsMine ? Color.yellow : Color.green;
        }
        else
        {
            mRenderer.color = photonView.IsMine ? Color.green : Color.yellow;
        }
        mMovement.mActive = photonView.IsMine;
    }

    void Update()
    {
        if(!photonView.IsMine)
        {
            return;
        }
        if (mAttacking)
        {
            mCurAttackTimer += Time.deltaTime;
            if(mCurAttackTimer > mAttackTimer)
            {
                mCurAttackTimer = 0.0f;
                mAttacking = false;
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        stream.Serialize(ref mAttacking);
        stream.Serialize(ref mHealth);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(!photonView.IsMine)
        {
            return;
        }
        if(mAttacking)
        {
            photonView.RPC("CauseDamage", RpcTarget.AllViaServer,
                GameSceneHandler.Instance.mPlayerId);
        }
    }

    [PunRPC]
    public void CauseDamage(int pOpponentId)
    {
        if(pOpponentId == GameSceneHandler.Instance.mPlayerId)
        {
            return;
        }
        if(mHealth <= 0)
        {
            return;
        }
        mHealth -= mHitDamage;
        mMovement.HurtPlayer();
        if(mHealth <= 0)
        {
            PhotonNetwork.RaiseEvent(
                    GenEvents.GameEnd,
                    GameSceneHandler.Instance.mPlayerId,
                    new RaiseEventOptions { Receivers = ReceiverGroup.All },
                    new SendOptions { Reliability = true });
            mMovement.KillPlayer();
        }
    }

}
