using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cgl.Thesis.Data;
using DG.Tweening;

namespace Cgl.Thesis
{
    public class LevelElevator : MonoBehaviour
    {
    
        public enum ElevatorState
        {
            Up,
            Down
        }

        public ElevatorState m_currentState;

        public Transform m_upPositionTrans;
        public Transform m_downPositionTrans;

        public float m_moveSpeed;

        public UnityEvent m_onElevattoReachUP;
        public UnityEvent m_onElevattoReachDown;

        private Vector3 m_downPosition;
        private Vector3 m_upPosition;

        // Start is called before the first frame update
        void Start()
        {
            m_downPosition = m_downPositionTrans.position;
            m_upPosition = m_upPositionTrans.position;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void MoveDown()
        {
            if (m_currentState == ElevatorState.Down) return;
            transform.DOMove(m_downPosition, m_moveSpeed).OnComplete(() => { if (m_onElevattoReachDown != null) m_onElevattoReachDown.Invoke(); }); ;
            m_currentState = ElevatorState.Down;
        }

        public void MoveUP()
        {
            if (m_currentState == ElevatorState.Up) return;
            transform.DOMove(m_upPosition, m_moveSpeed).OnComplete(() => { if (m_onElevattoReachUP != null) m_onElevattoReachUP.Invoke(); });
            m_currentState = ElevatorState.Up;
        }
    }
}
