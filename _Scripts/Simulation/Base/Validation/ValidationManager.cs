using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFramework.Core;
using XFramework.Simulation;

namespace XFramework.Simulation
{
    /// <summary>
    /// 验证管理类
    /// </summary>
    public class ValidationManager : Singleton<ValidationManager>
    {
        /// <summary>
        /// 验证字典
        /// </summary>
        private Dictionary<ValidationType, BaseValidation> dicValidations = null;

        protected override void Init()
        {
            base.Init();
            dicValidations = new Dictionary<ValidationType, BaseValidation>();
        }

        #region Get Validation
        /// <summary>
        /// get module by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public BaseValidation GetValidation(ValidationType validationType)
        {
            if (dicValidations.ContainsKey(validationType))
                return dicValidations[validationType];
            return null;
        }
        #endregion

        #region Load Validation
        /// <summary>
        /// 加载验证
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">流程类型</param>
        /// <param name="mode">生产模式</param>
        public void Load(ValidationType validationType, ProductionMode mode)
        {
            if (!dicValidations.ContainsKey(validationType))
            {
                Type t = ValidationDefine.GetValidationScript(validationType);
                BaseValidation validation = Activator.CreateInstance(t) as BaseValidation;
                validation.Initialize(validationType, mode);
                dicValidations.Add(validationType, validation);
            }
        }
        #endregion

        #region Release Validation
        /// <summary>
        /// 卸载验证
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Unload(ValidationType validationType)
        {
            if (dicValidations.ContainsKey(validationType))
            {
                BaseValidation validation = dicValidations[validationType];
                validation.Release();
                dicValidations.Remove(validationType);
            }
        }

        /// <summary>
        /// 释放所有验证
        /// </summary>
        public void UnloadAll()
        {
            List<ValidationType> _keyList = new List<ValidationType>(dicValidations.Keys);
            for (int i = 0; i < _keyList.Count; i++)
            {
                Unload(_keyList[i]);
            }
            dicValidations.Clear();
        }
        #endregion
    }
}
