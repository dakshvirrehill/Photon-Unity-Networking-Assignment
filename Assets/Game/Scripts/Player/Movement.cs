using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Player))]
public class Movement : MonoBehaviour
{
    [HideInInspector] public bool mActive = true;
    float mHorizontal = 0.0f;
    Player mPlayer;
    [SerializeField] float mSpeed = 10;
    #region Animator Parameters
    [Header("AnimatorParameters")]
    [SerializeField] string mHorizontalParam;
    [SerializeField] string mAttackParam;
    [SerializeField] string mHurtParam;
    [SerializeField] string mDeadParam;
    #endregion
    void Start()
    {
        mPlayer = GetComponent<Player>();
    }
    void Update()
    {
        mPlayer.mRenderer.flipX = mPlayer.mRigidbody.velocity.x < 0;
        if (!mActive)
        {
            return;
        }
        mHorizontal = Input.GetAxis("Horizontal");
        mPlayer.mAnimator.SetFloat(mHorizontalParam, mHorizontal);
        if(Input.GetButtonDown("Fire1"))
        {
            mPlayer.mAnimator.SetTrigger(mAttackParam);
        }
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
