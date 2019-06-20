using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RootMotion.FinalIK;
using DG.Tweening;

namespace Cgl.Thesis
{
    public class InteractableLever : InteractableObjectFBBIK
    {
        public float maxAngle = 50.0f;
        public Transform m_liverHandleTrans;
        public Vector3 m_leverHandleOnRotation;
        public float m_leverRotaionSpeed;

        public UnityEvent m_onInteractionComplete;

        public override void StartInteraction(InteractionSystem interactionSystem)
        {
            if (!m_enableInteraction) return;

            base.StartInteraction(interactionSystem);
            m_InteractionSystem = interactionSystem;
            m_InteractionSystem.StartInteraction(FullBodyBipedEffector.RightHand, m_finalIKInteractionObject, true); 
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

        private void MovePlayerToRightPos()
        {
            Vector3 playerPos = m_currentInteractionTrigger.transform.position;
            playerPos.y = m_player.transform.position.y;

            Vector3 newPlayerPos = playerPos + m_currentInteractionTrigger.InteractionForward * m_playerMinAproachDistance;
            m_player.MovePlayerTo(newPlayerPos, false, () => { });
        }


        public void MoveLever()
        {
            m_liverHandleTrans.DOLocalRotate(m_leverHandleOnRotation, m_leverRotaionSpeed).OnComplete(() => 
            {
                if (m_onInteractionComplete != null) m_onInteractionComplete.Invoke();

            });

            //HideIntercationIndicator(Vector3.zero);
        }

        public override void ShowInteractionIndicator(Vector3 pos)
        {
            //if (m_pickedTheObject)
            //{
            //    base.ShowInteractionIndicator(pos);
            //}
        }

        public override void HideIntercationIndicator(Vector3 pos)
        {
            base.HideIntercationIndicator(pos);
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