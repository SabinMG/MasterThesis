using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cgl.Thesis.Data;
using DG.Tweening;

namespace Cgl.Thesis
{
    public class LevelRailBox : MonoBehaviour
    {
            public enum RailboxPosition
            {
                Start,
                End,
                Moving
            }

            public RailboxPosition m_currentState;

            public Transform m_startPositionTrans;
            public Transform m_endPositionTrans;
            public float m_moveSpeed;

            public UnityEvent m_onReachStartPosition;
            public UnityEvent m_onReachEndPosition;

            private Vector3 m_startPosition;
            private Vector3 m_endPosition;

            // Start is called before the first frame update
            void Start()
            {
                m_startPosition = m_startPositionTrans.position;
                m_endPosition = m_endPositionTrans.position;
            }

            // Update is called once per frame
            void Update()
            {
            
            }

            public void MovetoStart()
            {
                if (m_currentState == RailboxPosition.Start || m_currentState == RailboxPosition.Moving) return;
                m_currentState = RailboxPosition.Moving;
                transform.DOMove(m_startPosition, m_moveSpeed).SetEase(Ease.Linear).OnComplete(() => { m_currentState = RailboxPosition.Start;  if (m_onReachStartPosition != null) m_onReachStartPosition.Invoke(); }); 
            }

            public void MovetoEnd()
            {
                if (m_currentState == RailboxPosition.End || m_currentState ==  RailboxPosition.Moving) return;
                m_currentState = RailboxPosition.Moving;
                transform.DOMove(m_endPosition, m_moveSpeed).SetEase(Ease.Linear).OnComplete(() => { m_currentState = RailboxPosition.End; if (m_onReachEndPosition != null) m_onReachEndPosition.Invoke(); }); 
            }
    }
}
