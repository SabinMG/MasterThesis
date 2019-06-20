using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UB;

namespace Cgl.Thesis
{
    public class IntroController : MonoBehaviour
    {
        public float m_splashLifeTime = 3.0f;
        public SplashScreen m_splashScreen;
        public float m_splashFadeSpeed = 3.0f;
        public D2FogsNoiseTexPE m_screenFog;
        public float m_fogFadeSpeed = 3.0f;
        public Player m_player;

        public Action OnIntroCompleteAction;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        }

        public void InitialilzeForIntro(bool initForIntro)
        {
            if (initForIntro)
            {
                m_screenFog.enabled = true;
                m_screenFog.Density = 5.0f;
                m_splashScreen.Alpha = 1.0f;
                m_splashScreen.gameObject.SetActive(true);
                m_player.InitialilzeForIntro(true);
            }
            else
            {
                m_player.InitialilzeForIntro(false);
                m_screenFog.enabled = false;
                m_splashScreen.gameObject.SetActive(false);
            }
        }

        public void StartInroduction(Action oncomplete)
        {
            OnIntroCompleteAction = oncomplete;
            m_player.OnPlayerStandUpAnimationFinished += OnPlayerStandUpAnimFinished;
            StartCoroutine(IntroRoutine());
        }

        private void OnPlayerStandUpAnimFinished()
        {
            Debug.Log("intro finished");
            m_player.InitialilzeForIntro(false);
            m_player.OnPlayerStandUpAnimationFinished -= OnPlayerStandUpAnimFinished;
            OnIntroCompleteAction();
        }

        IEnumerator IntroRoutine()
        {
            yield return new WaitForSeconds(m_splashLifeTime);
            StartCoroutine(FadeOutSplashRoutine());
            StartCoroutine(FadeOutFogRoutine());
            yield return null;
        }

        IEnumerator FadeOutSplashRoutine()
        {
            while (m_splashScreen.Alpha > 0.0f)
            {
                m_splashScreen.Alpha -= Time.deltaTime * m_splashFadeSpeed;
                yield return null;
            }
            //onComplete();
            // m_screenFog.enabled = false;
            yield return null;
        }

        IEnumerator FadeOutFogRoutine()
        {
            while (m_screenFog.Density > 0.0f)
            {
                m_screenFog.Density -= Time.deltaTime * m_fogFadeSpeed;
                yield return null;
            }
            //onComplete();
           // m_screenFog.enabled = false;
            yield return null;
        } 
    }
}
