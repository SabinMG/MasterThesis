using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cgl.Thesis.Utilities;
using Cgl.Thesis.Data;

namespace Cgl.Thesis
{
    public class EditorSceneSetter : MonoBehaviour
    {
        public Transform m_cameraTransform;
        public Transform m_playerTransfrom;
        public Transform m_motherTranform;

        public TransformDataSO m_cameraTransformData;
        public TransformDataSO m_playerTransfromData;
        public TransformDataSO m_motherTranformData;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void CopyTransformDataToSO()
        {
            m_cameraTransformData.position = m_cameraTransform.position;
            m_playerTransfromData.position = m_playerTransfrom.position;
            m_motherTranformData.position = m_motherTranform.position;
        }

        public void ApplyTransformData()
        {
            m_cameraTransform.position = m_cameraTransformData.position;
            m_playerTransfrom.position = m_playerTransfromData.position;
            m_motherTranform.position = m_motherTranformData.position;
        }
    }
}
