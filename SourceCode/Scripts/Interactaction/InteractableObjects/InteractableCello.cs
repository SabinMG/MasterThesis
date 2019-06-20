using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RootMotion.FinalIK;
using DG.Tweening;
using Cgl.Thesis.Data;

namespace Cgl.Thesis
{
    public class InteractableCello : InteractableObjectFBBIK
    {
        public float maxAngle = 50.0f;
        public Animator m_bowAnimator;
        public float m_maxPlayTime = 4.0f;
        public GameObject[] m_musicNotes;
        public Transform m_noteStartTranformPosition;
        public MoveTowardsFlowPoint[] m_noteEndPoints;
        public BoolSO m_aboutFinishInidicator;

        public UnityEvent m_onInteractionComplete;
        public UnityEvent m_onStartPlayingCello;


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


        public void PlayCello()
        {

            if (m_onStartPlayingCello != null) m_onStartPlayingCello.Invoke();

            m_bowAnimator.SetTrigger("PlayCello");
            HideIntercationIndicator(Vector3.zero);
            //m_liverHandleTrans.DOLocalRotate(m_leverHandleOnRotation, m_leverRotaionSpeed).OnComplete(() =>
            //{
            //    if (m_onInteractionComplete != null) m_onInteractionComplete.Invoke();

            //});

            //HideIntercationIndicator(Vector3.zero);
            Invoke("StopPlayignCello", m_maxPlayTime);

        }

        public void ShowMuiscNote()
        {
            int random = Random.Range(0, 2);
            GameObject musicNote = m_musicNotes[random];

            GameObject noteRenderObject = Instantiate(musicNote, m_noteStartTranformPosition.position, Quaternion.identity);
            MusicNoteMoveTowardsFlow moveComp = noteRenderObject.GetComponent<MusicNoteMoveTowardsFlow>();

            moveComp.m_currentMovePoint = m_noteEndPoints[Random.Range(0, m_noteEndPoints.Length)];
            moveComp.StartMoving(true);
        }

        public void StopPlayignCello()
        {
            m_aboutFinishInidicator.Value = true;
            m_InteractionSystem.ResumeInteraction(FullBodyBipedEffector.RightHand);
            m_bowAnimator.speed = 0.0f;

            if (m_onInteractionComplete != null)
            {
                m_onInteractionComplete.Invoke();
            }
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