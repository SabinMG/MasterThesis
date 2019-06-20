using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cgl.Thesis
{
    public class StoryDraggableBoxDicision : StoryDecision
    {
        public InteractableBox m_interactableBox;
    
        private bool m_dicisionConditionSatisfied = false;

        public override void StartDecide()
        {
            m_dicisionConditionSatisfied = false;
            m_interactableBox.OnReachBoarder.AddListener(OnDraggableBoxReachedBoarder);
        }

        public override void EndDecide()
        {
            m_interactableBox.OnReachBoarder.RemoveListener(OnDraggableBoxReachedBoarder);
            m_dicisionConditionSatisfied = false;
        }

        private void OnDraggableBoxReachedBoarder()
        {
            m_status = true;
            m_dicisionConditionSatisfied = true;
        }

        public override bool Decide()
        {
            return m_status;
        }   
    }
}
