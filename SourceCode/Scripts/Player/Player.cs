using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cgl.Thesis.Data;
using Cgl.Thesis.Utilities;

namespace Cgl.Thesis
{
    public class Player : ThirdPersonController
    {
        public static Player _instance;

        public static Player Instance
        {
            get { return _instance; }
        }

        private Vector3 m_playerLastPosition;
        public delegate void PlayerPositionUpdateDelegate (Vector3 position);
        public event PlayerPositionUpdateDelegate OnPlayerPositionUpdate;

       
        [Header("--- player Input ---")]
        //public ThirdPersonInput m_thirdPersonInput;
        public BoolSO m_playerInputEnabled;
        public ThirdPersonController m_thirdPersonController;

        private Transform m_transform;
        // interaction motor controll
        private InteractionController m_interactionController;
        //private Vector3 constraintMotorDirection;
        //private bool m_isInRootMotionInteraction;
       // private bool m_isInInplaceInteraction;


        public override void Awake()
        {
            _instance = this;
            base.Awake();
            m_interactionController = InteractionController.Instance;
            m_transform = transform;
        }

        // Use this for initialization
        void Start()
        {
            m_playerLastPosition = m_transform.position;
        }

        public override void Update()
        {
            base.Update();
          
            if (!MathUtilities.V3Equal(m_playerLastPosition, m_transform.position))
            {
                Vector3 deltaPosition = m_playerLastPosition - m_transform.position;
                if (OnPlayerPositionUpdate != null) OnPlayerPositionUpdate(deltaPosition);
                m_playerLastPosition = m_transform.position;
            }

            ReadInput();
            UpdateController();
        }

        public void InitialilzeForIntro(bool initForIntro)
        {
            if (initForIntro)
            {
                m_currentPlayerHandler = ControllerHandler.HandledByRootMotion;
                SetForIntroAnimation(true);
                AnimationStateBehaviour animatorStateBehaviour = m_animator.GetBehaviour<AnimationStateBehaviour>();
                animatorStateBehaviour.OnAnimationStateExit += OnPlayerAnimationStateChanged;

                m_rigidbody.isKinematic = true;
                m_grounderFBBIK.weight = 0.0f;

                //GetComponent<TransformDataApplier>().ApplyTransfrom();
            }
            else
            {
                m_rigidbody.isKinematic = false;
                m_currentPlayerHandler = ControllerHandler.HandledByLocomotionMotorMotion;
                SetForIntroAnimation(false);
               // m_grounderFBBIK.weight = 1.0f;
            }
        }

        private void OnPlayerAnimationStateChanged(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.IsName("Stand Up")) // hardcoding wrong
            {
                PlayerStandUpAnimationFinished();
            }
        }

        private void OnEnable()
        {
            m_thirdPersonInput.OnJumpKeyEvent += Jump;
          
            m_thirdPersonInput.OnInteractionKeyDownEvent += StartInteraction;
            m_thirdPersonInput.OnInteractionKeyUpEvent += EndInteraction;

            m_interactionController.OnInteractionStartEvent += OnInteractionStartEventCallback;
            m_interactionController.OnInteractionStopEvent += OnInteractionStopEventCallback;

            m_interactionController.OnInteractableObjectInRangeEvent += OnInteractableObjectInRangeEventCallback;
            m_interactionController.OnInteractableObjectOutRangeEvent += OnInteractableObjectOutRangeEventCallback;
        }

        private void OnDisable()
        {
            m_thirdPersonInput.OnJumpKeyEvent -= Jump;
   
            m_thirdPersonInput.OnInteractionKeyDownEvent -= StartInteraction;
            m_thirdPersonInput.OnInteractionKeyUpEvent -= EndInteraction;

            m_interactionController.OnInteractionStartEvent -= OnInteractionStartEventCallback;
            m_interactionController.OnInteractionStopEvent -= OnInteractionStopEventCallback;

            m_interactionController.OnInteractableObjectInRangeEvent -= OnInteractableObjectInRangeEventCallback;
            m_interactionController.OnInteractableObjectOutRangeEvent -= OnInteractableObjectOutRangeEventCallback;
        }

        public void ReadInput()
        {
            if (!m_playerInputEnabled.Value) return;
                m_thirdPersonController.InputMoveAxis = m_thirdPersonInput.InputMoveAxis;
        }

        public void StartInteraction()
        {
            if (!m_playerInputEnabled.Value) return;
            if (!m_interactionController.IsInteracting) m_interactionController.StartIntercation();
        }

        public void EndInteraction()
        {
            if (!m_playerInputEnabled.Value) return;
            if (m_interactionController.IsInteracting) m_interactionController.EndInteraction();
        }

        public void MovePlayerTo(Vector3 position, bool faceOnDir, Action onComplete)
        {
            if (!m_playerInputEnabled.Value) return;
            m_thirdPersonController.MovePlayerTo(position, faceOnDir, onComplete);
        }

        public void MovePlayerTo(Vector3[] positions, bool faceOnDir, Action onComplete)
        {

        }

        private void Jump() // includes wall climb
        {
            if (!m_playerInputEnabled.Value) return;
            m_thirdPersonController.Jump();
        }

        private void OnInteractionStartEventCallback(InteractableObjectFBBIK interactableObject)
        {
            m_thirdPersonController.OnInteractionStartEvent(interactableObject);
        }

        private void OnInteractionStopEventCallback(InteractableObjectFBBIK interactableObject)
        {
            m_thirdPersonController.OnInteractionStopEvent(interactableObject); 
        }

        private void OnInteractableObjectInRangeEventCallback(InteractableObjectFBBIK interactableObject)
        {
            m_thirdPersonController.OnInteractableObjectInRangeEvent(interactableObject);
        }

        private void OnInteractableObjectOutRangeEventCallback(InteractableObjectFBBIK interactableObject)
        {
             m_thirdPersonController.OnInteractableObjectOutRangeEvent(interactableObject);
        }
    }
}
