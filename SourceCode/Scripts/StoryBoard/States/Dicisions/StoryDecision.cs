using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cgl.Thesis
{
    public abstract class StoryDecision : MonoBehaviour
    {
        public bool m_status;
        public bool Status
        {
            get { return m_status; }
        }

        public abstract void StartDecide();
        public abstract void EndDecide();

        public abstract bool Decide(); // update to dicision

        public virtual void OnEnable()
        {

        }

        public virtual void OnDisable()
        {

        }
    }
}
