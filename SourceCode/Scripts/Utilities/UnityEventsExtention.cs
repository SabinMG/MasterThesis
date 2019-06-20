using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Cgl.Thesis.Utilities
{
    [System.Serializable]
    public class UnityIntEvent : UnityEvent<int>
    {
    }

    [System.Serializable]
    public class UnityVector3Event : UnityEvent<Vector3>
    {
    }

    [System.Serializable]
    public class UnityGameObjectEvent : UnityEvent<GameObject>
    {
    }

    public class UnityEventsExtention : MonoBehaviour
    {
      
    }
}
