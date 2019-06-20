using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Cgl.Thesis
{
    public class BalloonRopeJoint : MonoBehaviour
    {
        public Transform m_otheFollowtTransform;
        public float m_followSpeed = 1.0f;

        private Vector3 m_offsetVector;

        // Start is called before the first frame update
        void Start()
        {
            m_offsetVector = transform.position - m_otheFollowtTransform.transform.position;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, m_otheFollowtTransform.position + m_offsetVector, Time.deltaTime * m_followSpeed);
        }
    }
}
