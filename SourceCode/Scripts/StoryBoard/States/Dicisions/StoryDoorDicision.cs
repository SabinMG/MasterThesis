using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cgl.Thesis
{
    public class StoryDoorDicision : StoryDecision
    {
        public InteractableDoor.DoorState m_doorStateCondition;
        public InteractableDoor m_interactableDoor;

        public override void StartDecide()
        {
        }

        public override void EndDecide()
        {

        }

        public override bool Decide()
        {
           if(m_interactableDoor.m_currentDoorState == m_doorStateCondition)
            {
                m_status = true;
                return true;
            }

            return false;
        }

        public override void OnEnable()
        {
            base.OnEnable();       
        }

        public override void OnDisable()
        {
            base.OnDisable();
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
