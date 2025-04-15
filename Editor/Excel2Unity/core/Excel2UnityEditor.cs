using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

namespace XFramework.Editor
{
    public class Excel2UnityEditor : EditorWindow
    {
        static Excel2UnityEditor window;

        /// <summary>
        /// 项目根路径	
        /// </summary>
        private static string dataPath;

        /// <summary>
        /// Excel路径
        /// </summary>
        private static string excelPath;

        /// <summary>
        /// 编码索引
        /// </summary>
        private static int indexOfEncoding = 0;

        /// <summary>
        /// 保存路径
        /// </summary>
        private static string savePath;

        /// <summary>
        /// 编码选项
        /// </summary>
        private static string[] encodingArray = new string[] { "UTF-8", "GB2312" };

        /// <summary>
        /// 输出类型索引
        /// </summary>
        private static int indexOfType = 0;

        /// <summary>
        /// 输出类型
        /// </summary>
        private static string[] typeArray = new string[] {"Procedure", "Clean", "Goods", "Document", "Valve", "PipeFitting", "Guide" , "Assessment", "Status", "FaultInfo", "KnowledgePoint", "AssemblyStep", "CheckQuestion" };

        [MenuItem("Tools/Excel2Unity")]
        static void Init()
        {
            dataPath = "Assets/StreamingAssets/Product/SettingSource/";
            window = EditorWindow.GetWindow<Excel2UnityEditor>(false, "Excel2Unity", false);
            window.position = new Rect(new Vector2(400, 300), new Vector2(600, 300));
            window.Show();
        }

        private void OnGUI()
        {
            OnDrawGUI();
            OnDrawExport();
        }

        private void OnDrawGUI()
        {
            GUILayout.Space(8);
            GUILayout.BeginHorizontal();
            GUILayout.Space(8);
            EditorGUILayout.LabelField("请选择编码类型", GUILayout.Width(85));
            indexOfEncoding = EditorGUILayout.Popup(indexOfEncoding, encodingArray, GUILayout.Width(125));
            GUILayout.EndHorizontal();
            GUILayout.Space(8);

            GUILayout.BeginHorizontal();
            GUILayout.Space(8);
            EditorGUILayout.LabelField("请选择转换类型", GUILayout.Width(85));
            indexOfType = EditorGUILayout.Popup(indexOfType, typeArray, GUILayout.Width(125));
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.red;
            EditorGUILayout.LabelField(" * 注意类型是否选中正确", style);
            GUILayout.EndHorizontal();
            GUILayout.Space(8);
        }

        private void OnDrawExport()
        {
            if (string.IsNullOrEmpty(excelPath))
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(8);
                EditorGUILayout.LabelField("目前没有Excel文件被选中哦!");
                GUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(8);
                EditorGUILayout.LabelField("当前选中文件", GUILayout.Width(85));
                EditorGUILayout.LabelField(excelPath);
                GUILayout.EndHorizontal();

                GUILayout.Space(8);
                GUILayout.BeginHorizontal();
                GUILayout.Space(8);
                EditorGUILayout.LabelField("请选择保存路径", GUILayout.Width(85));
                savePath = EditorGUILayout.TextField(savePath, GUILayout.Width(485));
                dataPath = savePath.Substring(0, savePath.LastIndexOf("/") + 1);
                GUILayout.EndHorizontal();

                GUILayout.Space(24);
                if (GUILayout.Button("转换"))
                {
                    Debug.Log("开始转换。");
                    Convert();
                    Debug.Log("转换完成。");
                    AssetDatabase.Refresh();
                }
            }
        }

        private void Convert()
        {
            //创建转换接口
            IExcelToUnity excelToUnity = null;
            string type = typeArray[indexOfType];
            switch (type)
            {
                case TypeOption.Clean:
                    break;
                case TypeOption.Goods:
                    excelToUnity = new ExcelToGoods();
                    break;
                case TypeOption.Document:
                    excelToUnity = new ExcelToDocument();
                    break;
                case TypeOption.Procedure:
                    excelToUnity = new ExcelToProcedure();
                    break;
                case TypeOption.Valve:
                    excelToUnity = new ExcelToValve();
                    break;
                case TypeOption.PipeFitting:
                    excelToUnity = new ExcelToPipeFitting();
                    break;
                case TypeOption.Guide:
                    excelToUnity = new ExcelToGuide();
                    break;
                case TypeOption.Assessment:
                    excelToUnity = new ExcelToAssessment();
                    break;
                case TypeOption.Status:
                    excelToUnity = new ExcelToStatus();
                    break;
                case TypeOption.FaultInfo:
                    excelToUnity = new ExcelToFaultInfo();
                    break;
                case TypeOption.KnowledgePoint:
                    excelToUnity = new ExcelToKnowledgePoint();
                    break;
                case TypeOption.AssemblyStep:
                    excelToUnity = new ExcelToAssemblyStep();
                    break;
                case TypeOption.CheckQuestion:
                    excelToUnity = new ExcelToCheckQuestion();
                    break;
                default:
                    break;
            }
            //转换
            if (excelToUnity != null)
            {
                Encoding encoding = Encoding.GetEncoding(encodingArray[indexOfEncoding]);
                excelToUnity.Convert(excelPath, savePath, encoding);
            }
        }

        private void OnSelectionChange()
        {
            Show();
            OnSelection();
            Repaint();
        }

        private void OnSelection()
        {
            if (Selection.objects.Length > 0)
            {
                //获取选中的对象
                Object selection = Selection.objects[0];
                string path = AssetDatabase.GetAssetPath(selection);
                if (Path.GetExtension(path).Equals(".xlsx") || Path.GetExtension(path).Equals(".xls"))
                {
                    excelPath = path;
                    string name = Path.GetFileNameWithoutExtension(excelPath);
                    dataPath = excelPath.Substring(0, excelPath.LastIndexOf("/") + 1);
                    savePath = dataPath + name + ".xml";
                }
                else
                {
                    excelPath = "";
                    savePath = "";
                }
            }
            else
            {
                excelPath = "";
                savePath = "";
            }
        }

        /// <summary>
        /// 转换类型选项
        /// </summary>
        public class TypeOption
        {
            //{ "Clean", "Goods", "Document"};
            /// <summary>
            /// 流程
            /// </summary>
            public const string Procedure = "Procedure";
            public const string Clean = "Clean";
            public const string Goods = "Goods";
            public const string Document = "Document";
            public const string Valve = "Valve";
            public const string PipeFitting = "PipeFitting";
            public const string Guide = "Guide";
            public const string Assessment = "Assessment";
            /// <summary>
            /// 状态
            /// </summary>
            public const string Status = "Status";
            /// <summary>
            /// 故障信息
            /// </summary>
            public const string FaultInfo = "FaultInfo";
            /// <summary>
            /// 知识点
            /// </summary>
            public const string KnowledgePoint = "KnowledgePoint";
            /// <summary>
            /// 拆装步骤
            /// </summary>
            public const string AssemblyStep = "AssemblyStep";

            /// <summary>
            /// 考核题目
            /// </summary>
            public const string CheckQuestion = "CheckQuestion";
        }
    }
}
