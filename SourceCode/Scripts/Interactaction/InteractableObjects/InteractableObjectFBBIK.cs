using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RootMotion.FinalIK;

namespace Cgl.Thesis
{
    // base class for all interactable object used Final ik
    [RequireComponent(typeof(InteractionObject))]
    public class InteractableObjectFBBIK : InteractableObject
    {
        public InteractionTypes m_interactionType;
       // public GameObject m_interactionIndicator;
        public Transform m_interactionIndicatorPosition;

        public bool m_unparentTriggers;
        public List<InteractableInRangeTrigger> m_interactionTriggers;
        public PlayerInteractionTypes m_playerInteractionType; // set it on editor
        public InteractionWrapModes m_InteractionWrapMode;
        public Transform m_InteractionTargetPivot;
        public float m_playerRootMotionSpeed;
      
        public float m_playerMinAproachDistance = .2f; 
        public bool m_isInterruptable = true; // player can interaupt the interactin any time
        public bool m_isInRange;

        protected Player m_player; // at the moment we are only looking for the player
        protected bool canInteractNow;
        protected InteractionObject m_finalIKInteractionObject;
        protected InteractableInRangeTrigger m_currentInteractionTrigger;
        protected InteractionSystem m_InteractionSystem;
        protected bool m_canInteract;
        protected Transform m_transform;
        protected bool m_isInInteraction;

        public UnityEvent m_onStartInteractionEvent;
        public UnityEvent m_onEndInteractionEvent;

        public delegate void InteractableObjectFBBIKDelegate(InteractableObjectFBBIK obj);
        public event InteractableObjectFBBIKDelegate OnInteractableObjectInRangeEvent;
        public event InteractableObjectFBBIKDelegate OnInteractableObjectOutRangeEvent;

        public delegate void InteractableObjectIndicatorDelegate(InteractableObjectFBBIK obj, Vector3 inidcatorPosition);
        public event InteractableObjectIndicatorDelegate OnInteractionIndicatorShowEvent;
        public event InteractableObjectIndicatorDelegate OnInteractionIdicatorHideEvent;

        public delegate void InteractableActionDelegate(InteractableObjectFBBIK obj);

        public int ObjectID
        {
            get
            {
                return m_objectID;
            }
        }

        public InteractionTypes InteractionType
        {
            get
            {
                return m_interactionType;
            }
        }

        public PlayerInteractionTypes PlayerInteractionType
        {
            get
            {
                return m_playerInteractionType;
            }
        }

        public InteractableInRangeTrigger CurrentInteractionTrigger
        {
            get
            {
                return m_currentInteractionTrigger;
            }
        }

        public InteractionWrapModes InteractionWrapMode
        {
            get
            {
                return m_InteractionWrapMode;
            }
        }

        public float PlayerMinAproachDistance
        {
            get
            {
                return m_playerMinAproachDistance;
            }
        }

        //public bool EnableInteraction
        //{
        //    get
        //    {
        //        return m_enableInteraction;
        //    }
        //    set
        //    {
        //        m_enableInteraction = value;
        //    }
        //}

        protected virtual void Awake()
        {
            if (m_interactionTriggers.Count == 0)
            {
                Debug.LogError("please assign interactionTriggers" + name);
                return;
            }

            m_player = Player.Instance;
            m_transform = transform;
           // m_interactionIndicator.SetActive(false);
            m_finalIKInteractionObject = GetComponent<InteractionObject>();

            if (m_unparentTriggers)
            {
                for (int i = 0; i < m_interactionTriggers.Count; i++)
                {
                    m_interactionTriggers[i].transform.parent = null;
                }
            }
        }

        private void OnEnable()
        {
            for (int i = 0; i < m_interactionTriggers.Count; i++)
            {
                m_interactionTriggers[i].OnTriggerEnterEvent += OnTriggerEnterCallback;
                m_interactionTriggers[i].OnTriggerExitEvent += OnTriggerExitCallback;
            }

            InteractionController.Instance.RegistertheInteractableObject(this);
        }

        private void OnDisable()
        {
            for (int i = 0; i < m_interactionTriggers.Count; i++)
            {
                m_interactionTriggers[i].OnTriggerEnterEvent -= OnTriggerEnterCallback;
                m_interactionTriggers[i].OnTriggerExitEvent -= OnTriggerExitCallback;
            }
            InteractionController.Instance.UnRegistertheInteractableObject(this);
        }

        // Use this for initialization
        protected virtual void Start()
        {
           
        }

        // Update is called once per frame
        public virtual void Update() // update only called on base object
        {
            if (!m_enableInteraction)
            {
               // m_interactionIndicator.SetActive(false);
                return;
            }

            if (m_isInRange)
            {
                bool canInteract = CheckForInteraction();
                //Debug.Log(canInteract);

                if (m_canInteract != canInteract)
                {
                    m_canInteract = canInteract;
                    if (m_currentInteractionTrigger != null && !canInteract)
                    {
                        if (m_isInInteraction) EndInteraction();
                    }

                    if (canInteract) OnInteractableObjectInRangeEvent(this);
                    else OnInteractableObjectOutRangeEvent(this);
                }
                // m_interactionIndicator.SetActive(canInteract);
            }
            else
            {

            }
            // m_interactionIndicator.SetActive(false);
        }

        public void EnableInteraction(bool value)
        {
            m_enableInteraction = value;
        }

        public virtual bool CheckForInteraction()
        {
            return false;
        }

        public virtual bool IsInRange()
        {
            return m_isInRange;
        }

        public virtual void StartInteraction(InteractionSystem interactionSystem)
        {
            m_isInInteraction = true;
            m_player.OnPlayerPositionUpdate += OnPlayerPositionUpdateCallback;
        }

        public virtual void EndInteraction()
        {
            m_isInInteraction = false;
            m_player.OnPlayerPositionUpdate -= OnPlayerPositionUpdateCallback;
        }

        public virtual void OnPlayerPositionUpdateCallback(Vector3 position)
        {

        }

        public virtual void ShowInteractionIndicator(Vector3 pos)
        {
            OnInteractionIndicatorShowEvent(this, pos);
        }

        public virtual void HideIntercationIndicator(Vector3 pos)
        {
            OnInteractionIdicatorHideEvent(this, pos);
        }

        protected virtual void OnTriggerEnterCallback(Collider other, InteractableInRangeTrigger trigger)
        {
            if (other.gameObject.tag == m_player.tag)
            {
                m_currentInteractionTrigger = trigger;
               // Debug.Log(" player entered");
                m_isInRange = true;
            }
        }

        protected virtual void OnTriggerExitCallback(Collider other, InteractableInRangeTrigger trigger)
        {
            if (other.gameObject.tag == m_player.tag)
            {
                m_isInRange = false;
                m_currentInteractionTrigger = null;
            }
        }

        protected virtual void OnInteractableObjectInRange()
        {
           // m_canInteract = false;
            //m_isInRange = false;
            //m_currentInteractionTrigger = null;
            //OnInteractableObjectInRangeEvent(this);
        }

        protected virtual void OnInteractableObjectOutRange()
        {
           // m_canInteract = false;
            m_isInRange = false;
            m_currentInteractionTrigger = null;
            OnInteractableObjectOutRangeEvent(this);
        }
    }
}







