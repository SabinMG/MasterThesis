using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RootMotion.FinalIK;

namespace Cgl.Thesis
{
    public class InteractableBox : InteractableObjectFBBIK
    {
        public float maxAngle = 50.0f;
        private BoxCollider m_boxCollider;
        public GameObject m_BoxColliderGO;

        public UnityEvent m_boxMove;
        public UnityEvent OnReachBoarder;
        public UnityEvent OnReachBoarderZBack;

        public float m_xMaxMoveRight= 1.0f;
        public float m_xMaxMoveLeft = 1.0f;
        public float m_zMaxMoveBack = 1.0f;
        private Vector3 m_initialTransforPosition;

        private bool m_interactionInPause; // where the hand reached the box

        //public Animator m_doorAnimator;
        protected override void Awake()
        {
            base.Awake();
            m_boxCollider = m_BoxColliderGO.GetComponent<BoxCollider>();
            m_initialTransforPosition = transform.position;
        }

        void OnDrawGizmosSelected()
        {
            // Draw a yellow cube at the transform position
            Gizmos.color = Color.yellow;

            Vector3 xRightPos = transform.position;
            xRightPos += transform.right * m_xMaxMoveRight;
            Gizmos.DrawLine(transform.position, xRightPos);

            Vector3 xLeftPos = transform.position;
            xLeftPos -= transform.right * m_xMaxMoveLeft;
            Gizmos.DrawLine(transform.position, xLeftPos);


            Vector3 zBacktPos = transform.position;
            zBacktPos -= -transform.forward * m_zMaxMoveBack;
            Gizmos.DrawLine(transform.position, zBacktPos);

        }

        // Use this for initialization
        void Start()
        {


        }

        public override bool CheckForInteraction()
        {
            Vector3 forward = m_currentInteractionTrigger.InteractionForward;
            Vector3 toOther = m_player.transform.position - (m_currentInteractionTrigger.transform.position - m_currentInteractionTrigger.InteractionForward* m_currentInteractionTrigger.InteractionTriggerCollider.bounds.extents.x);
            if (Vector3.Dot(forward, toOther) > 0)
            {
                float angle = Vector3.Angle(forward, m_player.transform.forward);
                if (angle < maxAngle)
                {
                    // rotating m_InteractionTargetPivot

                    Vector3 pivotPos = m_transform.position - m_currentInteractionTrigger.InteractionForward * m_boxCollider.bounds.extents.x;
                    m_InteractionTargetPivot.position = new Vector3(pivotPos.x, m_InteractionTargetPivot.position.y, pivotPos.z);

                    m_InteractionTargetPivot.forward = m_currentInteractionTrigger.InteractionForward;

                    Vector3 pivotSideDirection = Vector3.Cross(m_currentInteractionTrigger.InteractionForward, Vector3.up).normalized;
                    Debug.DrawRay(m_InteractionTargetPivot.position, pivotSideDirection);

                    Vector3 direction = m_player.transform.position - m_InteractionTargetPivot.position;

                    Vector3 projectedOnCubeSide =  Vector3.Project(direction, pivotSideDirection);
                    m_InteractionTargetPivot.position = m_InteractionTargetPivot.position + projectedOnCubeSide.normalized * projectedOnCubeSide.magnitude;

                    return true;
                }
            }
            return false;
        }

        public override void StartInteraction(InteractionSystem interactionSystem)
        {
            base.StartInteraction(interactionSystem);
            Debug.Log("start box interraction");
            m_InteractionSystem = interactionSystem;
            m_InteractionSystem.StartInteraction(FullBodyBipedEffector.LeftHand, m_finalIKInteractionObject, true);
            m_InteractionSystem.StartInteraction(FullBodyBipedEffector.RightHand, m_finalIKInteractionObject, true);

            if (m_onStartInteractionEvent != null) m_onStartInteractionEvent.Invoke();

            MovePlayerToRightPos();
        }

        private void MovePlayerToRightPos()
        {
           Vector3 playerPos = m_InteractionTargetPivot.position;
           playerPos.y = m_player.transform.position.y;
           Vector3 newPlayerPos = playerPos - m_player.transform.forward *m_playerMinAproachDistance;
           m_player.MovePlayerTo(newPlayerPos, false, ()=>{ });
        }

        public override void EndInteraction()
        {
            base.EndInteraction();
            m_InteractionSystem.StopInteraction(FullBodyBipedEffector.LeftHand);
            m_InteractionSystem.StopInteraction(FullBodyBipedEffector.RightHand);
            m_interactionInPause = false;

            if (m_onEndInteractionEvent != null) m_onEndInteractionEvent.Invoke();
        }

        public void StartPlayDoorAnimation()
        {
 
        }

        public void InterctionEvent(FullBodyBipedEffector effectorType, InteractionObject interactionObject, InteractionObject.InteractionEvent interactionEvent)
        {
            Debug.Log(interactionEvent.time);
        }

        public void SetInteractionInPause()
        {
            m_interactionInPause = true;
        }

        public override void OnPlayerPositionUpdateCallback(Vector3 deltaposition)
        {
            if (m_interactionInPause && m_currentInteractionTrigger != null)
            {
                m_transform.position = m_transform.position + m_currentInteractionTrigger.InteractionForward * deltaposition.magnitude;
                if (m_boxMove != null) m_boxMove.Invoke();
            }

            if (m_currentInteractionTrigger.InteractionSide == InteractionSideEnum.Left)
            {
                if (m_transform.position.x >= ((m_initialTransforPosition + transform.right * m_xMaxMoveRight).x - m_boxCollider.bounds.extents.x))
                {
                    EndInteraction();
                   // m_enableInteraction = false;
                    OnInteractableObjectOutRange();
                    if (OnReachBoarder != null) OnReachBoarder.Invoke();
                }
            }

            if (m_currentInteractionTrigger.InteractionSide == InteractionSideEnum.Right)
            {
                if (m_transform.position.x <= ((m_initialTransforPosition - transform.right *m_xMaxMoveLeft).x - m_boxCollider.bounds.extents.x))
                {
                    EndInteraction();
                   // m_enableInteraction = false;
                    OnInteractableObjectOutRange();
                    if (OnReachBoarder != null) OnReachBoarder.Invoke();
                }
            }

            if (m_currentInteractionTrigger.InteractionSide == InteractionSideEnum.Front)
            {
                if (m_transform.position.z >= ((m_initialTransforPosition + transform.forward * m_zMaxMoveBack).z- m_boxCollider.bounds.extents.z))
                {
                    Debug.Log("end interation ++");
                    EndInteraction();
                    // m_enableInteraction = false;
                    OnInteractableObjectOutRange();
                    if (OnReachBoarderZBack != null) OnReachBoarderZBack.Invoke();
                }
            }
        }

        protected override void OnTriggerEnterCallback(Collider other, InteractableInRangeTrigger trigger)
        {
            base.OnTriggerEnterCallback(other, trigger);
        }

        protected override void OnTriggerExitCallback(Collider other, InteractableInRangeTrigger trigger)
        {
            base.OnTriggerExitCallback(other, trigger);
        }
    }
}

