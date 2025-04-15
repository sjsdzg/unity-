using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XFramework.Common
{
    public enum InternalTriggerType
    {
        OnAwake,
		OnStart,
		OnUpdate,
		OnFixedUpdate,
		OnLateUpdate,
		OnDrawGizmos,
		OnMouseDown,
		OnMouseDrag,
		OnMouseEnter,
		OnMouseExit,
		OnMouseOver,
		OnMouseUp,
		OnTriggerEnter,
		OnTriggerExit,
		OnTriggerStay,
		OnGetMouseButtonDown,
		OnGetMouseButtonUp,
		OnGetMouseButton,
		OnGetKey,
		OnGetKeyDown,
		OnGetKeyUp,
    }
}
