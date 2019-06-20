using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Cgl.Thesis
{
    public class StoryDelayAction : StoryAction
    {
        public float m_delayTime;
        private bool m_startedTimer;

        private float m_timerStartTime;
        public UnityEvent OnDelayEnded;

        public override void OnActionBegin()
        {
            m_startedTimer = true;
            m_timerStartTime = Time.time;
        }

        public override void OnAtionUpdate()
        {
            if (!m_startedTimer) return;
            if (Time.time >= m_timerStartTime + m_delayTime)
            {
                if (OnDelayEnded != null) OnDelayEnded.Invoke();
                m_startedTimer = false;
                FinishAction(this);
            } 
        }

        public override void OnActionFinished()
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
