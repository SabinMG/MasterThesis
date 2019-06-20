using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;


namespace Cgl.Thesis
{
    public class InteractableLadder : InteractableObjectFBBIK
    {
        public float maxAngle = 50.0f;
        private float m_currentPlayerClimbPercentage;

        public Transform m_ladderDownPositionTrans;
        public Transform m_ladderUpPositionTrans;

        public Vector3 m_ladderDirection;
        public float m_ladderLength;

        public int m_downTriggerID = 1;
        public int m_upTriggerID = 2;
        public Collider m_laddderUpGateCollider;

        public enum DirectionClimb
        {
            Up,
            Down
        }

        public DirectionClimb m_currentPlayerClimbDirection;
       // private Vector3 m_interactionTargetPivotOffset;

        protected override void Awake()
        {
            base.Awake();
        }

        // Use this for initialization
        void Start()
        {
            Vector3 ladderDirVec = (m_ladderUpPositionTrans.position - m_ladderDownPositionTrans.position);
            m_ladderDirection = ladderDirVec.normalized;
            m_ladderLength = ladderDirVec.magnitude;
           // m_interactionTargetPivotOffset = m_ladderUpPositionTrans.position - m_InteractionTargetPivot.position;
        }

        // Update is called once per frame
        public override void Update() // update only called on base object
        {
            base.Update();

            if (m_isInInteraction)
            {
                if (m_currentPlayerClimbDirection == DirectionClimb.Up && Mathf.Approximately(m_currentPlayerClimbPercentage, 1.0f))
                {
                    EndInteraction();
                    OnInteractableObjectOutRange();
                    m_currentPlayerClimbDirection = DirectionClimb.Down;
                }

                if (m_currentPlayerClimbDirection == DirectionClimb.Down && Mathf.Approximately(m_currentPlayerClimbPercentage, 0.0f))
                {
                    EndInteraction();
                    m_currentPlayerClimbDirection = DirectionClimb.Up;
                    //OnInteractableObjectOutRange();
                }


            }
        }

        public override bool CheckForInteraction()
        {
            if (m_isInInteraction) return true;
            Vector3 forward = m_currentInteractionTrigger.InteractionForward;
            Vector3 toOther = m_player.transform.position - m_currentInteractionTrigger.transform.position;
            if (Vector3.Dot(forward, toOther) > 0)
            {
                float angle = Vector3.Angle(forward, m_player.transform.forward);
                if (angle < maxAngle)
                {
                    return true;
                }
            }
            return false;
        }

        void OnDrawGizmosSelected()
        {
            if ((m_ladderUpPositionTrans != null && m_ladderUpPositionTrans != null))
            {
                // Draws a blue line from this transform to the target
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(m_ladderDownPositionTrans.position, m_ladderUpPositionTrans.position);
                //Handles.Label(transform.position, "Text");
            }
        }

        public Quaternion GetPlayerRotationOnLader()
        {
            return m_transform.rotation;
        }

        public Vector3 GetPlayerPositionOnLader(float inputDirection, float climbSpeed)
        {
            if (inputDirection != 0.0f)
            {
                if (inputDirection > 0) m_currentPlayerClimbDirection = DirectionClimb.Up;
                else m_currentPlayerClimbDirection = DirectionClimb.Down;
            }
            
            m_currentPlayerClimbPercentage += inputDirection * climbSpeed * Time.deltaTime;
            m_currentPlayerClimbPercentage = Mathf.Clamp(m_currentPlayerClimbPercentage, 0.0f, 1.0f);

            Vector3 positionOnLadder = m_ladderDownPositionTrans.position + m_ladderDirection * m_ladderLength * m_currentPlayerClimbPercentage;
            m_InteractionTargetPivot.position = positionOnLadder + m_ladderDirection * .6f + m_ladderDownPositionTrans.forward*.1f;

            return positionOnLadder;
        }

        private void SetInteractionTargetPivotPosition()
        {
            Vector3 positionOnLadder = m_ladderDownPositionTrans.position + m_ladderDirection * m_ladderLength * m_currentPlayerClimbPercentage;
            m_InteractionTargetPivot.position = positionOnLadder + m_ladderDirection *.6f + m_ladderDownPositionTrans.forward * .1f; ;
        }

        public override void StartInteraction(InteractionSystem interactionSystem)
        {
            if (m_isInInteraction) return;
            m_InteractionSystem = interactionSystem;
            m_isInInteraction = true;

            m_laddderUpGateCollider.enabled = false;
            SetInteractionTargetPivotPosition();

            m_InteractionSystem.StartInteraction(FullBodyBipedEffector.LeftHand, m_finalIKInteractionObject, true);
            m_InteractionSystem.StartInteraction(FullBodyBipedEffector.RightHand, m_finalIKInteractionObject, true);

            MovePlayerToRightPosAndStartInteract();
        }


        private void MovePlayerToRightPosAndStartInteract()
        {
            Vector3 playerPos = m_currentInteractionTrigger.transform.position;
            playerPos.y = m_player.transform.position.y;

            Vector3 newPlayerPos = playerPos + m_currentInteractionTrigger.InteractionForward * m_playerMinAproachDistance;
            m_player.MovePlayerTo(newPlayerPos, false, () => 
            {
               
            });
        }


        public override void EndInteraction()
        {
            if (!m_isInInteraction) return;
            if (Mathf.Approximately(m_currentPlayerClimbPercentage, 0.0f) || Mathf.Approximately(m_currentPlayerClimbPercentage, 1.0f))
            {
                base.EndInteraction();
                m_InteractionSystem.StopInteraction(FullBodyBipedEffector.LeftHand);
                m_InteractionSystem.StopInteraction(FullBodyBipedEffector.RightHand);
                m_isInInteraction = false;
                Debug.Log("end ladder interaction");
            }

            Invoke("DesableGateCollider", 1.0f);
        }

        private void DesableGateCollider()
        {
            m_laddderUpGateCollider.enabled = true;
        }

        protected override void OnTriggerEnterCallback(Collider other, InteractableInRangeTrigger trigger)
        {
            if (m_isInInteraction) return; // if in interaction we need to exit the ladder first
            if(trigger.TriggerID == m_downTriggerID)
            {
                m_currentPlayerClimbPercentage = 0.0f;
                m_currentPlayerClimbDirection = DirectionClimb.Up;
            }
            else if (trigger.TriggerID == m_upTriggerID)
            {
                m_currentPlayerClimbPercentage = 1.0f;
                m_currentPlayerClimbDirection = DirectionClimb.Down;
            }

            // there are two colliders so no need to enter two times until it ext both
            base.OnTriggerEnterCallback(other, trigger);
        }

        protected override void OnTriggerExitCallback(Collider other, InteractableInRangeTrigger trigger)
        {
            // exit condition are handled from the player position on the ladder
           if(!m_isInInteraction)
           base.OnTriggerExitCallback(other, trigger);
        }
    }
}

