using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cgl.Thesis
{
    public class StoryStateSwitcher : MonoBehaviour
    {
        public StoryBoardBase m_storyBoard;
        public StoryStateBase m_nextState;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SwitchState()
        {
            m_storyBoard.SwitchState(m_nextState);
        }
    }
}
