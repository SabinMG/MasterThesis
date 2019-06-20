using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cgl.Thesis.Data;
using DG.Tweening;

namespace Cgl.Thesis
{
    public class LevelMovableLand : MonoBehaviour
    {
        public enum LandPosition
        {
            Start,
            Mid,
            End,
            Moving
        }
        public LandPosition m_currentState;

        public Transform m_startPositionTrans;
        public Transform m_midPositionTrans;
        public Transform m_endPositionTrans;
        public float m_moveSpeed;

        public UnityEvent m_onReachStartPosition;
        public UnityEvent m_onReachMidPosition;
        public UnityEvent m_onReachEndPosition;

        private Vector3 m_startPosition;
        private Vector3 m_midPosition;
        private Vector3 m_endPosition;

        // Start is called before the first frame update
        void Start()
        {
            m_startPosition = m_startPositionTrans.position;
            m_midPosition = m_midPositionTrans.position;
            m_endPosition = m_endPositionTrans.position;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void MovetoStart()
        {
            if (m_currentState == LandPosition.Start || m_currentState == LandPosition.Moving) return;
            m_currentState = LandPosition.Moving;
            transform.DOMove(m_startPosition, m_moveSpeed).SetEase(Ease.Linear).OnComplete(() => { m_currentState = LandPosition.Start; if (m_onReachStartPosition != null) m_onReachStartPosition.Invoke(); });
        }

        public void MovetoMid()
        {
            if (m_currentState == LandPosition.Mid || m_currentState == LandPosition.Moving) return;
            m_currentState = LandPosition.Moving;
            transform.DOMove(m_midPosition, m_moveSpeed).SetEase(Ease.Linear).OnComplete(() => { m_currentState = LandPosition.Mid; if (m_onReachMidPosition != null) m_onReachMidPosition.Invoke(); });
        }

        public void MovetoEnd()
        {
            if (m_currentState == LandPosition.End || m_currentState == LandPosition.Moving) return;
            m_currentState = LandPosition.Moving;
            transform.DOMove(m_endPosition, m_moveSpeed).SetEase(Ease.Linear).OnComplete(() => { m_currentState = LandPosition.End; if (m_onReachEndPosition != null) m_onReachEndPosition.Invoke(); });
        }
    }
}
