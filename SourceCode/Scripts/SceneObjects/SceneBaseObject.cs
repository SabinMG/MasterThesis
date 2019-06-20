using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cgl.Thesis
{
    public class SceneBaseObject : MonoBehaviour
    {
        protected Transform m_transform;
        public virtual void Awake()
        {
            m_transform = transform;

        }

        // Use this for initialization
        public virtual void Start()
        {

        }

        // Update is called once per frame
        public virtual void Update()
        {

        }

    }
}
