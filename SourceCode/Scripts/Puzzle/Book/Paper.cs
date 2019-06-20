using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


namespace Cgl.Thesis
{
    public class Paper : MonoBehaviour
    {
        public DOTweenPath m_paperFlyPath;
        public Vector3 m_paperTunrnRotation;
        public float m_paperTunrnSpeed = 2.0f;
        private Renderer m_enderer;

        // Start is called before the first frame update
        void Start()
        {
            m_enderer = GetComponent<Renderer>();
        }

        public void StartFlying(float delay)
        {
            Invoke("StartFlying",delay);
        }

        public void StartFlying()
        {
            if (m_paperFlyPath != null)
            {
                m_paperFlyPath.DOPlay();

                Vector4 waveA = new Vector4(1,1,.03f,1);
                Vector4 waveB = new Vector4(1, 1, .03f, 1);
                Vector4 waveC = new Vector4(1, 1, .03f, 1);

                m_enderer.material.SetVector("_WaveA", waveA);
                m_enderer.material.SetVector("_WaveB", waveB);
                m_enderer.material.SetVector("_WaveB", waveC);
            }
           
        }

        public void TurnPaper()
        {
            transform.DOLocalRotate(m_paperTunrnRotation, m_paperTunrnSpeed).OnComplete(() =>
            {

            });
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
