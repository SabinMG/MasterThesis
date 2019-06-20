using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cgl.Thesis
{
    public class StoryPictureFrameDicision : StoryDecision
    {
        public enum InteractionEventType
        {
            Start,
            End
        }

        public InteractionEventType m_InteractionEventType;
        public InteractablePictureFrame m_interactablePictureFrame;

        private bool m_checkForInteractionEvents = false;
        private bool m_dicisionConditionSatisfied = false;
       
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void OnEnable()
        {
            base.OnEnable();
            m_interactablePictureFrame.OnShowPictureFrameStartEvent += OnShowPictureFrameStartEvent;
            m_interactablePictureFrame.OnShowPictureFrameEndEvent += OnShowPictureFrameEndEvent;
          
        }

        public override void OnDisable()
        {
            base.OnDisable();
            m_interactablePictureFrame.OnShowPictureFrameStartEvent -= OnShowPictureFrameStartEvent;
            m_interactablePictureFrame.OnShowPictureFrameEndEvent -= OnShowPictureFrameEndEvent;
        }

        public override void StartDecide()
        {
        }

        public override void EndDecide()
        {

        }

        public override bool Decide()
        {
            m_checkForInteractionEvents = true;
            return m_dicisionConditionSatisfied;
        }

        private void OnShowPictureFrameStartEvent(InteractableObjectFBBIK obj)
        {
            if (m_checkForInteractionEvents)
            {
                if (m_InteractionEventType == InteractionEventType.Start)
                {
                    m_status = true;
                    m_dicisionConditionSatisfied = true;
                }
            }
        }

        private void OnShowPictureFrameEndEvent(InteractableObjectFBBIK obj)
        {
            if (m_checkForInteractionEvents)
            {
                if (m_InteractionEventType == InteractionEventType.End)
                {
                    m_status = true;
                    m_dicisionConditionSatisfied = true;
                }
            }      
        }     
    }
}
