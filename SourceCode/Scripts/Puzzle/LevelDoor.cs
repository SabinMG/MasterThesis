using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;


namespace Cgl.Thesis
{
    public class LevelDoor : MonoBehaviour
    {
        public enum DoorState
        {
            Closed,
            Opened
        }

        public DoorState m_currentState;
        public Transform m_openPositionTrans;
        public Transform m_closePositionTrans;
        public Transform m_OpenCloseTranform;
        public float m_openCloseSpeed;

        public UnityEvent m_onDoorOpenEvent;
        public UnityEvent m_onDoorCloeseEvent;

        private Vector3 m_openPosition;
        private Vector3 m_closePosition;

        // Start is called before the first frame update
        void Start()
        {
            m_openPosition = m_openPositionTrans.position;
            m_closePosition = m_closePositionTrans.position;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OpenDoor()
        {
            if (m_currentState == DoorState.Opened) return;
            m_OpenCloseTranform.DOMove(m_openPosition, m_openCloseSpeed).OnComplete(() => { if (m_onDoorOpenEvent != null) m_onDoorOpenEvent.Invoke(); }); ;
            m_currentState = DoorState.Opened;
        }

        public void CloseDoor()
        {
            if (m_currentState == DoorState.Closed) return;
            m_OpenCloseTranform.DOMove(m_closePosition, m_openCloseSpeed).OnComplete(() => { if (m_onDoorCloeseEvent != null) m_onDoorCloeseEvent.Invoke(); });
            m_currentState = DoorState.Closed;
        }
    }
}
