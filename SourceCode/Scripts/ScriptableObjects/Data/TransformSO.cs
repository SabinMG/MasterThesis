using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cgl.Thesis.Data
{
    [CreateAssetMenu(fileName = "TransformSO", menuName = "SO/Data/TransformSO", order = 1)]
    public class TransformSO : ScriptableObject, ISerializationCallbackReceiver
    {
        public Transform m_value;

        [NonSerialized]
        public Transform m_runtimeValue;

        public Transform Value
        {
            get { return m_runtimeValue; }
            set { m_runtimeValue = value; }
        }

        public void OnAfterDeserialize()
        {
            m_runtimeValue = m_value;
        }

        public void OnBeforeSerialize()
        {
        }
    }
}
