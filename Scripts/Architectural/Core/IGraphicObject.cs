using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Architectural
{
    public interface IGraphicObject
    {
        Transform transform
        {
            get;
        }

        void Rebuild();
    }
}
