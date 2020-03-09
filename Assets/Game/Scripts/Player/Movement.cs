using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Player))]
public class Movement : MonoBehaviour
{
    [HideInInspector] public bool mActive = false;
    float mHorizontal = 0.0f;
    Player mPlayer;
    [SerializeField] float mSpeed = 10;
    #region Animator Parameters
    [Header("AnimatorParameters")]
    public string mHorizontalParam;
    [SerializeField] string mAttackParam;
    [SerializeField] string mHurtParam;
    [SerializeField] string mDeadParam;
    #endregion
    bool mDodge = false;
    void Start()
    {
        mPlayer = GetComponent<Player>();
    }
    void Update()
    {
        mPlayer.mRenderer.flipX = mPlayer.mAnimator.GetFloat(mHorizontalParam) > 0;
        if (!mActive)
        {
            return;
        }
        mHorizontal = Input.GetAxis("Horizontal");
        mPlayer.mAnimator.SetFloat(mHorizontalParam, mHorizontal);
        if(Input.GetButtonDown("Fire1"))
        {
            mPlayer.mAnimator.SetTrigger(mAttackParam);
            mPlayer.CreateAttack();
        }
        mDodge = Input.GetButtonDown("Jump") && mPlayer.mInAttack;
    }

    void FixedUpdate()
    {
        if(!mActive)
        {
            return;
        }
        mPlayer.mRigidbody.velocity = new Vector2(
                mHorizontal * mSpeed,
                mPlayer.mRigidbody.velocity.y
        );
        if (mDodge)
        {
            RaycastHit2D aHit = Physics2D.Raycast(transform.position, mPlayer.mDodgeDirection.normalized, mPlayer.mDodgeDirection.magnitude * mSpeed,LayerMask.GetMask("Wall"));
            if (aHit.collider == null)
            {
                mPlayer.mRigidbody.MovePosition(new Vector2(transform.position.x, transform.position.y) + mPlayer.mDodgeDirection * mSpeed);
            }
        }
    }


    public void HurtPlayer()
    {
        mPlayer.mAnimator.SetTrigger(mHurtParam);
    }

    public void KillPlayer()
    {
        mPlayer.mAnimator.SetBool(mDeadParam, true);
    }

}
