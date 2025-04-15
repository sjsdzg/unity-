using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;
using XFramework.UI;
using XFramework.Common;

namespace XFramework.Module
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class MyselfControl : MonoBehaviour
    {
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        public bool disable; //是否禁用

        /// <summary>
        /// 自身投射器
        /// </summary>
        private Projector projector;

        /// <summary>
        /// NPC名称
        /// </summary>
        private TextMeshPro textName;

        /// <summary>
        /// CameraController
        /// </summary>
        private CameraController cameraControl;

        CapsuleCollider m_Capsule;

        private EntityMyself entity = null;
        /// <summary>
        /// 对应实体
        /// </summary>
        public EntityMyself Entity
        {
            get { return entity; }
            set { entity = value; }
        }

        private void Start()
        {
            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacter>();
            projector = transform.GetComponentInChildren<Projector>();
            //textName = transform.GetComponentInChildren<TextMeshPro>();
            m_Capsule = GetComponent<CapsuleCollider>();
            projector.gameObject.SetActive(false);
            cameraControl = m_Cam.GetComponent<CameraController>();

            //if (Entity != null)
            //{
            //    textName.text = Entity.Name;
            //}
        }

        private void Update()
        {
            //if (textName!=null)
            //{
            //    textName.transform.eulerAngles = Camera.main.transform.rotation.eulerAngles;
            //}
          
        }

        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            if (disable)
            {
                m_Character.Move(Vector3.zero, false, false);
                return;
            }

            // read inputs
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = (v * m_CamForward + h * m_Cam.right);
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = (v * Vector3.forward + h * Vector3.right);

                Debug.Log("m_Move : " + m_Move);
            }

            //速度变慢
            //m_Move *= 0.5f;
            bool _bo = Input.GetKey(KeyCode.LeftControl);
            //if (_bo && cameraControl.PersonMode == PersonMode.FirstPerson)
            //{
            //    cameraControl.offsetVector = Vector3.Lerp(cameraControl.offsetVector, new Vector3(0, 1f, 0), Time.deltaTime * 10);
            //}
            //else
            //{
            //    cameraControl.offsetVector = Vector3.Lerp(cameraControl.offsetVector, new Vector3(0, m_Capsule.height, 0), Time.deltaTime * 10);
            //}

            // pass all parameters to the character control script
            m_Character.Move(m_Move, false, false);
        }
    }
    
}
