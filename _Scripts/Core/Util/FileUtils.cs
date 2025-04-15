using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.Core
{
    public static class FileUtils
    {
        private static int fileCount = 0;

        /// <summary>
        /// 获取路径下所有目录和文件的总数
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static int GetFileSystemEntryCount(string path)
        {
            fileCount = 0;
            CountFileSystemEntry(path);
            return fileCount;
        }

        /// <summary>
        /// 统计FileSystemEntry的递归函数
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static int CountFileSystemEntry(string path)
        {
            try
            {
                string[] entries = Directory.GetFileSystemEntries(path);
                fileCount += entries.Length;
                foreach (var entry in entries)
                {
                    if (Directory.Exists(entry))
                    {
                        CountFileSystemEntry(entry);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }

            return fileCount;
        }

    }
}
