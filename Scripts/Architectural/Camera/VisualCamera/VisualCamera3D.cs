using UnityEngine;

namespace XFramework.Architectural
{
    public class VisualCamera3D : VisualCameraBase
    {
        private Camera m_Camera;

        private Transform m_Pivot;

        private float distance = 5.0f;

        private float x = 0.0f;
        private float y = 0.0f;

        private Plane m_Plane;
        private Quaternion m_Quaternion;
        private Vector3 lastMousePosition;

        private bool rotating;
        private bool panning;

        public VisualCamera3D(CameraController cameraController) : base(cameraController)
        {
            m_Camera = cameraController.MainCamera;
            m_Pivot = cameraController.Pivot;
        }

        public override void Enter(CameraMode prevMode)
        {
            m_Camera.orthographic = false;
            distance = VisualCameraSettings.Visual3D.distance;
            Quaternion rotaion = Quaternion.Euler(VisualCameraSettings.Visual3D.angle);
            m_Camera.transform.rotation = rotaion;
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotaion * negDistance + m_Pivot.position;
            m_Camera.transform.position = position;
            // 同步
            SyncAngles();
        }

        public override void Exit(CameraMode nextMode)
        {
            VisualCameraSettings.Visual3D.distance.Value = distance;
            VisualCameraSettings.Visual3D.angle.Value = m_Camera.transform.rotation.eulerAngles;
        }

        public void SyncAngles()
        {
            Vector3 angles = m_Camera.transform.eulerAngles;
            x = angles.y;
            y = angles.x;
        }

        public override void Update()
        {
            // 旋转
            if (!panning)
            {
                if (InputUtils.GetMouseButtonDown(InputUtils.left_button))
                {
                    rotating = true;
                }
                else if (InputUtils.GetMouseButton(InputUtils.left_button))
                {
                    if (rotating)
                    {
                        Vector2 orbitAxes = InputUtils.GetAxisMouseXY();
                        Orbit(orbitAxes.x, orbitAxes.y, 0);
                    }
                }
                else if (InputUtils.GetMouseButtonUp(InputUtils.left_button))
                {
                    rotating = false;
                }
            }

            // 平移
            if (!rotating)
            {
                if (InputUtils.GetMouseButtonDown(InputUtils.right_button) || InputUtils.GetMouseButtonDown(InputUtils.middle_button))
                {
                    panning = true;
                    lastMousePosition = InputUtils.mousePosition;
                    m_Plane = new Plane(-m_Camera.transform.forward, m_Pivot.position);
                    Vector3 vector = Quaternion.FromToRotation(Vector3.right, m_Camera.transform.right) * Vector3.forward;
                    m_Quaternion = Quaternion.FromToRotation(m_Camera.transform.up, vector);
                }
                else if (InputUtils.GetMouseButton(InputUtils.right_button) || InputUtils.GetMouseButton(InputUtils.middle_button))
                {
                    if (panning)
                    {
                        Vector3 prevPoint;
                        Vector3 point;

                        Vector3 mousePosition = InputUtils.mousePosition;
                        if (GetPointOnPlane(lastMousePosition, out prevPoint) && GetPointOnPlane(mousePosition, out point))
                        {
                            Vector3 delta = prevPoint - point;
                            delta = m_Quaternion * delta;
                            Pan(delta);
                        }

                        lastMousePosition = mousePosition;
                    }
                }
                else if (InputUtils.GetMouseButtonUp(InputUtils.right_button) || InputUtils.GetMouseButtonUp(InputUtils.middle_button))
                {
                    panning = false;
                }
            }

            float deltaZ = InputUtils.GetAxis(InputUtils.mouse_scroll_wheel);
            Zoom(deltaZ);
        }

        private bool GetPointOnPlane(Vector3 mousePosition, out Vector3 point)
        {
            Ray ray = m_Camera.ScreenPointToRay(mousePosition);
            float center;
            if (m_Plane.Raycast(ray, out center))
            {
                point = ray.GetPoint(center);
                return true;
            }

            point = Vector3.zero;
            return false;
        }

        public override void Pan(Vector3 delta)
        {
            m_Camera.transform.position += delta;
            m_Pivot.position += delta;
        }

        public override void Zoom(float deltaZ)
        {
            Orbit(0, 0, deltaZ);
        }

        public override void Reset()
        {
            throw new System.NotImplementedException();
        }

        public void Orbit(float deltaX, float deltaY, float deltaZ)
        {
            if (deltaX == 0 && deltaY == 0 && deltaZ == 0)
                return;

            deltaX = deltaX * VisualCameraSettings.Visual3D.xSpeed;
            deltaY = deltaY * VisualCameraSettings.Visual3D.ySpeed;

            x += deltaX;
            y -= deltaY;
            y = Mathf.Clamp(y % 360, VisualCameraSettings.Visual3D.yMinLimit, VisualCameraSettings.Visual3D.yMaxLimit);


            Quaternion rotaion = Quaternion.Euler(y, x, 0);
            m_Camera.transform.rotation = rotaion;

            float z = deltaZ * VisualCameraSettings.Visual3D.zoomSpeed;

            distance = Mathf.Clamp(distance - z * Mathf.Max(1.0f, distance), VisualCameraSettings.Visual3D.distanceMin, VisualCameraSettings.Visual3D.distanceMax);
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotaion * negDistance + m_Pivot.position;
            m_Camera.transform.position = position;
        }
    }
}