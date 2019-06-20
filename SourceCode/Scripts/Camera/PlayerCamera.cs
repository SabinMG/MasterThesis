using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform m_playerTransform;
    public float m_smoothSpeed;

    private Vector3 m_offset;
    private Transform m_transform;
    private Vector3 velocity = Vector3.zero;

    private float m_cameraInitialZPos;
   
    private void Awake()
    {
        m_transform = GetComponent<Transform>();
    }

    // Use this for initialization
    void Start ()
    {
        m_cameraInitialZPos = m_transform.position.z;
        m_offset = m_transform.position - m_playerTransform.transform.position;
    }
	
	// Update is called once per frame
	
    private void LateUpdate()
    {
        Vector3 targetPosition = m_playerTransform.position + m_offset;
      //  targetPosition.z = m_cameraInitialZPos;
        m_transform.position = targetPosition;// Vector3.Lerp(m_transform.position, targetPosition,Time.deltaTime* m_smoothSpeed);

        //m_transform.position  = Vector3.SmoothDamp(transform.position, targetPosition,
                                                //ref velocity, Time.deltaTime * m_smoothSpeed);
    }
}
