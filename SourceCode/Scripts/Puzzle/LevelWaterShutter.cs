using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cgl.Thesis.Data;
using DG.Tweening;

namespace Cgl.Thesis
{
 
    public class LevelWaterShutter : MonoBehaviour
    {
        public enum ShutterState
        {
            Closed,
            Opened
        }

        public ShutterState m_currentState;

        public Transform m_openPositionTrans;
        public Transform m_closePositionTrans;

        public float m_openCloseSpeed;

        public UnityEvent m_onShutterOpen;
        public UnityEvent m_onShutterClose;

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

        public void OpenGate()
        {
           if (m_currentState == ShutterState.Opened) return;
            transform.DOMove(m_openPosition, m_openCloseSpeed).OnComplete(() => { if (m_onShutterOpen != null) m_onShutterOpen.Invoke(); }); ;
            m_currentState = ShutterState.Opened;
        }

        public void CloseGate()
        {
            if (m_currentState == ShutterState.Closed) return;
            transform.DOMove(m_closePosition, m_openCloseSpeed).OnComplete(()=> { if (m_onShutterClose != null) m_onShutterClose.Invoke(); });
            m_currentState = ShutterState.Closed;
        }
    }
}
