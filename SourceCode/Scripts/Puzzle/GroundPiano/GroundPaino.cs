using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Cgl.Thesis.Data;

namespace Cgl.Thesis
{
    public class GroundPaino : InteractableObject
    {
        public GameObject[] m_musicNotes;
        public MoveTowardsFlowPoint[] m_firtMovePoints;

        public Transform m_noteIndicator;
        public PianoTile[] m_noteIndicatorTriggers;
        public Vector3[] m_noteIndicatorRotation;
        public float[] m_roatationSpeeds;
    
        public UnityEvent m_PlayingCompleted;
        public UnityEvent m_onStartPlaying;

        public BoolSO m_aboutFinishInidicator;


        private int currentNoteIndictorPos = 0;
        private bool m_canPlayPiano = true;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ShowPianoNote(PianoTile tile)
        {
            if (!m_canPlayPiano) return;

            int random = Random.Range(0,2);
            GameObject musicNote = m_musicNotes[random];

            GameObject noteRenderObject = Instantiate(musicNote, tile.m_noteStartPosition.position, Quaternion.identity);
            MusicNoteMoveTowardsFlow moveComp = noteRenderObject.GetComponent<MusicNoteMoveTowardsFlow>();

            moveComp.m_currentMovePoint = m_firtMovePoints[Random.Range(0, m_firtMovePoints.Length)];
            moveComp.StartMoving(true);

            if (currentNoteIndictorPos == m_noteIndicatorTriggers.Length -1 && (m_noteIndicatorTriggers[currentNoteIndictorPos] == tile))
            {
                if (m_PlayingCompleted != null)
                {
                    m_PlayingCompleted.Invoke();
                    m_canPlayPiano = false;
                }
            }

            if (currentNoteIndictorPos < m_noteIndicatorTriggers.Length-1) // skipping the last key
            {

                if (m_onStartPlaying != null && m_noteIndicatorTriggers[0] == tile)
                {
                    m_onStartPlaying.Invoke();
                }

                if (m_noteIndicatorTriggers[currentNoteIndictorPos] == tile)
                {
                    m_noteIndicator.DOLocalRotate(m_noteIndicatorRotation[currentNoteIndictorPos], m_roatationSpeeds[currentNoteIndictorPos]);

                    currentNoteIndictorPos++;
                    if (currentNoteIndictorPos == m_noteIndicatorTriggers.Length - 1)
                    {
                        m_aboutFinishInidicator.Value = true;
                    }  
                }
            } 
        }
    }
}
