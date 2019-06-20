using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using System;

using Cgl.Thesis;

public class ThirdPersonController : ThirdPersonAnimator
{

    public enum ControllerHandler
    {
        HandledByRootMotion,//  handled from the original animation kevalue
        HandledByLocomotionMotorMotion, // full controll ovrer user input axis
        HandledByCustomRootMotion, // position updates either by small routines or partial input from user
        HandledByAgentRootMotion,//  position updates completeely handly by script
        PauseMototoMotion  // player waiting for some game actions to happens
    }

    public ControllerHandler m_currentPlayerHandler = ControllerHandler.HandledByLocomotionMotorMotion;
    private bool m_isClimbingWall = false;
    private bool m_isClimbingLadder = false;

    [Header("--- Wall Climb Set UP---")]
    public float m_ClimbRayCastLengh = .5f;
    public float m_ClimbHitMinDistance = .3f;
    public float m_ClimbHitMaxAngle = 40.0f;

    public float m_climbHeight;
    public float m_climbForward;
    public AnimationCurve m_ClimbYCurve;
    public AnimationCurve m_ClimbZCurve;

    private Vector3 m_climbStartPos;
    public LayerMask m_ClimbWallyerLayer;

    [Header("--- Ladder Climb Set UP---")]
    public InteractableLadder m_currentLadder;
    public float m_ladderClimbSpead = 1.0f;

    [Header("--- FullBodyBipedIK IK ---")]
    public FullBodyBipedIK m_fBBIK;

    [Header("--- Grounder IK ---")]
    public GrounderFBBIK m_grounderFBBIK;
    public InteractableObjectFBBIK m_currentInteractingObject;

    [Header("--- Box push ---")]
    public float m_boxPushSpeed = .5f;

    protected bool m_isInteracting; // doing some interaction 
    protected float m_interactionMoveSpeed; // custom speed for interaction, like pushing box

    [Header("--- Locomotion additional settings ---")]
    public InteractableObjectFBBIK m_closetInteractableObjectInRange;
    public float m_agentRootMotionSpeed = .5f; // motor speed when handled by script

    private ControllerHandler m_lastPlayerHandler; // copying last handler before taking controll to by scripts


    #region unity callbacks
    public override void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    public override void Start()
    {

    }

    // Update is called once per frame
    public override void Update()
    {
        if (m_currentPlayerHandler == ControllerHandler.HandledByCustomRootMotion)
        {
            if (m_currentInteractingObject != null)
            {
                if (m_currentInteractingObject.InteractionType == InteractionTypes.Ladder)
                {
                    UpdateLadderClimbDirAnimation(Mathf.Round(m_inputMoveAxis.y));
                }
            }   
        }
    }

    public void UpdateController()
    {
        if (m_currentPlayerHandler == ControllerHandler.HandledByLocomotionMotorMotion)
        {
            if (m_currentInteractingObject != null)
            {
                if (m_currentInteractingObject.InteractionType == InteractionTypes.DraggableBox)
                {
                    UpdateAnimator(Mathf.Clamp(m_speed, 0, 0.4f));
                }
                else
                {
                    UpdateAnimator(Mathf.Clamp(m_speed, 0, 0.5f));
                }
            }
            else
            {
                UpdateAnimator(Mathf.Clamp(m_speed, 0, 0.5f));
            }

            UpdateMotor();
            m_grounderFBBIK.weight = m_isJumping ? 0.0f : 1.0f;
        }
        else if (m_currentPlayerHandler == ControllerHandler.HandledByCustomRootMotion)
        {
            UpdateAnimator(0.0f);
            m_grounderFBBIK.weight = 0.0f;
        }
        else if (m_currentPlayerHandler == ControllerHandler.PauseMototoMotion)
        {
            UpdateAnimator(0.0f);
            m_grounderFBBIK.weight = 1.0f;
        }
        else if (m_currentPlayerHandler == ControllerHandler.HandledByAgentRootMotion)
        {
            UpdateAnimator(0.0f);
            m_grounderFBBIK.weight = 1.0f;
        }
    }

