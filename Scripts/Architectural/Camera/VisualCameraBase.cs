using UnityEngine;

namespace XFramework.Architectural
{
    public abstract class VisualCameraBase
    {
        protected CameraController m_CameraController;
        public VisualCameraBase(CameraController cameraController)
        {
            m_CameraController = cameraController;
        }

        public abstract void Enter(CameraMode prevMode);

        public abstract void Exit(CameraMode nextMode);

        public abstract void Update();

        public abstract void Pan(Vector3 delta);

        public abstract void Zoom(float deltaZ);

        public abstract void Reset();
    }
}