using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.UI
{
    public class JudgeCondition
    {
        public string Start { get; set; }

        public string End { get; set; }

        public JudgeCondition(string start, string end)
        {
            Start = start;
            End = end;
        }
    }

    public class JudgeConditionCollection
    {
        public List<JudgeCondition> Conditions { get; set; }

        public static JudgeConditionCollection LoadFromResources(string path)
        {
            TextAsset asset = Resources.Load<TextAsset>(path);

            JudgeConditionCollection collection = new JudgeConditionCollection();
            collection.Conditions = new List<JudgeCondition>();

            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(asset.text)))
            {
                using (TextReader reader = new StreamReader(ms))
                {
                    string line; 
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] strs = line.Split(' ');

                        for (int i = 0; i < strs.Length - 1; i++)
                        {
                            JudgeCondition condition = new JudgeCondition(strs[i], strs[i + 1]);
                            collection.Conditions.Add(condition);
                        }
                    }
                }
            }
            return collection;
        }
    }
}
