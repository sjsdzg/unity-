using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;
using XFramework.Component;

namespace XFramework.Actions
{
    /// <summary>
    /// 投料
    /// </summary>
    public class FeedingAction : ActionBase
    {
        /// <summary>
        /// 物料桶
        /// </summary>
        private GameObject bucket;

        /// <summary>
        /// 绑定FluidLevelCoponent的物体
        /// </summary>
        private GameObject fluidLevel;

        /// <summary>
        /// 旋转方向及角度
        /// </summary>
        private Vector3 rot;
        /// <summary>
        /// 对应的粒子效果
        /// </summary>
        //public GameObject particle;
        /// <summary>
        /// 投料全程所用总时间
        /// </summary>
        private float allTime;
        /// <summary>
        /// 是否立刻返回完成
        /// </summary>
        public bool immediate;
        public FeedingAction(GameObject _bucket,Vector3 _rot,float _allTime, bool _immediate)
        {
            bucket = _bucket;            
            rot = _rot;
            allTime = _allTime;
            immediate = _immediate;
            //particle = _particle;
        }
        public override void Execute()
        {
            FeedingComponent feeding = bucket.GetOrAddComponent<FeedingComponent>();
            if (feeding != null)
            {
                //feeding.particle = particle;
                feeding.rot = rot;
                feeding.allTime = allTime;
                //bucket.gameObject.SetActive(true);
                feeding.IsStart = true;

                if (immediate == true)
                {
                    Completed();
                }
                else
                {
                    CoroutineManager.Instance.StartCoroutine(EndFeeding());
                }
            }
            else
            {
                Error(new Exception("feeding is null"));
            }
        }
        /// <summary>
        /// 结束投料以后
        /// </summary>
        /// <returns></returns>
        IEnumerator EndFeeding()
        {
            yield return new WaitForSeconds(allTime);
            Completed();
        }
    }
}
