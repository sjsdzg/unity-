using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace XFramework.Core
{
    public class MonoDriver : DDOLSingleton<MonoDriver>
    {
        private WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();

        private List<IUpdate> m_Updates = new List<IUpdate>();

        private void Update()
        {
            for (int i = 0; i < m_Updates.Count; i++)
            {
                IUpdate driver = m_Updates[i];
                driver.Update();
            }
        }

        public static void Attach(IDriver driver)
        {
            Instance.InternalAttach(driver);
        }

        private void InternalAttach(IDriver driver)
        {
            if (driver is IUpdate)
            {
                m_Updates.Add(driver as IUpdate);
            }
        }

        public static void Detach(IDriver driver)
        {
            Instance.InternalDetach(driver);
        }

        private void InternalDetach(IDriver driver)
        {
            if (driver is IUpdate)
            {
                m_Updates.Remove(driver as IUpdate);
            }
        }

        /// <summary>
        /// 等待一定帧数后，执行
        /// </summary>
        /// <param name="number"></param>
        /// <param name="action"></param>
        public void WaitForFrame(int number, Action action)
        {
            if (number <= 0)
                return;

            if (action == null)
                return;

            StartCoroutine(OnWaitForFrame(number, action));
        }

        private IEnumerator OnWaitForFrame(int number, Action action)
        {
            for (int i = 0; i < number; i++)
            {
                yield return endOfFrame;
            }

            action.Invoke();
        }
    }
}

