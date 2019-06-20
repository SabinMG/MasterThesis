using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

namespace Cgl.Thesis
{
    public class InteractionController : Singleton<InteractionController>
    {
        public InteractionSystem m_interactionSystem;
        public InteractableObjectFBBIK m_currentInteractableObject; // moment we are expecting only one interactable object
        public InteractionIndicator m_interactionIndicator;

        private Dictionary<InteractableObjectFBBIK, InteractableObjectFBBIK> m_interactableObjectsDict;

        public delegate void InteractionDelegate(InteractableObjectFBBIK obj);
        public event InteractionDelegate OnInteractionStartEvent;
        public event InteractionDelegate OnInteractionStopEvent;

        public event InteractionDelegate OnInteractableObjectInRangeEvent;
        public event InteractionDelegate OnInteractableObjectOutRangeEvent;

        //public event InteractionDelegate OnCanInteractionTrueEvent; // when everything is aligned with interaction
        //public event InteractionDelegate OnCanInteractionFalseEvent;

        public event InteractionDelegate OnInteractionIndicatorShowEvent;
        public event InteractionDelegate OnInteractionIdicatorHideEvent;

        private bool m_isInteracting;
        public bool IsInteracting
        {
            get
            {
                return m_isInteracting;
            }
        }
   
        public override void Awake()
        {
            base.Awake(); // must call
            m_interactableObjectsDict = new Dictionary<InteractableObjectFBBIK, InteractableObjectFBBIK>();
        }

        public void Start()
        {
           
        }

        public void OnEnable()
        {
            m_interactionSystem.OnInteractionStart = OnInteractionStart;
            m_interactionSystem.OnInteractionStop = OnInteractionStop;
           // m_interactionSystem.OnInteractionEvent =
        }

        public void OnDisbale()
        {
           
        }

        // Update is called once per frame
        void Update()
        {
           

        }

        public void HideInteractionIndicator()
        {
            m_interactionIndicator.ShowInteractionIndicator(false);
        }

        public void ShowInteractionIndicator(Vector3 position)
        {
            m_interactionIndicator.ShowInteractionIndicator(true, position);
        }

        public void ShowInteractionIndicator(Transform positionTrans)
        {
            m_interactionIndicator.ShowInteractionIndicator(true, positionTrans.position);
        }

        public void OnInteractionStart(FullBodyBipedEffector effectorType, InteractionObject interactionObject)
        {
            // m_interactionIndicator.ShowInteractionIndicator(false);
             m_isInteracting = true;
            if (m_currentInteractableObject != null)
            {
                OnInteractionStartEvent(m_currentInteractableObject);
            }      
        }

        public void OnInteractionStop(FullBodyBipedEffector effectorType, InteractionObject interactionObject)
        {
            Debug.Log("intrecation end");
            m_isInteracting = false;
            if (m_currentInteractableObject != null)
            {
                OnInteractionStopEvent(m_currentInteractableObject);
            }
        }

        public void RegistertheInteractableObject(InteractableObjectFBBIK interactableObject)
        {
            m_interactableObjectsDict.Add(interactableObject, interactableObject);
            interactableObject.OnInteractableObjectInRangeEvent += OnInteractableObjectInRangeEventCallback;
            interactableObject.OnInteractableObjectOutRangeEvent += OnInteractableObjectOutRangeEventCallback;

            interactableObject.OnInteractionIndicatorShowEvent += OnInteractionIndicatorShowEventEventCallback;
            interactableObject.OnInteractionIdicatorHideEvent += OnInteractionIdicatorHideEventCallback;
        }

        public void UnRegistertheInteractableObject(InteractableObjectFBBIK interactableObject)
        {
            m_interactableObjectsDict.Remove(interactableObject);
            interactableObject.OnInteractableObjectInRangeEvent -= OnInteractableObjectInRangeEventCallback;
            interactableObject.OnInteractableObjectOutRangeEvent -= OnInteractableObjectOutRangeEventCallback;

           interactableObject.OnInteractionIndicatorShowEvent -= OnInteractionIndicatorShowEventEventCallback;
           interactableObject.OnInteractionIdicatorHideEvent -= OnInteractionIdicatorHideEventCallback;
        }

        public void StartIntercation()
        {
            if (m_currentInteractableObject != null)
            {
                m_isInteracting = true;
                m_currentInteractableObject.StartInteraction(m_interactionSystem);
            }
        }

        public void EndInteraction()
        {
            if (m_currentInteractableObject != null)
            {
                m_isInteracting = false;
                if(m_currentInteractableObject.InteractionWrapMode == InteractionWrapModes.Loop) m_currentInteractableObject.EndInteraction();
            }
        }

        private void OnInteractableObjectInRangeEventCallback(InteractableObjectFBBIK interactableObject)
        {
            //m_interactionIndicator.ShowInteractionIcon(false, interactableObject.m_interactionIndicatorPosition.position);
            m_currentInteractableObject = interactableObject;
            if (OnInteractableObjectInRangeEvent != null) OnInteractableObjectInRangeEvent(interactableObject);
        }

        private void OnInteractableObjectOutRangeEventCallback(InteractableObjectFBBIK interactableObject)
        {
           // m_interactionIndicator.ShowInteractionIcon(false, null);
            m_currentInteractableObject = null;
            if (OnInteractableObjectOutRangeEvent != null) OnInteractableObjectOutRangeEvent(interactableObject);
        }

        private void OnInteractionIndicatorShowEventEventCallback(InteractableObjectFBBIK interactableObject, Vector3 pos)
        {
            m_interactionIndicator.ShowInteractionIndicator(true, pos);
        }

        private void OnInteractionIdicatorHideEventCallback(InteractableObjectFBBIK interactableObject, Vector3 pos)
        {
            m_interactionIndicator.ShowInteractionIndicator(false);
        }
    }
}
