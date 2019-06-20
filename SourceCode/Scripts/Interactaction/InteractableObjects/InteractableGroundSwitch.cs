using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cgl.Thesis.Data;
using UnityEngine.Events;

namespace Cgl.Thesis
{
    public class InteractableGroundSwitch : InteractableObject
    {
        public Color m_onColor;
        public Color m_offColor;
        public string materialColorProperty = "_Color1_T";
        public StringSO m_otherObjectTagSO;
        public ConditionBaseSO m_condition;

        private bool m_jumpHitTop;

        public bool m_isSwithOn = false;
        public UnityEvent m_onSwitchOnEvent;
        public UnityEvent m_onSwitchOffEvent;
        private MeshRenderer m_renderer;
    
        private void Awake()
        {
            m_renderer = GetComponent<MeshRenderer>();
            SetColor();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnTriggerEnterTop(GameObject go)
        {
            if (m_condition != null) // condition must be satsidfied
            {
                if (!m_condition.IsSatisfied()) { return; }
            }

            if (m_otherObjectTagSO.Value == go.tag)
            {
                m_jumpHitTop = true;
            }
        }

        public void OnTriggerEnterBottom(GameObject go)
        {
            if (m_condition != null) //condition must be satsidfied
            {
                if (!m_condition.IsSatisfied()) { return; }
            }

            if (m_otherObjectTagSO.Value == go.tag)
            {
                if (m_jumpHitTop)
                {
                    m_isSwithOn = !m_isSwithOn;
                    if (m_isSwithOn)
                    {
                        if (m_onSwitchOnEvent != null) m_onSwitchOnEvent.Invoke();
                    }
                    else
                    {
                        if (m_onSwitchOffEvent != null) m_onSwitchOffEvent.Invoke();
                    }

                    PlayAnimation();
                    SetColor();
                }
                m_jumpHitTop = false;
            }
        }

        private void PlayAnimation()
        {

        }

        private void SetColor()
        {
            Color switchColor = m_isSwithOn ? m_onColor : m_offColor;
            m_renderer.material.SetColor(materialColorProperty, switchColor);
        }
    }
}
