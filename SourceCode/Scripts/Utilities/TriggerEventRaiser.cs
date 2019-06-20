using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cgl.Thesis.Utilities;

namespace Cgl.Thesis
{
    public class TriggerEventRaiser : MonoBehaviour
    {
        public UnityGameObjectEvent OnTriggerEnterEvent;
        public UnityGameObjectEvent OnTriggerEnterExit;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        protected void OnTriggerEnter(Collider other)
        {
            if (OnTriggerEnterEvent != null) OnTriggerEnterEvent.Invoke(other.gameObject);
        }

        protected void OnTriggerExit(Collider other)
        {
            if (OnTriggerEnterExit != null) OnTriggerEnterExit.Invoke(other.gameObject);
        }
    }
}
