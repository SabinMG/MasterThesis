using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cgl.Thesis
{
    public class StoryTriggerDicision : StoryDecision
    {
        public enum TriggerType
        {
            Enter,
            Exit
        }

        public bool m_simulateTigger;
        public TriggerType m_triggerTypeCondition;
        public string m_colliderTag;

        public StoryBoardEventTrigger m_storyBoardEventTrigger;
        private bool m_checkForTriggerEvents = false;
        private bool m_dicisionConditionSatisfied = false;

        public override void StartDecide()
        {
        }

        public override void EndDecide()
        {

        }

        public override bool Decide()
        {
            m_checkForTriggerEvents = true;
            if (m_simulateTigger) return true;

            return m_dicisionConditionSatisfied;
        }

        public override void OnEnable()
        {
            base.OnEnable();
            m_storyBoardEventTrigger.OnTriggerEnterEvent += OnTriggerEnterEventCallback;
            m_storyBoardEventTrigger.OnTriggerExitEvent += OnTriggerExitEventCallback;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            m_storyBoardEventTrigger.OnTriggerEnterEvent += OnTriggerEnterEventCallback;
            m_storyBoardEventTrigger.OnTriggerExitEvent += OnTriggerExitEventCallback;
        }

        public virtual void OnTriggerEnterEventCallback(Collider collider, StoryBoardEventTrigger trigger)
        {
            if (m_checkForTriggerEvents)
            {
                if (m_triggerTypeCondition == TriggerType.Enter && collider.tag == m_colliderTag)
                {
                    m_status = true;
                    m_dicisionConditionSatisfied = true;
                }  
            }
        }

        public virtual void OnTriggerExitEventCallback(Collider collider, StoryBoardEventTrigger trigger)
        {
            if (m_checkForTriggerEvents)
            {
                if (m_triggerTypeCondition == TriggerType.Exit && collider.tag == m_colliderTag)
                {
                    m_status = true;
                    m_dicisionConditionSatisfied = true;
                }
            }
        }
    }
}
