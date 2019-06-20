using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cgl.Thesis
{
    public class WaterFall2D : MonoBehaviour
    {
        public float  m_scrollSpeedY = 0.5f;
        public float m_stopWaterFallSpeed = 1.0f;

        private Renderer m_renderer;
        private bool m_stopWaterFall; 
        private float m_UV2Offest = 0.0f;

        // Start is called before the first frame update
        void Start()
        {
            m_renderer = GetComponent<Renderer>();
        }

        // Update is called once per frame
        void Update()
        {

            float offsetY = 0.0f;
            offsetY = Time.time * m_scrollSpeedY % 1;
            m_renderer.material.mainTextureOffset = new Vector2(0, offsetY);

            if (m_stopWaterFall)
            {
                m_UV2Offest += Time.deltaTime * m_stopWaterFallSpeed;
                Vector2 uv2Offset = new Vector2(0, m_UV2Offest);
                m_renderer.material.SetTextureOffset("_UVTwo", uv2Offset);
            }
        }

        public void StopWaterFall()
        {
            m_stopWaterFall = true;
        }
    }
}



