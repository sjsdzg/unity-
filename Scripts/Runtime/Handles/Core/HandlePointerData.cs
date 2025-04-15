using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Runtime
{
    public class HandlePointerData
    {
        /// <summary>
        /// Mouse : 0 : Left 1 : Right 2 : Middle
        /// Touch : 
        /// </summary>
        public int pointerId { get; set; }

        public BaseHandle pointerEnter { get; set; }

        public BaseHandle pointerPress { get; set; }

        public BaseHandle pointerDrag { get; set; }

        public Vector2 position { get; set; }

        public Vector2 delta { get; set; }

        public Vector2 pressPosition { get; set; }

        public Vector2 scrollDelta { get; set; }

        public bool useDragThreshold { get; set; }

        public bool dragging { get; set; }

        public int pointerCurrentControl { get; set; }

        public int pointerPressControl { get; set; }

        public bool IsPointerMoving()
        {
            return delta.sqrMagnitude > 0.0f;
        }
    }
}
