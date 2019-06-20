using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Cgl.Thesis
{
    public class StoryPathDicision : StoryDecision
    {
        public enum PathEventType
        {
            Oncomplete,
            OnStart
        }

        public PathEventType m_pathEventType;
        public DOTweenPath m_cureentPath;

        private bool m_dicisionConditionSatisfied = false;

        public override void StartDecide()
        {
            if (m_pathEventType == PathEventType.Oncomplete)
            {
                m_cureentPath.onComplete.AddListener(OnCompletePath);
            }
            else if (m_pathEventType == PathEventType.OnStart)
            {
                m_cureentPath.onStart.AddListener(OnStartPath);
            }
        }

        public override void EndDecide()
        {
            if (m_pathEventType == PathEventType.Oncomplete)
            {
                m_cureentPath.onComplete.RemoveListener(OnCompletePath);
            }
            else if (m_pathEventType == PathEventType.OnStart)
            {
                m_cureentPath.onStart.AddListener(OnStartPath);
            }
            //m_cureentPath = null;
            m_status = false;
        }

        void OnCompletePath()
        {
            m_status = true; 
        }

        void OnStartPath()
        {
            m_status = true;
        }

        public override bool Decide()
        {
            return m_status;
        }

        public override void OnEnable()
        {
            base.OnEnable();
        }

        public override void OnDisable()
        {
            base.OnDisable();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}




