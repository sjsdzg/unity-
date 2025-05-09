﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XFramework.Common
{
    public enum CameraMode
    {
        Always,
        Hold
    }

    [RequireComponent(typeof(CameraController))]
    public class FreeForm : MonoBehaviour
    {
        public bool cameraEnabled = true;
        public CameraMode cameraMode = CameraMode.Hold;

        public bool controllerEnabled = false;
        public bool controllerInvertY = true;
        public bool mouseInvertY = false;
        public bool lockMouseCursor = true;
        public bool limitTab = false;

        public Vector2 mouseSensitivity = new Vector2(1.5f, 1.0f);
        public Vector2 controllerSensitivity = new Vector2(1.0f, 0.7f);

        [HideInInspector]
        public float x;
        [HideInInspector]
        public float y;
        private float yAngle;
        private float angle;

        private string rightAxisXName;
        private string rightAxisYName;
        private bool mouse0;
        private bool mouse1;

        private Vector3 upVector;
        private Vector3 downVector;

        private bool smartPivotInit;

        private CameraController cameraController;
        private float rawDistance = 0;


        public void Start()
        {
            cameraController = GetComponent<CameraController>();

            if (!cameraController.IsThirdPerson)
            {
                rawDistance = cameraController.desiredDistance;
                cameraController.desiredDistance = 0;
            }

            mouse0 = false;
            mouse1 = false;

            x = 0;
            y = 0;

            smartPivotInit = true;

            upVector = Vector3.up;
            downVector = Vector3.down;

            string platform = Application.platform.ToString().ToLower();

            if (platform.Contains("windows") || platform.Contains("linux"))
            {
                rightAxisXName = "Right_4";
                rightAxisYName = "Right_5";
            }
            else
            {
                rightAxisXName = "Right_3";
                rightAxisYName = "Right_4";
            }

            // test if the controller axis are setup
            try
            {
                Input.GetAxis(rightAxisXName);
                Input.GetAxis(rightAxisYName);
            }
            catch
            {
                //Debug.LogWarning("Controller Error - Right axis not set in InputManager. Controller is disabled!");
                controllerEnabled = false;
            }
        }

        public void Update()
        {
            if (IsPointerOverUI())
                return;

            if (cameraController.Target == null)
                return;

            if (cameraEnabled)
            {
                //切换视角
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    ChangeCamera();
                }

                mouse0 = Input.GetMouseButton(0);
                mouse1 = Input.GetMouseButton(1);

                if ((cameraMode == CameraMode.Hold && mouse1) || cameraMode == CameraMode.Always)
                {
                    x = Input.GetAxis("Mouse X") * mouseSensitivity.x;
                    y = Input.GetAxis("Mouse Y") * mouseSensitivity.y;

                    if (mouseInvertY)
                        y *= -1.0f;

                    if (lockMouseCursor)
                    {
                        Cursor.visible = false;
                        Cursor.lockState = CursorLockMode.Locked;
                    }
                }
                else
                {
                    x = 0;
                    y = 0;

                    if (Cursor.lockState == CursorLockMode.Locked)
                    {
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.None;
                    }
                }

                if (controllerEnabled && x == 0 && y == 0)
                {
                    x = Input.GetAxis(rightAxisXName) * controllerSensitivity.x;
                    y = Input.GetAxis(rightAxisYName) * controllerSensitivity.y;

                    if (controllerInvertY)
                        y *= -1.0f;
                }

                if (cameraController.IsThirdPerson)
                {
                    if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
                    {
                        cameraController.desiredDistance += cameraController.zoomOutStepValue;
                    }
                    if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
                    {
                        cameraController.desiredDistance -= cameraController.zoomOutStepValue;
                    }

                    if (cameraController.desiredDistance < 0)
                        cameraController.desiredDistance = 0;

                    if (cameraController.desiredDistance < cameraController.minDistance)
                        cameraController.desiredDistance = cameraController.minDistance;

                    if (cameraController.desiredDistance > cameraController.maxDistance)
                        cameraController.desiredDistance = cameraController.maxDistance;
                }

                Vector3 offsetVectorTransformed = cameraController.Target.transform.rotation * cameraController.offsetVector;

                transform.RotateAround(cameraController.Target.position + offsetVectorTransformed, cameraController.Target.up, x);

                yAngle = -y;
                // Prevent camera flipping
                angle = Vector3.Angle(transform.forward, upVector);
                if (yAngle > 0)
                {
                    if (angle + yAngle > 180.0f)
                    {
                        yAngle = Vector3.Angle(transform.forward, upVector) - 180;

                        if (yAngle < 0)
                            yAngle = 0;
                    }
                }
                else
                {
                    if (angle + yAngle < 0.0f)
                    {
                        yAngle = Vector3.Angle(transform.forward, downVector) - 180;
                    }
                }

                if (!cameraController.smartPivot || cameraController.cameraNormalMode
                    && (!cameraController.bGroundHit || (cameraController.bGroundHit && y < 0) || transform.position.y > (cameraController.Target.position.y + cameraController.offsetVector.y)))
                {
                    // normal mode
                    transform.RotateAround(cameraController.Target.position + offsetVectorTransformed, transform.right, yAngle);
                }
                else
                {
                    // smart pivot mode
                    if (smartPivotInit)
                    {
                        smartPivotInit = false;
                        cameraController.InitSmartPivot();
                    }

                    transform.RotateAround(transform.position, transform.right, yAngle);

                    if (transform.rotation.eulerAngles.x > cameraController.startingY || (transform.rotation.eulerAngles.x >= 0 && transform.rotation.eulerAngles.x < 90))
                    {
                        smartPivotInit = true;

                        cameraController.DisableSmartPivot();
                    }
                }
            }
        }

        /// <summary>
        /// 判断是否在UI上
        /// </summary>
        /// <returns></returns>
        public bool IsPointerOverUI()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.pressPosition = Input.mousePosition;
            eventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            if (results.Count > 0)
            {
                if (results[0].gameObject.layer == 5)
                {
                    return true;
                }
            }

            return false;
        }

        public void ChangeCamera()
        {
            if (limitTab)
                return;
            cameraController.IsThirdPerson = !cameraController.IsThirdPerson;
            if (cameraController.IsThirdPerson)
            {
                cameraController.PersonMode = PersonMode.ThirdPerson;
                cameraController.desiredDistance = rawDistance;
            }
            else
            {
                cameraController.PersonMode = PersonMode.FirstPerson;
                rawDistance = cameraController.desiredDistance;
                cameraController.desiredDistance = 0;
            }
        }
    }
}

