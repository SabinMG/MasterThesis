using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(ThirdPersonInput))]

public class ThirdPersonMotor : MonoBehaviour
{
    #region public variables 
    public float m_moveRotationSpeed = 10.0f;
    public LayerMask m_groundLayer;
    public float m_colliderHeight = 1.15f;
    protected float m_speed = 1.5f;
    public float m_walkSpeed = 1.5f;
    public float m_runSpeed = 2.5f;

    #endregion

    #region private variables 
    protected Transform m_transform;
    protected Rigidbody m_rigidbody;
    protected CapsuleCollider m_capsuleCollider;
    private Quaternion moveRotation;
    #endregion

    #region protected variables 
    protected Vector2 m_inputMoveAxis;

    public Vector2 InputMoveAxis
    {
        set
        {
            m_inputMoveAxis = value;
        }
    }

    protected ThirdPersonInput m_thirdPersonInput;
    protected float m_groundMinDistance = 0.2f;
    protected bool m_isGrounded = false;
    protected bool m_keepDirection;
    protected Vector3 m_moveDirection;
    protected Animator m_animator;

    protected bool m_useCustomeRooMotion; // custome handling of player postion
    protected Vector3 m_customRootPosition; // current defined position of  controller

    #endregion

    [Header("--- Grounded Setup ---")]

    [Tooltip("ADJUST IN PLAY MODE - Offset height limit for sters - GREY Raycast in front of the legs")]
    public float m_stepOffsetEnd = 0.45f;
    [Tooltip("ADJUST IN PLAY MODE - Offset height origin for sters, make sure to keep slight above the floor - GREY Raycast in front of the legs")]
    public float m_stepOffsetStart = 0.05f;
    [Tooltip("Higher value will result jittering on ramps, lower values will have difficulty on steps")]
    public float m_stepSmooth = 4f;
    [Tooltip("Max angle to walk")]
    [SerializeField]
    protected float m_slopeLimit = 45f;
    [Tooltip("Apply extra gravity when the character is not grounded")]
    [SerializeField]
    protected float m_extraGravity = -10f;

    protected bool m_isJumping;

    [Header("--- jumping Setup ---")]
    [Tooltip("Check to control the character while jumping")]
    public bool m_jumpAirControl = true;
    [Tooltip("How much time the character will be jumping")]
    public float m_jumpTimer = 0.3f;
    [HideInInspector]
    public float m_jumpCounter;
    [Tooltip("Add Extra jump speed, based on your speed input the character will move forward")]
    public float m_jumpForward = 3f;
    [Tooltip("Add Extra jump height, if you want to jump only with Root Motion leave the value with 0.")]
    public float m_jumpHeight = 4f;



    #region unity callbacks
    public virtual void Awake()
    {
        m_transform = GetComponent<Transform>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_capsuleCollider = GetComponent<CapsuleCollider>();
        m_thirdPersonInput = GetComponent<ThirdPersonInput>();
    }

    // Use this for initialization
    public virtual void Start()
    {

    }

    // Update is called once per frame
    public virtual void Update()
    {

    }
    #endregion

    #region private methods
    public virtual void UpdateMotor()
    {
        CheckGround();
        ControlJumpBehaviour();
        UpdateLocomotion();
    }

    private void CheckGround()
    {
        float groundDistance = GetGroundDistance();
        if (groundDistance <= m_groundMinDistance) m_isGrounded = true;
        else m_isGrounded = false;

        bool onStep = StepOffset();
    }

    protected void ControlJumpBehaviour()
    {
        if (!m_isJumping) return;

        m_jumpCounter -= Time.deltaTime;
        if (m_jumpCounter <= 0)
        {
            m_jumpCounter = 0;
            m_isJumping = false;
        }
        // apply extra force to the jump height   
        var vel = m_rigidbody.velocity;
        vel.y = m_jumpHeight;
        m_rigidbody.velocity = vel;
    }

    private float GetGroundDistance()
    {
        if (m_capsuleCollider != null)
        {
            // radius of the SphereCast
            float radius = m_capsuleCollider.radius * 0.9f;
            var dist = 10f;
            // position of the SphereCast origin starting at the base of the capsule
            Vector3 pos = m_transform.position + Vector3.up * (m_capsuleCollider.radius);
            // ray for RayCast
            Ray ray1 = new Ray(m_transform.position + new Vector3(0, m_colliderHeight / 2, 0), Vector3.down);
            // ray for SphereCast
            Ray ray2 = new Ray(pos, -Vector3.up);
            RaycastHit groundHit;
            // raycast for check the ground distance
            if (Physics.Raycast(ray1, out groundHit, m_colliderHeight / 2 + 2f, m_groundLayer))
                dist = m_transform.position.y - groundHit.point.y;
            // sphere cast around the base of the capsule to check the ground distance
            if (Physics.SphereCast(ray2, radius, out groundHit, m_capsuleCollider.radius + 2f, m_groundLayer))
            {
                // check if sphereCast distance is small than the ray cast distance
                if (dist > (groundHit.distance - m_capsuleCollider.radius * 0.1f))
                    dist = (groundHit.distance - m_capsuleCollider.radius * 0.1f);
            }

            float hitdistance = (float)System.Math.Round(dist, 2);
            return hitdistance;
        }
        return 0.0f;
    }

