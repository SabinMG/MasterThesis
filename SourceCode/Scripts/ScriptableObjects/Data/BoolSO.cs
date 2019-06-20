using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cgl.Thesis.Data
{
    [CreateAssetMenu(fileName = "BoolSO", menuName = "SO/Data/BoolSO", order = 1)]
    public class BoolSO : ScriptableObject, ISerializationCallbackReceiver
    {
        public bool m_value;

        [NonSerialized]
        public bool m_runtimeValue;
        public bool m_runtimeValueDisplay;

        public bool Value
        {
            get { return m_runtimeValue; }
            set { m_runtimeValue = value; m_runtimeValueDisplay = value; }
        }

        public void OnAfterDeserialize()
        {
            m_runtimeValue = m_value;
            m_runtimeValueDisplay = m_runtimeValue;
        }

        public void OnBeforeSerialize()
        {
        }
    }
}
