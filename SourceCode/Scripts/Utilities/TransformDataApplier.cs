using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cgl.Thesis.Data;

namespace Cgl.Thesis.Utilities
{
    public class TransformDataApplier : MonoBehaviour
    {
        public bool applyOnAwake = false;
        public TransformDataSO transformDataSO;

        public void Awake()
        {
            if (applyOnAwake)
            {
                ApplyTransfrom();
            }
        }

        public void ApplyTransfrom()
        {
            transform.position = transformDataSO.position;
            transform.eulerAngles = transformDataSO.rotation;
            transform.localScale = transformDataSO.scale;
        }
    }
}
