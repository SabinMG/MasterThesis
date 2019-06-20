using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cgl.Thesis.Utilities
{
    public class BillboardTransform : MonoBehaviour
    {
        public Camera m_Camera;

        //Orient the camera after all movement is completed this frame to avoid jittering
        void LateUpdate()
        {
            if (m_Camera == null) m_Camera = Camera.main;
            transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
                m_Camera.transform.rotation * Vector3.up);
        }
    }
}
