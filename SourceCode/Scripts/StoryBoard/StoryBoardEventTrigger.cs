using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cgl.Thesis
{
    public class StoryBoardEventTrigger : MonoBehaviour
    {
        public int m_eventID;
        public delegate void TriggerDelegate(Collider collider, StoryBoardEventTrigger trigger);
        public TriggerDelegate OnTriggerEnterEvent;
        public TriggerDelegate OnTriggerExitEvent;

        public int EventID
        {
            get { return m_eventID; }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        protected void OnTriggerEnter(Collider other)
        {
            if (OnTriggerEnterEvent != null) OnTriggerEnterEvent(other, this);

        }

        protected void OnTriggerExit(Collider other)
        {
            if (OnTriggerExitEvent != null) OnTriggerExitEvent(other, this);
        }
    }


}


