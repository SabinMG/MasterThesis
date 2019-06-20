using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cgl.Thesis.Data;


namespace Cgl.Thesis
{
    public class Mother : Singleton<Mother>
    {
        public DOTweenPath m_currentPath;
        public bool m_startMovingOnStart;

        private Transform m_transform;
        private Animator m_animator;

        private InteractionController m_playerInteractionController;
        private InteractableObjectFBBIK m_CurrentDraggableBox;
        private bool m_isInfrontOfDraggabelBoxNow;

        public DOTweenPath CureentPath
        {
            get{ return m_currentPath; }
        }

        public override void Awake()
        {
            base.Awake();
            m_playerInteractionController = InteractionController.Instance;
            m_animator = GetComponent<Animator>();
        }

        // Use this for initialization
        void Start()
        {
            m_transform = transform;

            if (m_startMovingOnStart)
            {
                StartMovingOnPath(m_currentPath);
            }
        }

        // Update is called once per frame
        void Update()
        {
            //if (m_isInfrontOfDraggabelBoxNow)
            //{
            //    if (m_CurrentDraggableBox != null)
            //    {
            //        Vector3 position = m_transform.position;
            //        position.x = m_CurrentDraggableBox.transform.position.x;
            //        m_transform.position = position;
            //        //bool infrontOFbox = CheckObjectIsInfrontOfIT(m_CurrentDraggableBox.transform);
            //    }

            //}
            //else
            //{
            //   
            //}   
        }

        private void OnPathUpdate()
        {
            m_transform.position = m_currentPath.transform.position;
            m_transform.rotation = m_currentPath.transform.rotation;
        }

        private void OnEnable()
        {
            //m_playerInteractionController.OnInteractionStartEvent += OnInteractionStartEventCallback;
           // m_playerInteractionController.OnInteractionStopEvent += OnInteractionStopEventCallback;
        }

        private void OnDisable()
        {
            //m_playerInteractionController.OnInteractionStartEvent -= OnInteractionStartEventCallback;
           // m_playerInteractionController.OnInteractionStopEvent -= OnInteractionStopEventCallback;
        }

        private void OnInteractionStartEventCallback(InteractableObjectFBBIK interactableObject)
        {
            switch (interactableObject.InteractionType)
            {
                case InteractionTypes.DraggableBox:
                    m_CurrentDraggableBox = interactableObject;
                    break;
                default:
                    break;
            }
        }

        private void OnInteractionStopEventCallback(InteractableObjectFBBIK interactableObject)
        {
            switch (interactableObject.InteractionType)
            {
                case InteractionTypes.DraggableBox:
                    m_CurrentDraggableBox = null;
                    break;

                default:
                    break;
            }
        }

        public void SetInfrontOftheDraggableBox(bool isInfront)
        {
            m_isInfrontOfDraggabelBoxNow = isInfront;
        }


        //-------------------------- version 2.0---------------
        public void SetNewPath(DOTweenPath path)
        {
            m_currentPath = path;
            m_currentPath.onUpdate.AddListener(OnPathUpdate);
        }

        public void StartMovingOnCurrentPath()
        {
            if (m_currentPath != null)
            {
                StartMovingOnPath();
            }
        }

        public void StartMovingOnPath(DOTweenPath path)
        {
            SetNewPath(path);
            StartMovingOnPath();
        }

        public void PauseMovingOnPath(bool playIdle)
        {
            if (m_currentPath != null)
            {
                m_currentPath.DOPause();
                if (playIdle) SetSteMachineToIdle();
            }    
        }

        private void StartMovingOnPath()
        {
            SetSteMachineToWalkState();
            m_currentPath.DOPlay();  
        }

        public void EndMoveOnPath()
        {
            m_currentPath.DOComplete();
        }

        public void WalkTowards(Vector3 position, float duration, bool setToIdleOnend, Action OnComplete)
        {
            transform.DOMove(position, duration).OnComplete(() => { if (setToIdleOnend) SetSteMachineToIdle(); OnComplete(); });
            SetSteMachineToWalkState();
        }

        public void SetToPosition(Transform positionTrans)
        {
            transform.position = positionTrans.position;
        }

        public void PlayCryingAnimation()
        {
            SetSteMachineToCryingState();
        }

        public void PlayHappyAnimation()
        {
            SetSteMachineToHappyState();
        }

        public void PlayPointingAnimation()
        {
            SetSteMachineToPicturePointingState();
        }

        public void PlayIdleAnimation()
        {
            SetSteMachineToIdle();
        }

        private void SetSteMachineToIdle()
        {
            m_animator.SetTrigger("IdleStateTrigger");
            m_animator.SetBool("IdleState", true);
        }

        private void SetSteMachineToWalkState()
        {
            m_animator.SetTrigger("WalkStateTrigger");
            m_animator.SetBool("WalkState",true);
        }

        private void SetSteMachineToCryingState()
        {
            m_animator.SetTrigger("CryingState");
        }

        private void SetSteMachineToPicturePointingState()
        {
            m_animator.SetTrigger("PicturePointingStateTrigger");
            m_animator.SetBool("PicturePointingState", true);
        }

        private void SetSteMachineToHappyState()
        {
            m_animator.SetTrigger("HappyStateTrigger");
            m_animator.SetBool("HappyState", true);
        }
   
    }
}
