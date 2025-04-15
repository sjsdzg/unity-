using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.UI
{
    public class ArchitectReport
    {
        /// <summary>
        /// 用户框图
        /// </summary>
        public Texture2D UserGraph { get; set; }

        /// <summary>
        /// 总分
        /// </summary>
        public float TotalScore { get; set; }

        /// <summary>
        /// ItemDataList
        /// </summary>
        public List<ArchitectScoreItemData> ItemDataList { get; set; }

        public void Init(JudgeConditionCollection collection, Architectural.Document document)
        {
            int deductScore = 70;
            ItemDataList = new List<ArchitectScoreItemData>();

            foreach (var judgeCondition in collection.Conditions)
            {
                bool[] states = new bool[2];
                foreach (var wall in document.CurrentFloor.Walls)
                {
                    states[0] = false;
                    states[1] = false;

                    foreach (var relatedRoom in wall.RelatedRooms)
                    {
                        if (relatedRoom.Room.Name.Equals(judgeCondition.Start))
                        {
                            states[0] = true;
                        }

                        if (relatedRoom.Room.Name.Equals(judgeCondition.End))
                        {
                            states[1] = true;
                        }
                    }

                    if (states[0] && states[1])
                    {
                        break;
                    }
                }

                if (!states[0] || !states[1])
                {
                    ArchitectScoreItemData itemData = new ArchitectScoreItemData();
                    itemData.Name = (ItemDataList.Count + 1).ToString();
                    itemData.User = "【" + judgeCondition.Start + "】和【" + judgeCondition.End + "】未连接！";
                    itemData.Score = "-2";
                    ItemDataList.Add(itemData);
                    // 总分减 2
                    deductScore -= 2;
                }
            }
            // 总分
            float baseScore = document.CurrentFloor.Groups.Count * 30 / 52.0f;
            TotalScore = deductScore + Mathf.CeilToInt(baseScore);

            if (TotalScore < 0)
                TotalScore = 0;
            else if (TotalScore > 100)
                TotalScore = 100;
        }
    }
}
