using DG.Tweening;
using LiquidVolumeFX;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Actions;
using XFramework.Common;
using XFramework.Module;
using XFramework.Simulation;

namespace XFramework.Component
{
    /// <summary>
    /// 投料
    /// </summary>
    public class FeedingComponent : ComponentBase
    {
        /// <summary>
        /// 绑定FluidLevelCoponent的物体
        /// </summary>
        private FluidLevelComponent fluidLevel;

        /// <summary>
        /// 旋转方向及角度
        /// </summary>
        public Vector3 rot;

        /// <summary>
        /// 对应的粒子效果
        /// </summary>
        public ParticleSystem particle;

        /// <summary>
        /// 投料全程所用总时间
        /// </summary>
        public float allTime;

        private bool isStart;

        public bool IsStart
        {
            get
            {
                return isStart;
            }

            set
            {
                isStart = value;
                FeedingProcess();
            }
        }

        private void Awake()
        {
            particle = transform.GetComponentInChildren<ParticleSystem>(true);
            fluidLevel = transform.GetComponentInChildren<FluidLevelComponent>();
        }

        /// <summary>
        /// 投料过程
        /// </summary>
        /// <param name="_arg"></param>
        /// <param name="bucket">哪一个料桶</param>
        /// <param name="m_rot">桶转动的角度</param>
        /// <param name="particle">哪一个粒子效果</param>
        private void FeedingProcess()
        {
            //particle = transform.GetComponentInChildren<ParticleSystem>();
            //fluidLevel = transform.GetComponentInChildren<FluidLevelComponent>();
            gameObject.SetActive(true);
            //转桶
            StartCoroutine(RotateBucket());

            //投料
            StartCoroutine(Feeding());
        }

        /// <summary>
        /// 液位变化
        /// </summary>
        /// <returns></returns>
        IEnumerator ChangingFluidLevel()
        {
            float offset = 0 - fluidLevel.Value;
            float speed = offset / (allTime-2.5f);
            if (fluidLevel.Value < 0)
            {
                while ((fluidLevel.Value - 0) < 0)
                {
                    fluidLevel.Value += speed * Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
            }
            else if (fluidLevel.Value > 0)
            {
                while ((fluidLevel.Value - 0) > 0)
                {
                    fluidLevel.Value += speed * Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
            }
            //赋值
            fluidLevel.Value = 0;
            fluidLevel.gameObject.SetActive(false);
        }

        /// <summary>
        /// 转桶
        /// </summary>
        /// <returns></returns>
        IEnumerator RotateBucket()
        {
            gameObject.SetActive(true);
            gameObject.transform.DOLocalRotate(rot, allTime-2.5f, RotateMode.LocalAxisAdd);
            yield return new WaitForSeconds(allTime);            
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 投料
        /// </summary>
        /// <returns></returns>
        IEnumerator Feeding()
        {
            yield return new WaitForSeconds((allTime - 2.5f) * 1 / 7);
            particle.gameObject.SetActive(true);
            //液位变化
            StartCoroutine(ChangingFluidLevel());
            yield return new WaitForSeconds(allTime * 11/14-13/28);
            particle.gameObject.SetActive(false);
        }
    }
}
