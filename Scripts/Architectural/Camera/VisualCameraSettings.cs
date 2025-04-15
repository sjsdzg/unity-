using UnityEngine;
using XFramework.Core;

namespace XFramework.Architectural
{
    public abstract class VisualCameraSettings
    {
        public class Visual2D
        {
            public static PrefVector3 position = new PrefVector3("VisualCamera.Visual2D.position", new Vector3(30, 20, 36));
            public static PrefVector3 angle = new PrefVector3("VisualCamera.Visual2D.angle", new Vector3(90, 0, 0));
            public static PrefFloat size = new PrefFloat("VisualCamera.Visual2D.size", 48f);
            public static PrefFloat sizeMin = new PrefFloat("VisualCamera.Visual2D.sizeMin", 1f);
            public static PrefFloat sizeMax = new PrefFloat("VisualCamera.Visual2D.sizeMax", 120f);
            public static PrefFloat zoomSpeed = new PrefFloat("VisualCamera.Visual2D.zoomSpeed", 10f);
        }

        public class Visual3D
        {
            public static PrefVector3 pivot = new PrefVector3("VisualCamera.Visual3D.pivot", new Vector3(36, 0, 36));
            public static PrefVector3 angle = new PrefVector3("VisualCamera.Visual3D.angle", new Vector3(45, 0, 0));
            public static PrefFloat xSpeed = new PrefFloat("VisualCamera.Visual3D.xSpeed", 5.0f);
            public static PrefFloat ySpeed = new PrefFloat("VisualCamera.Visual3D.ySpeed", 5.0f);
            public static PrefFloat zoomSpeed = new PrefFloat("VisualCamera.Visual3D.zoomSpeed", 0.5f);
            public static PrefFloat distance = new PrefFloat("VisualCamera.Visual3D.distance", 80.0f);
            public static PrefFloat distanceMin = new PrefFloat("VisualCamera.Visual3D.distanceMin", 0.5f);
            public static PrefFloat distanceMax = new PrefFloat("VisualCamera.Visual3D.distanceMax", 1000f);
            public static PrefFloat yMinLimit = new PrefFloat("VisualCamera.Visual3D.yMinLimit", -360f);
            public static PrefFloat yMaxLimit = new PrefFloat("VisualCamera.Visual3D.yMaxLimit", 360f);
        }
    }
}                                                  