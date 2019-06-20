using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cgl.Thesis.Data
{
    [CreateAssetMenu(fileName = "BoolConditionSO", menuName = "SO/Conditions/BoolConditionSO", order = 1)]
    public class BoolConditionSO : ConditionBaseSO
    {
        public BoolSO m_boolSO;
        public override bool IsSatisfied()
        {
           if(m_boolSO.m_runtimeValue == !invertOutput) return true;
            return false;
        }
    }
}