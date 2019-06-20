using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ThirdPersonAnimator : ThirdPersonMotor
{
    #region private variables 

    #endregion

    public delegate void PlayerAnimationDelegate();
    public event PlayerAnimationDelegate OnPlayerStandUpAnimationFinished;

    #region unity callbacks
    public override void Awake()
    {
        base.Awake();
        m_animator = GetComponent<Animator>();
    }

    protected void PlayerStandUpAnimationFinished()
    {
        if (OnPlayerStandUpAnimationFinished != null) OnPlayerStandUpAnimationFinished();
    }

    // Use this for initialization
    public override void Start ()
    {
		
	}

    // Update is called once per frame
    public override void Update()
    {
        //if (m_isGrounded)
        //{
        //    //  if (m_speed <= 0.5f)
        //    UpdateRootMotion(m_walkSpeed);
        //}
    }

 
    public virtual void OnAnimatorMove()
    {
      
    }
    #endregion

    public virtual void UpdateAnimator( float speed)
    {
        m_animator.SetBool("IsGrounded", m_isGrounded);
        m_animator.SetFloat("InputVertical", speed, 0.1f, Time.deltaTime);
    }

    protected void PlayWallClimbAnimation (bool value)
    {
        m_animator.SetBool("Climb", value);
    }

    protected void PlayLadderClimbAnimation(bool value)
    {
        m_animator.SetBool("LadderClimb", value);
    }

    protected void UpdateLadderClimbDirAnimation(float dir)
    {
        m_animator.SetFloat("LadderClimbDir", dir);
    }

    protected void SetForIntroAnimation(bool enabled)
    {
        m_animator.SetBool("IntroAnimation", enabled);
    }
}
