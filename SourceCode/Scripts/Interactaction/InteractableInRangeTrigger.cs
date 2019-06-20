using UnityEngine;

namespace Cgl.Thesis
{
    public enum InteractionSideEnum
    {
        Front = 0,
        Left = 1,
        Right = 2,
        Up = 3,
        Down = 4,
        Back = 5
    }

    [RequireComponent(typeof(Collider))]
    public class InteractableInRangeTrigger : MonoBehaviour
    {
        // public string m_triggerObjectTag = "Player";
        public float m_customForwardAngle;
        public int m_triggerID;
        private Collider m_interactionTriggerCollider;

        public delegate void TriggerDelegate( Collider collider, InteractableInRangeTrigger trigger);
        public TriggerDelegate OnTriggerEnterEvent;
        public TriggerDelegate OnTriggerExitEvent;

        private Vector3 m_interactionForward;
        [SerializeField] private InteractionSideEnum m_interactionSide; // lies in which side of the interactble object 

        public int TriggerID
        {
            get { return m_triggerID; }
        }

        public Vector3 InteractionForward
        {
            get { return m_interactionForward.normalized; }
        }

        public InteractionSideEnum InteractionSide
        {
            get { return m_interactionSide; }
        }

        public Collider InteractionTriggerCollider
        {
            get { return m_interactionTriggerCollider; }
        }

        void Awake()
        {
            m_interactionTriggerCollider = GetComponent<Collider>();
            m_interactionTriggerCollider.isTrigger = true;
        }
        // Use this for initialization
        void Start()
        {
            m_interactionForward = Quaternion.AngleAxis(-m_customForwardAngle, transform.up) * transform.forward;
        }

        // Update is called once per frame
        void Update()
        {
          
        }

        void OnDrawGizmosSelected()
        {
            // Draws a 5 unit long red line in front of the object
            Gizmos.color = Color.red;
            m_interactionForward = Quaternion.AngleAxis(-m_customForwardAngle, transform.up) * transform.forward;
            Gizmos.DrawRay(transform.position, m_interactionForward);
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (OnTriggerEnterEvent != null) OnTriggerEnterEvent(other,this);
        }

        protected void OnTriggerExit(Collider other)
        {
            if (OnTriggerExitEvent != null) OnTriggerExitEvent(other, this);
        }
    }
}
  
