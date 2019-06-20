using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RootMotion.FinalIK;
using DG.Tweening;

namespace Cgl.Thesis
{
    // Note : its not a generic class 
    public class PaperBoatSceneController : MonoBehaviour
    {
        public float m_startInteractionIndicatorTime = 3.0f;
        public float m_firstBoatMoveTime = 3.0f;
        public float m_secondBoatMoveTime = 7.0f;
        public float m_thirdBoatMoveTime = 11.0f;

        public UnityEvent m_showInteractionIndicator;
        public UnityEvent m_hideInteractionIndicator;

        [Header("--- FullBodyBipedIK IK ---")]
        public FullBodyBipedIK m_fBBIK;
        public PaperBoat[] m_paperBoatObjects;
        private int m_currentBoatIndex;

        private PaperBoat m_currentBoatObject;

        // Start is called before the first frame update
        void Start()
        {
            Invoke("ShowInteractionIndicator", m_startInteractionIndicatorTime);
            Invoke("MoveBoatIntheWater", m_firstBoatMoveTime);
            Invoke("MoveBoatIntheWater", m_secondBoatMoveTime);
            Invoke("MoveBoatIntheWater", m_thirdBoatMoveTime);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void ShowInteractionIndicator()
        {
            if (m_showInteractionIndicator != null)
            {
                m_showInteractionIndicator.Invoke();
            }
        }

        private void MoveBoatIntheWater()
        {
           
            if (m_hideInteractionIndicator != null)
            {
                m_hideInteractionIndicator.Invoke();
            }

            m_currentBoatObject = m_paperBoatObjects[m_currentBoatIndex];
            float myFloat = 0.0f;
            SetLeftArmEffector(m_currentBoatObject.transform);

            DOTween.To(() => myFloat, x => myFloat = x, 1.0f, 1.0f).OnUpdate(() => { SetLeftArmEffectorWeight(myFloat); }).OnComplete(() => { m_currentBoatObject.DoPlayMoveToWater(StartMovingOnWater); }).SetOptions(false);
            m_currentBoatIndex++;
        }

        private void SetLeftArmEffector( Transform rightArmEffector = null)
        {
            m_fBBIK.solver.leftHandEffector.target = rightArmEffector;
        }

        private void SetLeftArmEffectorWeight(float weight)
        {
            m_fBBIK.solver.leftHandEffector.positionWeight = weight;
        }

        private void StartMovingOnWater()
        {
            SetLeftArmEffecterWeigthToZero();


            m_currentBoatObject.DoPlayMoveOverWater();
        }

        private void SetLeftArmEffecterWeigthToZero()
        {
            float myFloat = 1.0f;
            DOTween.To(() => myFloat, x => myFloat = x, 0.0f, 1.0f).OnUpdate(() => { SetLeftArmEffectorWeight(myFloat); }).OnComplete(() => {}).SetOptions(false);
        }
    }
}
