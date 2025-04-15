using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;
using XFramework.Common;
using XFramework.Module;

namespace XMLTest
{
    public class Program : MonoBehaviour
    {
        void Start()
        {
            // Deserialize 
            Stopwatch watch = new Stopwatch();
            watch.Start();
            string path = Application.streamingAssetsPath + "/Simulation/Project.xml";
            Project procedure = XMLHelper.DeserializeFromFile<Project>(path, Encoding.UTF8);
            //serializer.Serialize(Console.Out, personen);
            //foreach (var item in procedure.Entities)
            //{
            //    UnityEngine.Debug.Log(item.Name);
            //}
            watch.Stop();
            print(watch.ElapsedMilliseconds);
        }
    }
}
