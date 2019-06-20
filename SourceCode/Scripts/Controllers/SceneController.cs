using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cgl.Thesis.Data;

namespace Cgl.Thesis
{
    public class SceneController : Singleton<SceneController>
    {
        public bool m_playFromIntro;
        public bool m_startingFromtheBeging = true; // might be need to changed later
        public IntroController m_introController;
        public BoolSO m_playerInputEnabled;
        public InteractionController m_interactionController;
        public InteractableObjectFBBIK m_flowerObject;

       
        public Transform m_cameraTransform;
        public Transform m_playerTransfrom;
        public Transform m_motherTransfrom;
        public TransformDataSO m_startcameraTransformData;
        public TransformDataSO m_startplayerTransfromData;
        public TransformDataSO m_motherTransfromData;


        public override void Awake()
        {
          

            base.Awake();
            InitializeSceneForIntro(m_playFromIntro);
        }
  
        // Start is called before the first frame update
        private void Start()
        {
            if (m_startingFromtheBeging)
            {
                m_cameraTransform.position = m_startcameraTransformData.position;
                m_playerTransfrom.position = m_startplayerTransfromData.position;
                m_motherTransfrom.position = m_motherTransfromData.position;
            }

            if (m_playFromIntro)
            {
                m_introController.StartInroduction(() => { OnIntroComplete(); });
            }
            else
            {
                if(m_startingFromtheBeging) m_interactionController.ShowInteractionIndicator(m_flowerObject.m_interactionIndicatorPosition.position);
                m_playerInputEnabled.Value = true;
            }
        }

        // Update is called once per frame
        private void Update()
        {

        }

        private void OnIntroComplete()
        {
            m_playerInputEnabled.Value = true;
            m_interactionController.ShowInteractionIndicator(m_flowerObject.m_interactionIndicatorPosition.position);
        }

        private void InitializeSceneForIntro(bool startFromIntro)
        {
            m_introController.InitialilzeForIntro(startFromIntro);
        }
    }
}
