using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Cgl.Thesis
{
    public class StoryBoardBase : MonoBehaviour
    {
        public Mother m_mother;
        public PlayableDirector m_playableDirector;
        public ProjectorController m_projectorController;
        private InteractionController m_interactionController;

        public StoryStateBase m_currentStoryState;
        public int m_currentStoryStateIndex = 0;

        public virtual void Awake()
        {
            m_interactionController = InteractionController.Instance;
        }

        public virtual void Start()
        {
            m_projectorController.EnableProjector(false);
            m_currentStoryState.OnStateEnter();
        }

        public void MovetoNextStoryState()
        {
            m_currentStoryStateIndex++;
            //m_currentStoryState = m_storyStates[m_currentStoryStateIndex];
        }

        public void SwitchState(StoryStateBase state)
        {
            m_currentStoryState.OnStateExit();
            m_currentStoryState = state;
            m_currentStoryState.OnStateEnter();
        }

        public virtual void OnEnable()
        {
           
            m_interactionController.OnInteractionStartEvent -= OnInteractionStartEventCallback;
            m_interactionController.OnInteractionStopEvent -= OnInteractionStopEventCallback;
        }

        public virtual void OnDisable()
        {
            m_interactionController.OnInteractionStartEvent -= OnInteractionStartEventCallback;
            m_interactionController.OnInteractionStopEvent -= OnInteractionStopEventCallback;
        }
    

        public virtual void OnTriggerEnterEvent(Collider collider, StoryBoardEventTrigger trigger)
        {

        }

        public virtual void OnTriggerExitEvent(Collider collider, StoryBoardEventTrigger trigger)
        {

        }

        public virtual void OnInteractionStartEventCallback(InteractableObjectFBBIK interactableObject)
        {
            
        }

        public virtual void OnInteractionStopEventCallback(InteractableObjectFBBIK interactableObject)
        {
            
        }

       
    }
}
