using UnityEngine;

namespace XFramework.Architectural
{
    public class VisualCamera2D : VisualCameraBase
    {
        private Camera m_Camera;

        private Vector3 lastPosition;

        private float size = 10f;

        public VisualCamera2D(CameraController cameraController) : base(cameraController)
        {
            m_Camera = cameraController.MainCamera;
        }

        public override void Enter(CameraMode prevMode)
        {
            m_Camera.orthographic = true;
            m_Camera.orthographicSize = VisualCameraSettings.Visual2D.size;
            size = m_Camera.orthographicSize;

            Quaternion rotaion = Quaternion.Euler(VisualCameraSettings.Visual2D.angle);
            m_Camera.transform.rotation = rotaion;
            m_Camera.transform.position = VisualCameraSettings.Visual2D.position;
        }

        public override void Exit(CameraMode nextMode)
        {
            VisualCameraSettings.Visual2D.size.Value = m_Camera.orthographicSize;
            VisualCameraSettings.Visual2D.angle.Value = m_Camera.transform.rotation.eulerAngles;
            VisualCameraSettings.Visual2D.position.Value = m_Camera.transform.position;
        }

        public override void Update()
        {

            if (InputUtils.GetMouseButtonDown(InputUtils.middle_button))
            {
                // TODO
            }
            else if (InputUtils.GetMouseButton(InputUtils.middle_button))
            {
                Vector3 delta = lastPosition - m_Camera.ScreenToWorldPoint(InputUtils.mousePosition);
                Pan(delta);
            }

            float deltaZ = InputUtils.GetAxis(InputUtils.mouse_scroll_wheel);
            Zoom(deltaZ);

            lastPosition = m_Camera.ScreenToWorldPoint(InputUtils.mousePosition);
        }

        public override void Pan(Vector3 delta)
        {
            m_Camera.transform.position += new Vector3(delta.x, 0, delta.z);
        }

        public override void Zoom(float deltaZ)
        {
            if (deltaZ == 0)
                return;

            float z = deltaZ * VisualCameraSettings.Visual2D.zoomSpeed;
            size = Mathf.Clamp(size - z, VisualCameraSettings.Visual2D.sizeMin, VisualCameraSettings.Visual2D.sizeMax);
            m_Camera.orthographicSize = size;

            Fix();
        }

        private void Fix()
        {
            Vector3 delta = lastPosition - m_Camera.ScreenToWorldPoint(InputUtils.mousePosition);
            Pan(delta);
        }

        public override void Reset()
        {
            
        }
    }
}