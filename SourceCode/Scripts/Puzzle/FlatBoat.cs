using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;


namespace Cgl.Thesis
{
    public class FlatBoat : MonoBehaviour
    {
        public enum PositionState
        {
            Start,
            End,
            Moving
        }
        public Transform m_destinationStartTransform;
        public Transform m_destinationEndTranform;

        public Transform m_RightEdgePoint;
        public Transform m_leftEdgePoint;

        public PositionState m_currentPositionState;
        public float m_moveDuration = 3.0f;
        public UnityEvent m_onBoatReachOnStartPoint;
        public UnityEvent m_onBoatReachOnEndPoint;
        public UnityEvent m_onStartMoving;

        public UnityEvent m_onMotherEnterBoat; // not generic
        public UnityEvent m_onMotherExitBoat;


        private Vector3 m_destinationStartPosition;
        private Vector3 m_destinationEndPosition;

        // Start is called before the first frame update
        void Start()
        {
            m_destinationStartPosition = m_destinationStartTransform.position;
            m_destinationEndPosition = m_destinationEndTranform.position;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void MoveSwitch()
        {
            if ( m_currentPositionState == PositionState.Moving) return;
            if (m_currentPositionState == PositionState.Start) MovetoEndPoint();
            else MovetoStartPoint();
        }

        public void MovetoStartPoint()
        {
            if (m_currentPositionState == PositionState.Start || m_currentPositionState == PositionState.Moving) return;
            m_currentPositionState = PositionState.Moving;
            if (m_onStartMoving != null) m_onStartMoving.Invoke();

            transform.DOMove(m_destinationStartPosition, m_moveDuration).SetEase(Ease.Linear).OnComplete(() =>
            {
                m_currentPositionState = PositionState.Start;
                if (m_onBoatReachOnStartPoint != null) m_onBoatReachOnStartPoint.Invoke();
            });
        }

        public void MovetoEndPoint()
        {
            if (m_currentPositionState == PositionState.End || m_currentPositionState == PositionState.Moving) return;
            m_currentPositionState = PositionState.Moving;

            if (m_onStartMoving != null) m_onStartMoving.Invoke();
           
            transform.DOMove(m_destinationEndPosition, m_moveDuration).SetEase(Ease.Linear).OnComplete(() =>
            {
                m_currentPositionState = PositionState.End;
                if (m_onBoatReachOnEndPoint != null) m_onBoatReachOnEndPoint.Invoke();
            });
        }
    }
}
