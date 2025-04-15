using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DG.Tweening;
using XFramework.Common;
using System.Collections;

namespace XFramework.Simulation
{
    public class ContinueRotateAction : ActionBase
    {
        /// <summary>
        /// 物体
        /// </summary>
        public Transform transform;

        private float time=0;
        /// <summary>
        /// 旋转速度
        /// </summary>
        private float speed = -1;

        private bool isOn=false;

        private float currTime;
   
        public ContinueRotateAction(Transform _transform,  float _time, float _speed= -1)
        {
            transform = _transform;
            speed = _speed;
            time = _time;
            // isOn = _isOn;
            currTime = Time.time;
        }
        public override void Execute()
        {

            CoroutineManager.Instance.StartCoroutine(StartRota());


        }

        IEnumerator StartRota()
        {           
           
             while (true)
            {
               
                if (Time.time - currTime < time)
                {
                    transform.Rotate(Vector3.forward, speed * Time.deltaTime * 360);
                }
                else
                {
                    Completed();                 
                    CoroutineManager.Instance.StopCoroutine(StartRota());                  
                }
                yield return new WaitForEndOfFrame();
            } 
             
           
           

            


        }
    }
}
