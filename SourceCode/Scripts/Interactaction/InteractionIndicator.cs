using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cgl.Thesis.Utilities;

namespace Cgl.Thesis
{
    public class InteractionIndicator : BillboardTransform
    {
        public float m_animationSpeed = 1.0f;
        public float m_scaleFactor = .1f;

        private bool m_showInRangeIndication;
        private bool m_showInteractionIndication;
        private Material m_mat;
        private MeshRenderer m_meshRenderer;
        private Vector3 m_initialScale;
        
        private void Awake()
        {
            m_meshRenderer = GetComponent<MeshRenderer>();
            m_mat = m_meshRenderer.material;
            ShowInteractionIndicator(false);
            m_initialScale = transform.localScale;
        }
        // Start is called before the first frame update
        void Start()
        {   

        }

        // Update is called once per frame
        void Update()
        {
            if (m_showInRangeIndication)
            {
                float amplitude = Mathf.PingPong(Time.time* m_animationSpeed, 1 * m_scaleFactor);
                transform.localScale = new Vector3(m_initialScale.x+amplitude, m_initialScale.y+ amplitude, m_initialScale.z+ amplitude);
            }
        }

        public void ShowInteractionIndicator(bool show, Vector3? position = null)
        {
            m_meshRenderer.enabled = show;
            m_showInRangeIndication = show;
            if (position != null)
                transform.position = (Vector3)position;
        }
    }
}
