using UnityEngine;

namespace XFramework.Runtime
{
    /// <summary>
    /// 运行时组件
    /// </summary>
    public abstract class RuntimeComponent : MonoBehaviour, IGL
    {
        public abstract void OnDrawFrame();
    }
}