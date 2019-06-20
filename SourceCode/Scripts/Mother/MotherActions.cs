using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cgl.Thesis.Data;
using DG.Tweening;

namespace Cgl.Thesis
{
    public class MotherActions : MonoBehaviour
    {
        public Mother m_mother;
        private FlatBoat m_currentBoat;
        private InteractableBox m_currentBox;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartMovingCurrentOnPath()
        {
            m_mother.StartMovingOnCurrentPath();
        }

        public void StartMovingOnPath(DOTweenPath path)
        {
            Debug.Log("start moving"+ path.name);
            m_mother.StartMovingOnPath(path);
        }

        public void EndMovingOnPath()
        {
            m_mother.PauseMovingOnPath(true);
        }

        public void PauseMovingOnCurrentPath(bool playIdle)
        {
            m_mother.PauseMovingOnPath(playIdle);
        }

        public void SetToHappyMode()
        {
            m_mother.PlayHappyAnimation();
        }


        public void EnterToBoat(FlatBoat boat)
        {
            m_currentBoat = boat;
            m_mother.WalkTowards(boat.m_RightEdgePoint.position, 3.0f,true,()=>{ m_currentBoat.m_onMotherEnterBoat.Invoke(); });
            m_mother.transform.parent = boat.transform;
        }

        public void ExitBoat(Transform position)
        {
            m_mother.WalkTowards(position.position, 3.0f,false, () => { m_currentBoat.m_onMotherExitBoat.Invoke(); });
            m_mother.transform.parent = null;
        }

        public void MoveTowardsPosition(Vector3 position)
        {
            m_mother.WalkTowards(position, 0.2f, false, () => { });
        }

        public void SetToPosition(Transform positionTrans)
        {
            m_mother.SetToPosition(positionTrans); 
        }

        public void EnterToDraggableBoat(InteractableBox box)
        {
            m_mother.transform.parent = box.transform;
            m_mother.PauseMovingOnPath(true);
            m_currentBox = box;
        }

        public void ExitFromDraggableBox()
        {
            m_mother.transform.parent = null;
        }

        public void ExitFromDraggableBox(Transform exitPosition)
        {
            m_mother.WalkTowards(exitPosition.position, 1.0f, false, () => {  });
            m_mother.transform.parent = null;
        }

        public void EnterToElevator(LevelElevator elevator)
        {
            m_mother.transform.parent = elevator.transform;
        }

        public void ExitElevator(LevelElevator elevator)
        {
            m_mother.transform.parent = null;
        }

        public void EnterToRailBox(LevelRailBox railBox)
        {
            m_mother.transform.parent = railBox.transform;
        }

        public void ExitRailBox(LevelRailBox railBox)
        {
            m_mother.transform.parent = null;
        }

        public void EnterToMovableLand(LevelMovableLand land)
        {
            m_mother.transform.parent = land.transform;
        }

        public void ExitRailMovableLand(LevelMovableLand land)
        {
            m_mother.transform.parent = null;
        }
    }
}
