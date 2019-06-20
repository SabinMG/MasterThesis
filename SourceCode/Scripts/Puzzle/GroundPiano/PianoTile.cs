using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Cgl.Thesis
{
    [RequireComponent(typeof(AudioSource))]
    public class PianoTile : MonoBehaviour
    {
        public GroundPaino m_piano;
        public float m_keyPressSpeed = .5f;
        public float m_keyDefaultXRotaion = -1.95f;
        public float m_keyPressXRotaion = 0.0f;

        public Transform m_noteStartPosition;

        private Vector3 m_initialRotaiion;
        private AudioSource m_audioSource;


        // Start is called before the first frame update
        void Start()
        {
            m_initialRotaiion = transform.rotation.eulerAngles;
            m_audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnPressKey()
        {
            Vector3 pressRotaion = new Vector3(m_keyPressXRotaion, m_initialRotaiion.y, m_initialRotaiion.z);
            transform.DORotate(pressRotaion, m_keyPressSpeed);
            m_audioSource.Play();

            m_piano.ShowPianoNote(this);
        }

        public void OnReleaseKey()
        {
            Vector3 releaseRotaion = new Vector3(m_keyDefaultXRotaion, m_initialRotaiion.y, m_initialRotaiion.z);
            transform.DORotate(releaseRotaion, m_keyPressSpeed);
            m_audioSource.Stop();
        }

    }
}
