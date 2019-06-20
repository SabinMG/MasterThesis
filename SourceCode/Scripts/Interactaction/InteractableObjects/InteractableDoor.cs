using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RootMotion.FinalIK;


namespace Cgl.Thesis
{
    public class InteractableDoor : InteractableObjectFBBIK
    {
        public enum DoorState
        {
            Opened,
            Closed
        }

        public enum DoorOpenType
        {
            HalfOpen,
            FullOpen
        }

        public DoorState m_currentDoorState = DoorState.Closed;
        public DoorOpenType m_currentDoorOpenType = DoorOpenType.HalfOpen;

        public float maxAngle  = 50.0f;
        public Transform m_doorOpenHinge;
        public float m_doorOpenCloseTime = 2.0f;

        public UnityEvent OnDoorOpenEvent;
        public UnityEvent OnDoorCloseEvent;


        private bool m_isDoorInOpenCloseAnimation = false;

        private Quaternion m_doorClosedLocalRotation;
        private Quaternion m_doorOpenedLocalRotation;

        public DoorState CurrentDoorState
        {
            get { return m_currentDoorState; }
        }

        protected override void Awake()
        {
            base.Awake();
        }

        // Use this for initialization
        void Start()
        {
            m_doorClosedLocalRotation = m_doorOpenHinge.localRotation;

            if (m_currentDoorOpenType == DoorOpenType.HalfOpen)
            {
                m_doorOpenedLocalRotation = Quaternion.Euler(0, 90, 0);
            }
            else if (m_currentDoorOpenType == DoorOpenType.FullOpen)
            {
                m_doorOpenedLocalRotation = Quaternion.Euler(0, -180, 0);
            }
        }

        public override bool CheckForInteraction()
        {
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

        public override void StartInteraction(InteractionSystem interactionSystem)
        {
            if (!m_enableInteraction) return;
            base.StartInteraction(interactionSystem);
            m_InteractionSystem = interactionSystem;
            m_InteractionSystem.StartInteraction(FullBodyBipedEffector.LeftHand, m_finalIKInteractionObject, true);

            MovePlayerToRightPos();
        }

        private void MovePlayerToRightPos()
        {
            Vector3 playerPos = m_currentInteractionTrigger.transform.position;
            playerPos.y = m_player.transform.position.y;

            Vector3 newPlayerPos = playerPos + m_currentInteractionTrigger.InteractionForward * m_playerMinAproachDistance;
            m_player.MovePlayerTo(newPlayerPos, false, () => { });
        }

        public void DoDoorButtonPressAction()
        {
            if (m_isDoorInOpenCloseAnimation) return;
            if (m_currentDoorState == DoorState.Closed)
            {
                StartCoroutine(IEPlayDoorOpeningAnimation());
              
               // Debug.Log("door opening");
            }
            else if (m_currentDoorState == DoorState.Opened)
            {
                StartCoroutine(IEPlayDoorClosingAnimation());
              
               // Debug.Log("door closing");
            }
        }
        public void CloseDoor()
        {
            StartCoroutine(IEPlayDoorClosingAnimation());
        }

        IEnumerator IEPlayDoorOpeningAnimation()
        {
            if (OnDoorOpenEvent != null) OnDoorOpenEvent.Invoke();
            m_isDoorInOpenCloseAnimation = true;
            float timeT = 0.0f;
            while (timeT <= m_doorOpenCloseTime)
            {
               float intrTime = timeT / m_doorOpenCloseTime;
               m_doorOpenHinge.localRotation = Quaternion.Lerp(m_doorClosedLocalRotation, m_doorOpenedLocalRotation, intrTime);
               timeT += Time.deltaTime;
                yield return null;
            }

            m_currentDoorState = DoorState.Opened;
            yield return null;
            m_isDoorInOpenCloseAnimation = false;
        }

        IEnumerator IEPlayDoorClosingAnimation()
        {
            if (OnDoorCloseEvent != null) OnDoorCloseEvent.Invoke();
            m_isDoorInOpenCloseAnimation = true;
            float timeT = 0.0f;
            while (timeT <= m_doorOpenCloseTime)
            {
                float intrTime = timeT / m_doorOpenCloseTime;
                m_doorOpenHinge.localRotation = Quaternion.Lerp(m_doorOpenedLocalRotation, m_doorClosedLocalRotation, intrTime);
                timeT += Time.deltaTime;
                yield return null;
            }
            m_currentDoorState = DoorState.Closed;
            yield return null;
            m_isDoorInOpenCloseAnimation = false;
        }

        public void InterctionEvent(FullBodyBipedEffector effectorType, InteractionObject interactionObject, InteractionObject.InteractionEvent interactionEvent)
        {
            Debug.Log(interactionEvent.time);
            //m_doorAnimator.SetBool("OpenDoor", true); 
        }

        public override void EndInteraction()
        {
            m_InteractionSystem.StopInteraction(FullBodyBipedEffector.RightHand);
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
