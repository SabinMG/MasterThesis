using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Cgl.Thesis
{
    // base class for all interactable object
    public class InteractableObject : MonoBehaviour
    {
        public int m_objectID = 0;
        public bool m_enableInteraction = true;

        public delegate void InteractableObjectDelegate(InteractableObject obj);
        public event InteractableObjectDelegate OnInteractionStartEvent;
        public event InteractableObjectDelegate OnInteractionEndEvent;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
