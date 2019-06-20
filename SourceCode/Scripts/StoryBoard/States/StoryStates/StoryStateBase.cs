using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Cgl.Thesis
{
    public class StoryStateBase : MonoBehaviour
    {
        public StoryAction[] m_actions;
        public StoryAction m_currentAction;
        public int m_currentActionIndex;

        public UnityEvent OnStateFinishedEvent;

        public void FinisheAction(StoryAction stroyAction)
        {
            stroyAction.OnActionFinished();
            if (m_currentActionIndex + 1 < m_actions.Length)
            {
                NextState();
            }
            else
            {
                m_currentAction = null;
            } 
        }

        private void NextState()
        {
            m_currentAction.OnActionFinished();
            m_currentActionIndex++;
            m_currentAction = m_actions[m_currentActionIndex];
            m_currentAction.OnActionBegin();
        }


        public virtual void OnStateEnter()
        {
            m_currentAction = m_actions[0];
            for (int i = 0; i < m_actions.Length; i++)
            {
                m_actions[i].CurrentStoryStateBase = this;
                m_currentAction.OnActionBegin();
            }
        }

        public virtual void OnStateUpdate()
        {
            if (m_currentAction == null) return;
            m_currentAction.OnAtionUpdate();
        }

        public virtual void OnStateExit()
        {

        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

       
    }
}
