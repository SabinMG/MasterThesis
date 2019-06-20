using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cgl.Thesis.Utilities;
using Cgl.Thesis.Data;

namespace Cgl.Thesis
{
    public class SceneActionTrigger : MonoBehaviour
    {
        public bool m_TriggerOnce = false;
        public StringSO m_otherObjectTagSO;
        public ConditionBaseSO m_condition; // if(true will enable)

        public bool m_canInteractFromAnySide = false;
        public bool m_unparentOnStart = true;

        public float m_customForwardAngle;
        public float m_maxAngle = 90;
        public UnityEvent m_onTriggerEnter;
        public UnityEvent m_onTriggerExit;

        private Vector3 m_interactionForward;
        private bool m_TriggeredOnce = false;

        void Awake()
        {
            if (m_unparentOnStart) transform.parent = null;
        }

        void OnDrawGizmosSelected()
        {
            // Draws a 5 unit long red line in front of the object
            Gizmos.color = Color.red;
            m_interactionForward = Quaternion.AngleAxis(-m_customForwardAngle, transform.up) * transform.forward;
            Gizmos.DrawRay(transform.position, m_interactionForward);
        }

        // Start is called before the first frame update
        void Start()
        {
            m_interactionForward = Quaternion.AngleAxis(-m_customForwardAngle, transform.up) * transform.forward;
        }

        // Update is called once per frame
        void Update()
        {

        }

        protected void OnTriggerEnter(Collider other)
        {
            if (m_TriggerOnce && m_TriggeredOnce) return;

            if (other.gameObject.tag == m_otherObjectTagSO.Value) // code must be reafatored , my brain is nt working
            {
                if (m_canInteractFromAnySide)
                {
                    TriggerSceneAction();
                }
                else
                {
                    bool canRaiseEvent = CanRaiseEvent(other.transform);
                    if (canRaiseEvent) TriggerSceneAction();  
                }
            }
        }

        private void TriggerSceneAction()
        {
            if (m_onTriggerEnter != null)
            {
                if (m_condition != null)
                {
                    if (m_condition.IsSatisfied() == true)
                    {
                        m_TriggeredOnce = true;
                        m_onTriggerEnter.Invoke();
                    }
                }
                else
                {
                    m_TriggeredOnce = true;
                    m_onTriggerEnter.Invoke();
                }
            }
        }

        protected void OnTriggerExit(Collider other)
        {
            // if (OnTriggerExitEvent != null) OnTriggerExitEvent(other, this);
        }


        public bool CanRaiseEvent(Transform otherTrans)
        {
            Vector3 forward = this.m_interactionForward;
            Vector3 toOther = this.transform.position - otherTrans.position;
            if (Vector3.Dot(forward, toOther) > 0)
            {
                float angle = Vector3.Angle(forward, otherTrans.forward);
                if (angle < m_maxAngle)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
