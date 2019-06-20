using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Cgl.Thesis
{
    public class EventInvoker : MonoBehaviour
    {
        public bool m_invokeOnstart;
        public float m_delayOnStart;
        public UnityEvent OnEventInvoke;
       
        // Use this for initialization
        void Start()
        {
            if (m_invokeOnstart)
            {
                Invoke("InvokeEvent", m_delayOnStart);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void InvokeEventWithDelay(float delay)
        {
            Invoke("InvokeEvent", delay);
        }

        public void InvokeEvent()
        {
            if (OnEventInvoke != null) OnEventInvoke.Invoke();
        }
    }
}
