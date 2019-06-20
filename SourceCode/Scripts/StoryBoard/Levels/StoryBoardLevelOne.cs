using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cgl.Thesis
{
    public class StoryBoardLevelOne : StoryBoardBase
    {
      
        public override void Awake()
        {
            base.Awake();
        }

        public override void Start()
        {
            base.Start();
        }

        public void Update()
        {
            m_currentStoryState.OnStateUpdate();
        }

        public void StartEndOnPath()
        {
          
          //  m_mother.DoMoveOnPath();
        }

        public void StartMoveOnPath()
        {
            m_projectorController.EnableProjector(true);
            //m_mother.DoMoveOnPath();
        }

        public void StartShowSadAnimation()
        {
            //m_mother.PauseOnPath(false);
            m_mother.PlayCryingAnimation();
        }

        public void StartPicturPointingAnimation()
        {
           // m_mother.PauseOnPath(false);
            m_mother.PlayPointingAnimation();
        }

        public void StartShowHappyAnimation()
        {
            m_mother.PlayHappyAnimation();
        }

        public void StartShowIdleAnimation()
        {
            m_mother.PlayIdleAnimation();
        }

        public void StartPauseOnPath()
        {
         //   m_mother.PauseOnPath(true);
           // m_mother.
        }

        public void StartMoveAlongWithBox(bool state)
        {
            m_mother.SetInfrontOftheDraggableBox(state);
        }

    }
}

//// Update is called once per frame
//void Update()
//{

//}

//public void OnPlayerOpenFirstDooor()
//{

//}

//// story opening the first door

//public override void OnEnable()
//{
//    base.OnEnable();

//    for (int i = 0; i < m_interactableFirstPictureFrames.Length; i++)
//    {
//        m_interactableFirstPictureFrames[i].OnShowPictureFrameStartEvent += OnShowPictureFrameStartEventCallback;
//        m_interactableFirstPictureFrames[i].OnShowPictureFrameEndEvent += OnShowPictureFrameEndEventCallback;
//    }
//}

//public override void OnDisable()
//{
//    base.OnEnable();

//    for (int i = 0; i < m_interactableFirstPictureFrames.Length; i++)
//    {
//        m_interactableFirstPictureFrames[i].OnShowPictureFrameStartEvent -= OnShowPictureFrameStartEventCallback;
//        m_interactableFirstPictureFrames[i].OnShowPictureFrameEndEvent -= OnShowPictureFrameEndEventCallback;
//    }
//}

//public override void OnInteractionStartEventCallback(InteractableObject interactableObject)
//{
//    base.OnInteractionStartEventCallback(interactableObject);

//}

//public override void OnInteractionStopEventCallback(InteractableObject interactableObject)
//{
//    base.OnInteractionStopEventCallback(interactableObject);
//}

//public override void OnTriggerEnterEvent(Collider collider, StoryBoardEventTrigger trigger)
//{
//    base.OnTriggerEnterEvent(collider, trigger);
//    switch (trigger.EventID)
//    {
//        case 1:
//            OnTriggerSadAnimationEnter(collider, trigger);
//            break;
//        case 2:
//            OnTriggerPictureShow(collider, trigger);
//            break;
//        case 3:
//            OnTriggerDoorOpenEnter(collider, trigger);
//            break;
//        case 4:
//            OnTriggerMotherEnterDoor(collider, trigger);
//            break;

//        default:
//            break;
//    }
//}

//public override void OnTriggerExitEvent(Collider collider, StoryBoardEventTrigger trigger)
//{
//    base.OnTriggerExitEvent(collider, trigger);
//    switch (trigger.EventID)
//    {
//        case 1:
//            OnTriggerSadAnimationExit(collider, trigger);
//            break;

//        default:
//            break;
//    }
//}

//private void OnShowPictureFrameStartEventCallback(InteractableObject intrObject)
//{
//    switch (intrObject.ObjectID)
//    {
//        case 1:
//            m_mother.PlayHappyAnimation();
//            break;

//        default:
//            break;
//    }
//}

//private void OnShowPictureFrameEndEventCallback(InteractableObject intrObject)
//{
//    switch (intrObject.ObjectID)
//    {
//        case 1:
//            m_mother.DoMoveOnPath();
//            break;

//        default:
//            break;
//    }
//}

//private void OnInteractionStartEvent(InteractableObject interactableObject)
//{


//}

//private void OnInteractionEndEvent(InteractableObject interactableObject)
//{

//}

//private void OnTriggerSadAnimationEnter(Collider collider, StoryBoardEventTrigger trigger)
//{
//    if (collider.gameObject.tag == "Mother")
//    {
//        // m_playableDirector.Play();
//        m_mother.PauseOnPath();
//        m_mother.PlayCryingAnimation();
//    }
//}

//public void OnTriggerSadAnimationExit(Collider collider, StoryBoardEventTrigger trigger)
//{
//    if (collider.gameObject.tag == "Player")
//    {
//        Debug.Log(collider.name);
//        if (m_interactableFirstDoor.CurrentDoorState == InteractableDoor.DoorState.Opened)
//        {
//            m_projectorController.EnableProjector(true);
//            m_mother.DoMoveOnPath();
//        }
//    }
//}

//private void OnTriggerDoorOpenEnter(Collider collider, StoryBoardEventTrigger trigger)
//{
//    if (collider.gameObject.tag == "Mother" && m_interactableSecondDoor.CurrentDoorState == InteractableDoor.DoorState.Closed)
//    {
//        m_mother.PauseOnPath();
//        m_mother.PlayPointingAnimation();
//    }
//}

//private void OnTriggerMotherEnterDoor(Collider collider, StoryBoardEventTrigger trigger)
//{
//    if (collider.gameObject.tag == "Mother" && m_interactableSecondDoor.CurrentDoorState == InteractableDoor.DoorState.Opened)
//    {
//        m_mother.PauseOnPath();
//        m_projectorController.EnableProjector(false);
//    }
//}

//private void OnTriggerPictureShow(Collider collider, StoryBoardEventTrigger trigger)
//{
//    if (collider.gameObject.tag == "Mother")
//    {
//        m_mother.PauseOnPath();
//        m_mother.PlayPointingAnimation();
//    }
//}
