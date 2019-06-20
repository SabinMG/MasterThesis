using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cgl.Thesis
{
    public class MathUtilities : MonoBehaviour
    {

        public static bool V3Equal(Vector3 a, Vector3 b)
        {
            return Vector3.SqrMagnitude(a - b) < 0.0001;
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

