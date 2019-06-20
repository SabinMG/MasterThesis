using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Cgl.Thesis
{
    public class ProjectorController : MonoBehaviour
    {
        public float m_prjectorTurnSpeed = .5f;
        public Transform m_target;
        private Transform m_transform;
        private Projector m_projector;

        // Use this for initialization
        void Awake()
        {
            m_transform = transform;
            m_projector = GetComponent<Projector>();
        }



        // Update is called once per frame
        void Update()
        {
            m_transform.forward = Vector3.Lerp(m_transform.forward, -m_target.transform.right, m_prjectorTurnSpeed * Time.deltaTime);
        }

        public void EnableProjector(bool enable)
        {
            m_projector.enabled = enable;
        }
    }
}
