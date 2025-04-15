using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.Core
{
    public class DataManager
    {
        public static T GetData<T>(string s) where T : IDataObject<T>, new()
        {
            T t = new T();
            return t;
        }

        public static T GetData<T>(byte[] bytes) where T : IDataObject<T>, new()
        {
            T t = new T();
            return t;
        }

        public static void SaveData<T>(T t, string path)
        {

        }
    }
}
