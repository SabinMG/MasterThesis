using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Cgl.Thesis
{
    public class StoryAction : MonoBehaviour
    {
       // public bool m_canIntruptState;
        private StoryStateBase m_currentStoryStateBase;

        public StoryStateBase CurrentStoryStateBase
        {
            get
            {
                return m_currentStoryStateBase;
            }

            set
            {
                m_currentStoryStateBase = value;
            }
        }

        public virtual void OnActionBegin()
        {

        }

        public virtual void OnAtionUpdate()
        {

        }

        public virtual void OnActionFinished()
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

        public virtual void FinishAction(StoryAction action)
        {
            m_currentStoryStateBase.FinisheAction(action);
        }
    }


}