    bool StepOffset()
    {
        if (m_inputMoveAxis.sqrMagnitude < 0.1 || !m_isGrounded) return false;

        var _hit = new RaycastHit();
        var _movementDirection =  false && m_inputMoveAxis.magnitude > 0 ? (transform.right * m_inputMoveAxis.x + transform.forward * m_inputMoveAxis.y).normalized : transform.forward;
        Ray rayStep = new Ray((transform.position + new Vector3(0, m_stepOffsetEnd, 0) + _movementDirection * ((m_capsuleCollider).radius + 0.05f)), Vector3.down);

        Debug.DrawRay(rayStep.origin, rayStep.direction);
        if (Physics.Raycast(rayStep, out _hit, m_stepOffsetEnd - m_stepOffsetStart, m_groundLayer) && !_hit.collider.isTrigger)
        {
            if (_hit.point.y >= (transform.position.y) && _hit.point.y <= (transform.position.y + m_stepOffsetEnd))
            {
                var _speed = false ? Mathf.Clamp(m_inputMoveAxis.magnitude, 0, 1) : m_speed;
                var velocityDirection = false ? (_hit.point - transform.position) : (_hit.point - transform.position).normalized;
                float velocity = 0.0f;
                m_rigidbody.velocity = velocityDirection * m_stepSmooth * (_speed * (velocity > 1 ? velocity : 1));
                return true;
            }
        }
        return false;
    }

    protected void UpdateRootMotion(float velocity)
    {
        if (Time.deltaTime == 0) return;

        if (m_useCustomeRooMotion)
        {
            m_transform.position = m_customRootPosition;
        }
        else
        {
            float direction = 0.0f;
            var velY = transform.forward * velocity * m_speed;
            velY.y = m_rigidbody.velocity.y;
            var velX = transform.right * velocity * direction;
            velX.x = m_rigidbody.velocity.x;

            if (false)
            {
                //Vector3 v = (transform.TransformDirection(new Vector3(input.x, 0, input.y)) * (velocity > 0 ? velocity : 1f));
                //v.y = _rigidbody.velocity.y;
                //_rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, v, 20f * Time.deltaTime);
            }
            else
            {
                m_rigidbody.velocity = velY;
                m_rigidbody.AddForce(transform.forward * (velocity * m_speed) * Time.deltaTime, ForceMode.VelocityChange);
            }
        }
    }

    private void UpdateLocomotion()
    {
        UpdateCharactorLookDirection();
        FreeMovement(); 
    }

    private void UpdateCharactorLookDirection()
    {
        Vector3 inputDirection = new Vector3(m_inputMoveAxis.x, 0, m_inputMoveAxis.y);
        float oppositDirection = Vector3.Dot(inputDirection, m_moveDirection);
       
        if (oppositDirection < 0.0f && !m_useCustomeRooMotion) m_keepDirection = false;
        m_moveDirection = m_keepDirection ? m_moveDirection : inputDirection;
    }

    private void FreeMovement()
    {
        // set speed to both vertical and horizontal inputs
        m_speed = Mathf.Abs(m_inputMoveAxis.x) + Mathf.Abs(m_inputMoveAxis.y);
        m_speed = Mathf.Clamp(m_speed, 0, 1f);
        // add 0.5f on sprint to change the animation on animator
        //if (isSprinting) m_speed += 0.5f;

        if (m_keepDirection|| (m_inputMoveAxis != Vector2.zero && m_moveDirection.magnitude > 0.1f))
        {
            Vector3 lookDirection = m_moveDirection.normalized;
            moveRotation = Quaternion.LookRotation(lookDirection, m_transform.up);
            var diferenceRotation = moveRotation.eulerAngles.y - m_transform.eulerAngles.y;
            var eulerY = m_transform.eulerAngles.y;

            // apply free directional rotation while not turning180 animations
            if (m_isGrounded || (!m_isGrounded && m_jumpAirControl))
            {
                if (diferenceRotation < 0 || diferenceRotation > 0) eulerY = moveRotation.eulerAngles.y;
                var euler = new Vector3(m_transform.eulerAngles.x, eulerY, transform.eulerAngles.z);
                m_transform.rotation = Quaternion.Lerp(m_transform.rotation, Quaternion.Euler(euler), m_moveRotationSpeed * Time.deltaTime);
            }
        }
        #endregion
    }
}
