using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cgl.Thesis.Data
{
    [CreateAssetMenu(fileName = "TransformSO", menuName = "SO/Data/TransformSO", order = 1)]
    public class TransformDataSO : ScriptableObject
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale = new Vector3(1,1,1);
    }
}
