using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;
using DG.Tweening;

namespace XFramework.Actions
{
    public class SetCameraPositionAction : ActionBase
    {


        /// <summary>
        /// position
        /// </summary>
        public Vector3 pos;
        /// <summary>
        /// size
        /// </summary>
        public float size;
        public SetCameraPositionAction(Vector3 _position, float _size = 21f)
        {
            pos = _position;
            size = _size;
        }

        public override void Execute()
        {
            Camera.main.transform.DOLocalMove(pos,2f).OnComplete(()=>{
                Camera.main.orthographicSize = size;
            });           
            Completed();
        }
    }
    
}
