using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Cgl.Thesis.Data
{
    //[CreateAssetMenu(fileName = "BoolSO", menuName = "SO/Data/BoolSO", order = 1)]
    public class ConditionBaseSO : ScriptableObject
    {
        public bool invertOutput = false;
        public virtual bool IsSatisfied()
        {
            return !invertOutput;
        }
    }
}
