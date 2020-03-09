using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviourPun, IPunObservable
{
    public SpriteRenderer mRenderer;
    public Rigidbody2D mRigidbody;
    public Movement mMovement;
    public Animator mAnimator;

    public float mHealth = 10;
    public float mHitDamage = 1;
    [HideInInspector] public bool mInAttack = false;
    [HideInInspector] public Vector2 mDodgeDirection = Vector2.zero;
    bool mAttacking = false;
    bool mAttacked = false;
    public float mAttackTimer = 0.2f;
    float mCurAttackTimer = 0.0f;
    [HideInInspector] public HUD mCurrHUD = null;
    void Start()
    {
        mCurrHUD = MenuManager.Instance.GetMenu<HUD>(GameManager.Instance.mHUD);
        if (PhotonNetwork.IsMasterClient)
        {
            mRenderer.color = photonView.IsMine ? Color.yellow : Color.green;
            if (photonView.IsMine)
            {
                mCurrHUD.PlayerOneInit(photonView.Owner.NickName);
            }
            else
            {
                mCurrHUD.PlayerTwoInit(photonView.Owner.NickName);
            }
        }
        else
        {
            mRenderer.color = photonView.IsMine ? Color.green : Color.yellow;
            if (photonView.IsMine)
            {
                mCurrHUD.PlayerTwoInit(photonView.Owner.NickName);
            }
            else
            {
                mCurrHUD.PlayerOneInit(photonView.Owner.NickName);
            }
        }
        if(!photonView.IsMine)
        {
            MenuManager.Instance.HideMenu(GameManager.Instance.mNetworkwaitMenu);
            MenuManager.Instance.ShowMenu(GameManager.Instance.mHUD);
        }
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        if (mAttacking)
        {
            mCurAttackTimer += Time.deltaTime;
            if (mCurAttackTimer > mAttackTimer)
            {
                CreateAttack();
                mAttacking = false;
            }
        }
    }

    public void CreateAttack()
    {
        mCurAttackTimer = 0.0f;
        mAttacking = true;
        mAttacked = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        stream.Serialize(ref mAttacking);
        stream.Serialize(ref mHealth);
        if (stream.IsReading)
        {
            UpdateHealth();
        }
    }

    void UpdateHealth()
    {
        if (mCurrHUD == null)
        {
            mCurrHUD = MenuManager.Instance.GetMenu<HUD>(GameManager.Instance.mHUD);
        }
        if (PhotonNetwork.IsMasterClient)
        {
            if (photonView.IsMine)
            {
                mCurrHUD.UpdatePlayerOneHealth(mHealth / 10.0f);
            }
            else
            {
                mCurrHUD.UpdatePlayerTwoHealth(mHealth / 10.0f);
            }
        }
        else
        {
            if (photonView.IsMine)
            {
                mCurrHUD.UpdatePlayerTwoHealth(mHealth / 10.0f);
            }
            else
            {
                mCurrHUD.UpdatePlayerOneHealth(mHealth / 10.0f);
            }
        }
    }

    void CheckForAttack(Collision2D collision)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        Player aPlayer = collision.collider.GetComponent<Player>();
        if (aPlayer != null)
        {
            mInAttack = true;
            mDodgeDirection = transform.position - 
                aPlayer.transform.position;
            if(mDodgeDirection.x < 0)
            {
                mDodgeDirection.x = -1.0f;
            }
            else if(mDodgeDirection.x > 0)
            {
                mDodgeDirection.x = 1.0f;
            }
            if (!mAttacking || mAttacked)
            {
                return;
            }
            aPlayer.photonView.RPC("CauseDamage", RpcTarget.All,
                    GameSceneHandler.Instance.mPlayerId);
            mAttacked = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        CheckForAttack(collision);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        CheckForAttack(collision);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        Player aPlayer = collision.collider.GetComponent<Player>();
        if (aPlayer != null)
        {
            mInAttack = false;
            mDodgeDirection = Vector2.zero;
        }
    }

    [PunRPC]
    public void CauseDamage(int pOpponentId)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        if (pOpponentId == GameSceneHandler.Instance.mPlayerId)
        {
            return;
        }
        if (mHealth <= 0)
        {
            return;
        }
        mHealth -= mHitDamage;
        UpdateHealth();
        mMovement.HurtPlayer();
        if (mHealth <= 0)
        {
            mMovement.KillPlayer();
            photonView.RPC("DisplayGameEnd",
                PhotonNetwork.CurrentRoom.GetPlayer(pOpponentId), 
                true);
            DisplayGameEnd(false);
        }
    }

    [PunRPC]
    public void DisplayGameEnd(bool pWon)
    {
        GameSceneHandler.Instance.mLocalPlayer.mMovement.mActive = false;
        GameSceneHandler.Instance.mLocalPlayer.mRigidbody.velocity = Vector2.zero;
        GameSceneHandler.Instance.mLocalPlayer.mAnimator.SetFloat(mMovement.mHorizontalParam, 0);
        MenuManager.Instance.ShowMenu(GameManager.Instance.mGameOverMenu);
        GameOverMenu aGameOverMenu = MenuManager.Instance.GetMenu<GameOverMenu>(GameManager.Instance.mGameOverMenu);
        aGameOverMenu.SetWinLoss(pWon);
    }

    [PunRPC]
    public void PlayDodge()
    {
        AudioManager.Instance.PlaySFX("DodgeSound");
    }

}