    public void FixedUpdate() // root movement controll
    {
        if (m_currentPlayerHandler == ControllerHandler.HandledByLocomotionMotorMotion)
        {
            float walkspeed = m_walkSpeed;
            if (m_currentInteractingObject != null)
            {
                if (m_currentInteractingObject.InteractionType == InteractionTypes.DraggableBox)
                {
                    walkspeed = m_boxPushSpeed;
                }
            }

            if (m_isGrounded) //  if (m_isGrounded && CanMoveForward())
            {
                if (m_speed <= 0.5f)
                    UpdateRootMotion(walkspeed);
                else if (m_speed > 0.5 && m_speed <= 1f) { UpdateRootMotion(walkspeed); }
                    //UpdateRootMotion(m_runSpeed);
            }
        }
        else if (m_currentPlayerHandler == ControllerHandler.HandledByCustomRootMotion)
        {
            UpdateRootMotion(m_interactionMoveSpeed);
        }
        else if (m_currentPlayerHandler == ControllerHandler.PauseMototoMotion)
        {
            //UpdateRootMotion(m_interactionMoveSpeed);
        }
        else if (m_currentPlayerHandler == ControllerHandler.HandledByAgentRootMotion)
        {
            UpdateRootMotion(m_walkSpeed);
        }
    }

    public override void OnAnimatorMove()
    {
        if (m_currentPlayerHandler == ControllerHandler.HandledByRootMotion) // reading valued from input
        {
            Vector3 newPosition = m_animator.deltaPosition;
            transform.position += newPosition;
           
        }
        else if (m_currentPlayerHandler == ControllerHandler.HandledByLocomotionMotorMotion)
        {
            base.OnAnimatorMove();
        }
        else if (m_currentPlayerHandler == ControllerHandler.HandledByCustomRootMotion)
        {
            if (m_currentInteractingObject != null)
            {
                if (m_currentInteractingObject.InteractionType == InteractionTypes.Ladder)
                {
                    OnAnimatorMoveOnClimbLadder();
                }     
            }

            if (m_isClimbingWall) OnAnimatorMoveOnClimbWall();
        }
        else if (m_currentPlayerHandler == ControllerHandler.HandledByAgentRootMotion)
        {
          
        }
    }

    private bool CanMoveForward()
    {
        if (m_closetInteractableObjectInRange == null || m_isInteracting) return true;

        var hit = new RaycastHit();
        var rayDirection = m_transform.forward;
        Vector3 rayOrigin = (m_transform.position + rayDirection * (m_capsuleCollider.radius * 0.5f)) + m_transform.up * .1f;
        Ray ray = new Ray(rayOrigin, rayDirection);

        Vector3 hitDirection = new Vector3();
        bool isinRange = false;
        if (Physics.Raycast(ray, out hit, m_ClimbRayCastLengh, m_closetInteractableObjectInRange.gameObject.layer) && !hit.collider.isTrigger)
        {
            if (hit.distance >= m_closetInteractableObjectInRange.PlayerMinAproachDistance)
            {
                // calucalate hit normal and cache it
                hitDirection = hit.normal;

                float angle = Vector3.Angle(rayDirection, -hitDirection);
                isinRange = true;
            }
        }

        return isinRange;
    }

    private void OnAnimatorMoveOnClimbLadder()
    {
        float ladderInput = (Mathf.Round(m_inputMoveAxis.y));
        m_customRootPosition = m_currentLadder.GetPlayerPositionOnLader(ladderInput, m_ladderClimbSpead);
        m_transform.rotation = m_currentLadder.GetPlayerRotationOnLader();
    }

