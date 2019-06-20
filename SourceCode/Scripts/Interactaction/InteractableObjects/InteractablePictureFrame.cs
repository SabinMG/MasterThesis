using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using DG.Tweening;
using UnityEngine.Events;


namespace Cgl.Thesis
{
    public class InteractablePictureFrame : InteractableObjectFBBIK
    {
        public enum PictureState
        {
            Hidden,
            Shown
        }

        public PictureState m_currentPictureState = PictureState.Hidden;
        public float maxViewAngle = 50.0f;

        public GameObject m_picture;
        public Light m_pictureFrameLight;

        public InteractableActionDelegate OnShowPictureFrameStartEvent;
        public InteractableActionDelegate OnShowPictureFrameEndEvent;

        public float m_picutureShowSpeed = 3.0f;


        protected override void Awake()
        {
            base.Awake();
        }

        // Use this for initialization
        void Start()
        {
            m_pictureFrameLight.intensity = 0.0f;
        }

        public override bool CheckForInteraction()
        {
            Vector3 forward = m_currentInteractionTrigger.InteractionForward;
            Vector3 toOther = m_player.transform.position - m_currentInteractionTrigger.transform.position;
            if (Vector3.Dot(forward, toOther) > 0)
            {
                float angle = Vector3.Angle(forward, m_player.transform.forward);
                if (angle < maxViewAngle)
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

        public void PlayPictureShowAnimation()
        {
            Material mat = m_picture.GetComponent<MeshRenderer>().material;
            mat.DOColor(Color.white, "_Color", m_picutureShowSpeed);
            m_pictureFrameLight.DOIntensity(1.0f, m_picutureShowSpeed);
            if (OnShowPictureFrameStartEvent != null) OnShowPictureFrameStartEvent(this);
            StartCoroutine(IEPlayPictureShowAnimation());
        }

        IEnumerator IEPlayPictureShowAnimation()
        {
            yield return new WaitForSeconds(4.0f);
            if (OnShowPictureFrameEndEvent != null) OnShowPictureFrameEndEvent(this);
            yield return null;
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

