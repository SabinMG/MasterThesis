using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cgl.Thesis
{ 
    public class SplashScreen : MonoBehaviour
    {
        public CanvasGroup m_splashCanvasGroup;

        public float Alpha
        {
            get { return m_splashCanvasGroup.alpha;}
            set { m_splashCanvasGroup.alpha = value; }
        }

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