    private void OnAnimatorMoveOnClimbWall()
    {
        float currentAnimationTime = m_animator.GetFloat("ClimbTime");
        float currentHeighValue = m_ClimbYCurve.Evaluate(currentAnimationTime);
        float currentForwaredValue = m_ClimbZCurve.Evaluate(currentAnimationTime);

        m_rigidbody.useGravity = false;
        Vector3 newPosition = m_animator.deltaPosition;

        Vector3 localspaceUp = m_transform.up * (currentHeighValue * m_climbHeight);
        Vector3 localspaceForward = m_transform.forward * currentForwaredValue * m_climbForward;
        Vector3 localPos = localspaceUp + localspaceForward;

        //localspace.y = (currentHeighValue * m_climbHeight);
        //localspace.z = (currentForwaredValue * m_climbForward);
        m_customRootPosition = m_climbStartPos + localPos;

        if (Mathf.Approximately(currentAnimationTime, 1.0f))
        {
            m_useCustomeRooMotion = false;
            m_keepDirection = false;
            m_isClimbingWall = false;
            PlayWallClimbAnimation(false);
            m_currentPlayerHandler = ControllerHandler.HandledByLocomotionMotorMotion;
            m_rigidbody.useGravity = true;     
        }   
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    public void ReadInput()
    {
        m_inputMoveAxis = m_thirdPersonInput.InputMoveAxis;
    }

    // we take complete handling of the player
    public void MovePlayerTo(Vector3 position, bool faceOnDir, Action onComplete)
    {
        m_lastPlayerHandler = m_currentPlayerHandler;
        m_currentPlayerHandler = ControllerHandler.HandledByAgentRootMotion;
        // m_customRootPosition = m_transform.position;

        StartCoroutine(IEMovePlayerTo(position, faceOnDir, onComplete));
    }

    IEnumerator IEMovePlayerTo(Vector3 position, bool faceOnDir, Action onComplete)
    {
        Debug.Log("staring moving plazer bz script");
        Vector3 fromPos = m_transform.position;
        Vector3 toPos = position;

        float step = (m_agentRootMotionSpeed / (fromPos - toPos).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 1.0f)
        {
            t += step; // Goes from 0 to 1, incrementing by step each time
            m_transform.position = Vector3.Lerp(fromPos, toPos, t); // Move objectToMove closer to b
            yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
        }

        m_transform.position = toPos;
        m_currentPlayerHandler = m_lastPlayerHandler; // setting back to last handler
        onComplete();

        Debug.Log("ending  moving plazer bz script");
        yield return null; 
    }

    public void Jump()  // TODO : climb walll if the stands in front of the wall 
    {
        if (IsInfrontOfClimbWall())
        {
            ClimbWall();
        }
        else
        {
            JumpNormal();
        }
    }

    public void JumpNormal() 
    {
        // conditions to do this action
        bool jumpConditions = m_isGrounded && !m_isJumping;
        // return if jumpCondigions is false
        if (!jumpConditions) return;
        // trigger jump behaviour
        m_jumpCounter = m_jumpTimer;
        m_isJumping = true;

        // trigger jump animations            
        if (m_rigidbody.velocity.magnitude < 1)
            m_animator.CrossFadeInFixedTime("Jump", 0.1f);
        else
            m_animator.CrossFadeInFixedTime("JumpMove", 0.2f);
    }

    private bool IsInfrontOfClimbWall()
    {
        var hit = new RaycastHit();
        var rayDirection = m_transform.forward;
        Vector3 rayOrigin = (m_transform.position + rayDirection * (m_capsuleCollider.radius * 0.5f)) + m_transform.up * .1f;
        Ray ray = new Ray(rayOrigin, rayDirection);

        Vector3 hitDirection = new Vector3();
        bool isinRangeClimbRange = false;
        if (Physics.Raycast(ray, out hit, m_ClimbRayCastLengh, m_ClimbWallyerLayer) && !hit.collider.isTrigger)
        {
            if (hit.distance <= m_ClimbHitMinDistance)
            {
                // calucalate hit normal and cache it
                hitDirection = hit.normal;

                float angle = Vector3.Angle(rayDirection, -hitDirection);
                isinRangeClimbRange = true;
            }
        }

        return isinRangeClimbRange;
    }

    public void ClimbWall() // we are doing the same double calculation, later we will update the code
    {
        if (m_isClimbingWall) return;
        // check for climbing is possible 
        // if then desable the component
        var hit = new RaycastHit();
        var rayDirection = m_transform.forward;
        Vector3 rayOrigin = (m_transform.position + rayDirection * (m_capsuleCollider.radius * 0.5f)) + m_transform.up * .1f;
        Ray ray = new Ray(rayOrigin, rayDirection);

        Vector3 hitDirection = new Vector3();
        bool isinRangeClimbRange = false;
        if (Physics.Raycast(ray, out hit, m_ClimbRayCastLengh, m_ClimbWallyerLayer) && !hit.collider.isTrigger)
        {
            if (hit.distance <= m_ClimbHitMinDistance)
            {
                // calucalate hit normal and cache it
                hitDirection = hit.normal;

                float angle = Vector3.Angle(rayDirection, -hitDirection);
                isinRangeClimbRange = true;
            }
        }
        bool canClimb = m_isGrounded && !m_isJumping && isinRangeClimbRange;
        if (canClimb)
        {
            m_currentPlayerHandler = ControllerHandler.HandledByCustomRootMotion;
            m_useCustomeRooMotion = true;
            m_keepDirection = true;
            m_moveDirection = -hitDirection;
            PlayWallClimbAnimation(true);
            m_isClimbingWall = true;
            m_climbStartPos = m_transform.position;
        }
    }


    public void OnInteractableObjectInRangeEvent(InteractableObjectFBBIK interactableObject)
    {
        m_closetInteractableObjectInRange = interactableObject;
    }

    public void OnInteractableObjectOutRangeEvent(InteractableObjectFBBIK interactableObject)
    {
        m_closetInteractableObjectInRange = null;
    }

    public void OnInteractionStartEvent(InteractableObjectFBBIK interactableObject)
    {
        m_isInteracting = true;
        m_currentInteractingObject = interactableObject;
        switch (interactableObject.InteractionType)
        {
            case InteractionTypes.Door:
                m_currentPlayerHandler = ControllerHandler.PauseMototoMotion;
               // m_loackMotorMotion = true;
                break;

            case InteractionTypes.DraggableBox:
                m_currentPlayerHandler = ControllerHandler.HandledByLocomotionMotorMotion;
                ConstrainMotorDirection(interactableObject.CurrentInteractionTrigger.InteractionForward, interactableObject.m_playerRootMotionSpeed);
                break;

            case InteractionTypes.Ladder:
                m_currentLadder = (InteractableLadder)interactableObject;
                m_currentPlayerHandler = ControllerHandler.HandledByCustomRootMotion;
                m_rigidbody.useGravity = false;
                PlayLadderClimbAnimation(true);
                m_useCustomeRooMotion = true;
                m_customRootPosition = m_transform.position; // resetting it to the current player postion
                break;
        }
    }

    public void OnInteractionStopEvent(InteractableObjectFBBIK interactableObject)
    {
       
        switch (interactableObject.InteractionType)
        {
            case InteractionTypes.Door:
                m_currentPlayerHandler = ControllerHandler.HandledByLocomotionMotorMotion;
                break;

            case InteractionTypes.DraggableBox:
                m_currentPlayerHandler = ControllerHandler.HandledByLocomotionMotorMotion;
                //m_isInRootMotionInteraction = false;
                UnConstrainMotorDirection();
                break;

            case InteractionTypes.Ladder:
                m_currentLadder = null;
                PlayLadderClimbAnimation(false);
                m_currentPlayerHandler = ControllerHandler.HandledByLocomotionMotorMotion;
                m_rigidbody.useGravity = true;
                m_useCustomeRooMotion = false;
                //m_isInRootMotionInteraction = false;
                //m_thirdPersonController.UnConstrainMotorDirection();
                break;
        }
        m_currentInteractingObject = null;
        m_isInteracting = false;
    }

    public void ConstrainMotorDirection(Vector3 direction, float speed)
    {
        m_keepDirection = true;
        m_moveDirection = direction;
        m_interactionMoveSpeed = speed;
      
        m_animator.SetBool("Push",true);
    }

    public void UnConstrainMotorDirection()
    {
       // m_isInteracting = false;
        m_keepDirection = false;
        m_animator.SetBool("Push", false);
    }


    public void SetRightArmEffector( float psotionWeight, Transform rightArmEffector = null)
    {
        m_fBBIK.solver.rightHandEffector.target = rightArmEffector;
        m_fBBIK.solver.rightHandEffector.positionWeight = psotionWeight;
    }

    public void SetLeftArmEffector(float psotionWeight, Transform rightArmEffector = null)
    {
        m_fBBIK.solver.leftHandEffector.target = rightArmEffector;
        m_fBBIK.solver.leftHandEffector.positionWeight = psotionWeight;
    }

    #endregion
}
