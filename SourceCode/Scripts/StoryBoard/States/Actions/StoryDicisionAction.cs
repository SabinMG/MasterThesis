using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Cgl.Thesis
{
    public class StoryDicisionAction : StoryAction
    {
        public StoryDecision m_storyDicision;
        public bool m_everyFrame;

        public UnityEvent OnTrueEvent;
        public UnityEvent OnFalseEvent;

        public override void OnActionBegin()
        {
            m_storyDicision.StartDecide();
        }

        public override void OnAtionUpdate()
        {
            if (!m_everyFrame)
            {
                bool dicisionTruse = m_storyDicision.Decide();
                if (dicisionTruse)
                {
                    if (OnTrueEvent != null) OnTrueEvent.Invoke();

                }
                else
                {
                    if (OnFalseEvent != null) OnFalseEvent.Invoke();
                }
                FinishAction(this);
                return;
            }

            bool dicisionTrue = m_storyDicision.Decide();
            if (dicisionTrue)
            {
                if (OnTrueEvent != null) OnTrueEvent.Invoke();
                FinishAction(this);
            }  
        }

        public override void OnActionFinished()
        {
            m_storyDicision.EndDecide();
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
