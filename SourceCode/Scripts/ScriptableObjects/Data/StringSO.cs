using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cgl.Thesis.Data
{
    [CreateAssetMenu(fileName = "StringSO", menuName = "SO/Data/StringSO", order = 1)]
    public class StringSO : ScriptableObject
    {
        public string m_value;
        public string Value
        {
            get { return m_value; }
            set { m_value = value; }
        }
    }
}
