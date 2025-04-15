using UnityEngine;
using System.Collections;

namespace XFramework.Runtime
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class GLLayer : MonoBehaviour
    {
        void OnPreRender()
        {
            //GL.wireframe = true;
        }

        void OnPostRender()
        {
            GLRenderer.Instance.Draw();
        }
    }
}

