using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RootMotion.FinalIK;
using DG.Tweening;


namespace Cgl.Thesis
{
    public class InteractablePickableObject : InteractableObjectFBBIK
    {
        public float maxAngle = 50.0f;
        public int m_pickTirggerID = 1;
        public int m_dropTriggerID = 2;

        public bool m_useRightHand = true;
        public bool m_useLeftHand = false;

        public Transform m_dropPosition;
        private bool m_pickedTheObject;



        public UnityEvent m_onPickEvent;
        public UnityEvent m_onDropEvent;
        public UnityEvent m_onInteractionComplete;
        
        public enum PickIngState
        {
            Default,
            PIckObject,
            DropObject
        }
       
        public override void StartInteraction(InteractionSystem interactionSystem)
        {
            if (m_currentInteractionTrigger.TriggerID == m_pickTirggerID)
            {
                if (!m_enableInteraction || m_pickedTheObject) return;

                base.StartInteraction(interactionSystem);
                m_InteractionSystem = interactionSystem;
                m_InteractionSystem.StartInteraction(FullBodyBipedEffector.RightHand, m_finalIKInteractionObject, true);
                if (m_useLeftHand)
                {
                    m_InteractionSystem.StartInteraction(FullBodyBipedEffector.LeftHand, m_finalIKInteractionObject, true);
                }

                m_pickedTheObject = true;

                if (m_onPickEvent != null) m_onPickEvent.Invoke();
            }

            if (m_currentInteractionTrigger.TriggerID == m_dropTriggerID)
            {
                if (m_pickedTheObject)
                {
                    var poser = transform.parent.GetComponent<Poser>();
                    if (poser != null)
                    {
                        poser.poseRoot = null;
                        poser.weight = 0f;
                    }

                    DropedTheObeject();
                    transform.parent = null;
                    m_player.SetRightArmEffector(1.0f,this.transform);
                    transform.DOMove(m_dropPosition.position,1.0f).OnComplete(() => 
                    {
                        float myFloat = 1.0f;
                        DOTween.To(() => myFloat, x => myFloat = x, 0.0f, 2.0f).OnUpdate(()=> {m_player.SetRightArmEffector(myFloat, null); }).SetOptions(false);

                        if (m_onDropEvent != null) m_onDropEvent.Invoke();
                        if (m_onInteractionComplete != null) m_onInteractionComplete.Invoke();
                    });
                }
               
                //if (!m_enableInteraction) return;
                //base.StartInteraction(interactionSystem);
                //m_InteractionSystem = interactionSystem;
                //m_InteractionSystem.StartInteraction(FullBodyBipedEffector.RightHand, m_finalIKInteractionObject, true);
            }
        }


        public void ResumeInteraction()
        {
            m_InteractionSystem.ResumeAll();
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

     
        public void PickedTheObject()
        {
            HideIntercationIndicator(Vector3.zero);
        }

        public void DropedTheObeject()
        {
            HideIntercationIndicator(Vector3.zero);
        }

        public override void ShowInteractionIndicator(Vector3 pos)
        {
            if (m_pickedTheObject)
            {
                base.ShowInteractionIndicator(pos);
            }
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
