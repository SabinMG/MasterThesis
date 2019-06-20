using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cgl.Thesis.Data;
using UnityEngine.Events;

namespace Cgl.Thesis
{
    public class ConditionChecker : MonoBehaviour
    {
        public bool m_enableCheck = true;

        public bool m_checkOnStart;
        public bool m_checkOnUpdate;

        public bool m_stopWhenSatisfied;
        public ConditionBaseSO[] m_conditions;
        public UnityEvent m_onConditionSatisfied;

        private bool m_isRunningCheck;


        // Start is called before the first frame update
        void Start()
        {
            m_isRunningCheck = m_checkOnUpdate;
            if (m_checkOnStart && m_enableCheck)
            {
                Check();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (m_isRunningCheck && m_enableCheck)
            {
                Check();
            }
        }

        public void EnableChecking(bool enable)
        {
            m_enableCheck = enable;
        }

        public void Check()
        {
            bool satisfied = false;
            for (int i = 0; i < m_conditions.Length; i++)
            {
                satisfied = m_conditions[i].IsSatisfied();
                if (!satisfied) break;
            }

            if (satisfied)
            {
                Debug.Log("satis fied the condition"+ name);
                if (m_onConditionSatisfied != null) m_onConditionSatisfied.Invoke();
                m_isRunningCheck = false;
            }
        }
    }
}
