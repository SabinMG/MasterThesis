using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cgl.Thesis.Data;
using DG.Tweening;
using Cgl.Thesis.Utilities;

namespace Cgl.Thesis
{ 
    public class ClimbStepAlongWithBoxHelper : MonoBehaviour
    {
        public Transform m_interactableBoxTrans;
        public Transform m_stepEndPosTrans;
        public Transform m_motherStartPosTran;
        public UnityVector3Event m_boxMoveEvent;

        private Vector3 m_boxStartPosition;
        private Vector3 m_boxEndPosition;
        private Vector3 m_motherStartPosition;

        private float m_boxMoveDistance;
        private float m_motherMoveDistance;
        private Vector3 m_mohterMoveDirection;

        private bool m_reachedtheStepEnd;

     
        // Start is called before the first frame update
        void Start()
        {
            m_boxStartPosition = m_interactableBoxTrans.position;
            m_boxEndPosition = m_stepEndPosTrans.position;
            m_motherStartPosition = m_motherStartPosTran.position;

            m_boxMoveDistance = Mathf.Abs(Vector3.Magnitude(m_boxEndPosition - m_boxStartPosition));
            m_mohterMoveDirection = (m_boxEndPosition - m_motherStartPosition);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void OnBoxMove()
        {
            if (m_reachedtheStepEnd) return;
            float distanceAwayFromEnd = Mathf.Abs(Vector3.Magnitude(m_boxEndPosition - m_interactableBoxTrans.position));
            float percentage = distanceAwayFromEnd / m_boxMoveDistance;

            if (percentage <= 0.01f)
            {
                m_reachedtheStepEnd = true; return;
            }

            Vector3 motherPosition = m_motherStartPosition+ m_mohterMoveDirection*(1-percentage);
            if (m_boxMoveEvent != null)
            {
                m_boxMoveEvent.Invoke(motherPosition);
            }
        }
    }
}
