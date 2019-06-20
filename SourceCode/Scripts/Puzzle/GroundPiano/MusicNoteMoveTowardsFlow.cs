using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cgl.Thesis
{
    public class MusicNoteMoveTowardsFlow : MonoBehaviour
    {
        public MoveTowardsFlowPoint m_currentMovePoint;
        public float m_speed = 1.0f;
        public bool m_updateScale = false;

        private Vector3 m_lastPosition;
        private float m_xScale = 1.0f;
        private float m_totalDistance = 1.0f;

        private bool m_isMoving = false;
        private Vector3 m_initialScale;
        // private float m_yScale = 1.0f;

        // Start is called before the first frame update
        void Start()
        {
            m_lastPosition = transform.position;
            m_xScale = Random.Range(0.3f, 1.0f);
            m_speed = 35.0f;
            m_initialScale = transform.localScale;
        }
    
        public void StartMoving(bool updateScale)
        {
            m_lastPosition = transform.position;
            m_updateScale = updateScale;
            m_totalDistance = Vector3.Distance(transform.position, m_currentMovePoint.transform.position);
            m_isMoving = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (m_isMoving)
            {
                float distance = Vector3.Distance(transform.position, m_currentMovePoint.transform.position);

                float movePercentage = distance / m_totalDistance;
                m_lastPosition = Vector3.Lerp(m_lastPosition, m_currentMovePoint.transform.position, Time.deltaTime * (distance / m_speed));

                float xValue = Mathf.PerlinNoise(Time.time * m_xScale, 0.0f);
                Vector3 noisiVector = new Vector3(xValue, 0, 0);
                transform.position = m_lastPosition + noisiVector;
                transform.localScale = m_initialScale * movePercentage;

                if (distance < 2.0f)
                {
                    if (m_currentMovePoint.m_nextPoint != null)
                    {
                        m_currentMovePoint = m_currentMovePoint.m_nextPoint;
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
            }      
        }
    }
}

//https://gamedev.stackexchange.com/questions/131143/how-to-move-a-object-to-a-target-point-like-sine-wave-in-2d-world
