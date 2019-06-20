using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

namespace Cgl.Thesis
{
    public class PaperBoat : MonoBehaviour
    {
        public DOTweenPath m_moveToWaterPath;
        public DOTweenPath m_moveOverWaterPath;
        public Transform m_boatObject;
        public bool m_startShaking;
        public float m_ShakingAplitude = 1.0f;
        public float m_ShakingSpeed;

        private Vector3 m_moveOverWaterPathStartPosition;
        private float m_randomOffset;

        public void DoPlayMoveToWater(UnityAction onComplete)
        {
            m_moveToWaterPath.DOPlay();
            m_moveToWaterPath.onComplete.AddListener(onComplete);
        }
         
        public void DoPlayMoveOverWater()
        {
            m_moveOverWaterPath.transform.position = m_moveOverWaterPathStartPosition;
            m_boatObject.parent = m_moveOverWaterPath.transform;
            m_moveOverWaterPath.DOPlay();
            m_startShaking = true;
        }

        // Start is called before the first frame update
        void Start()
        {
            m_moveOverWaterPathStartPosition = m_moveOverWaterPath.transform.position;
            m_randomOffset = UnityEngine.Random.Range(0,50f);
        }

        // Update is called once per frame
        void Update()
        {
            if (m_startShaking)
            {
                Vector3 rotation = new Vector3(Mathf.Sin((Time.time - m_randomOffset)* m_ShakingSpeed) *m_ShakingAplitude, 0.0f, 0.0f );
                m_boatObject.eulerAngles = rotation;
            }
        }
    }
}
