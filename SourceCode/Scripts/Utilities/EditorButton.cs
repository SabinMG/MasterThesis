using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Cgl.Thesis.Utilities
{
    public class EditorButton : MonoBehaviour
    {
        public UnityEvent buttonAction;
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void Click()
        {
            if (buttonAction != null)
            {
                buttonAction.Invoke();
            }
        }
    }
}
