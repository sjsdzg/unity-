using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace XFramework.Architectural
{
    public interface IGraphicRaycast
    {
        bool Raycaset(Vector2 screenPoint, Camera eventCamera, out RaycastInfo result);
    }
}
