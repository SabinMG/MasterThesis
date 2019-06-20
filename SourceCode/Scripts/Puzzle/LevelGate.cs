using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cgl.Thesis.Data;
using UnityEngine.Events;

namespace Cgl.Thesis
{
    

    public class LevelGate : MonoBehaviour
    {
        public enum GateState
        {
            Closed,
            Opened
        }

        public GateState m_currentState;
        public float m_closedYRotation;
        public float m_openedYRotation;
        public float m_openCloseSpeed;

        public GameObject m_lockIndicator;
        public ConditionBaseSO m_indicatorShowCondition;

        public UnityEvent m_onDoorOpen;
        public UnityEvent m_onDoorClose;


        // Start is called before the first frame update
        void Start()
        {
            if (m_indicatorShowCondition == null) return;
            if (!m_indicatorShowCondition.IsSatisfied())
            {
                m_lockIndicator.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (m_indicatorShowCondition == null) return;

            if (!m_indicatorShowCondition.IsSatisfied())
            {
                m_lockIndicator.SetActive(false);
            }
            else
            {
                m_lockIndicator.SetActive(true);
            }
        }

        public void OpenGate()
        {
            if (m_currentState == GateState.Opened) return;
            transform.DORotate(new Vector3(0, m_openedYRotation, 0), m_openCloseSpeed).OnComplete(()=> { if (m_onDoorOpen != null) m_onDoorOpen.Invoke();});
            m_currentState = GateState.Opened;
        }

        public void CloseGate()
        {
            if (m_currentState == GateState.Closed) return;
            transform.DORotate(new Vector3(0, m_closedYRotation,0),m_openCloseSpeed).OnComplete(() => { if (m_onDoorClose != null) m_onDoorClose.Invoke();}); ;
            m_currentState = GateState.Closed;
        }
    }
}
