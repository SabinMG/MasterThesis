using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

namespace Cgl.Thesis
{
    public class MotherPathSwitcher : MonoBehaviour
    {
        public bool m_swichPathOnStart;
        public DOTweenPath m_nextPath;
        public UnityEvent m_onPathSwithEvent; // not generic
        private Mother m_mother;

        // Use this for initialization
        void Start()
        {
            m_mother = Mother.Instance;
            if (m_swichPathOnStart)
            {
                SwitchPath();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SwitchPath()
        {
             m_mother.SetNewPath(m_nextPath);
             if (m_onPathSwithEvent != null) m_onPathSwithEvent.Invoke();
        }
    }
}
