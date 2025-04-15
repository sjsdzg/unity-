using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using XFramework.Simulation;

namespace XFramework.Module
{
    /// <summary>
    /// 成绩考核标准
    /// </summary>
    public class AssessmentGrade : IEnumerable<KeyValuePair<int, AssessmentPoint>>
    {
        private int deduction = 2;
        /// <summary>
        /// 操作错误扣除的分值
        /// </summary>
        public int Deduction
        {
            get { return deduction; }
            set { deduction = value; }
        }

        /// <summary>
        /// 考核点总数
        /// </summary>
        public int Count
        {
            get
            {
                return points.Count;
            }
        }

        /// <summary>
        /// 满分
        /// </summary>
        public int Total
        {
            get
            {
                int total = 0;
                foreach (var item in points.Values)
                {
                    total += item.Value;
                }
                return total;
            }
        }

        ///// <summary>
        ///// 所有考核点是否都完成
        ///// </summary>
        //public bool IsAllFinished
        //{
        //    get
        //    {
        //        bool flag = true;
        //        foreach (var item in points.Values)
        //        {
        //            if (!item.IsFinished)
        //            {
        //                flag = false;
        //                break;
        //            }
        //        }
        //        return flag;
        //    }
        //}
        int score = 0;
        /// <summary>
        /// 总得分
        /// </summary>
        public int Score
        {
            get
            {
                score = 0;
                foreach (var item in points.Values)
                {
                    score += item.Score;
                }
                return score;
            }
        }

        /// <summary>
        /// 考核点内容
        /// </summary>
        private Dictionary<int, AssessmentPoint> points = new Dictionary<int, AssessmentPoint>();

        /// <summary>
        /// 考核点索引
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public AssessmentPoint this[int key]
        {
            get
            {
                if (!points.ContainsKey(key))
                    return null;

                return points[key];
            }
            //set
            //{
            //    if (points.ContainsKey(key))
            //        points[key] = value;
            //    else
            //        points.Add(key, value);
            //}
        }

        public IEnumerator<KeyValuePair<int, AssessmentPoint>> GetEnumerator()
        {
            if (null == points)
                yield break;

            foreach (KeyValuePair<int, AssessmentPoint> kvp in points)
            {
                yield return kvp;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return points.GetEnumerator();
        }

        /// <summary>
        /// 添加考核点
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public void Add(int key, AssessmentPoint value)
        {
            if (!points.ContainsKey(key))
            {
                value.AssGrade = this;
                points.Add(key, value);
            }
        }

        /// <summary>
        /// 移除考核点
        /// </summary>
        /// <param name="key">Key.</param>
        public void Remove(int key)
        {
            if (null != points && points.ContainsKey(key))
            {
                points.Remove(key);
            }
        }

        /// <summary>
        /// 某一项减分
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="key"></param>
        public void Deduct(ProductionMode mode, int key)
        {
            switch (mode)
            {
                case ProductionMode.Study:
                    break;
                case ProductionMode.Examine:
                    if (points.ContainsKey(key))
                    {
                        points[key].Deduct();
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 某一项减分
        /// </summary>
        /// <param name="key"></param>
        public void Deduct(int key)
        {
            if (points.ContainsKey(key))
            {
                points[key].Deduct();
            }
        }

        /// <summary>
        /// 某一项完成
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="key"></param>
        public void Finish(ProductionMode mode, int key)
        {
            switch (mode)
            {
                case ProductionMode.Study:
                    break;
                case ProductionMode.Examine:
                    if (points.ContainsKey(key))
                    {
                        points[key].IsFinished = true;
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 某一项完成
        /// </summary>
        /// <param name="key"></param>
        public void Finish(int key)
        {
            if (points.ContainsKey(key))
            {
                points[key].IsFinished = true;
            }
        }
        /// <summary>
        /// 某一项未完成
        /// </summary>
        /// <param name="key"></param>
        public void UnFinish(int key)
        {
            if (points.ContainsKey(key))
            {
                points[key].IsFinished = false;
                
            }
        }
    }
}
