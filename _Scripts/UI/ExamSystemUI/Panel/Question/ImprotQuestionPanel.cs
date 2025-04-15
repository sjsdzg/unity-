using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Common;
using XFramework.Module;
using XFramework.Proto;
using Crosstales.FB;
using System.Collections;
using UnityEngine.Networking;

namespace XFramework.UI
{
    public class ImprotQuestionPanel : AbstractPanel
    {
        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.ImprotQuestionPanel;
        }

        public const string questionTemplate = "/Files/template/试题_批量导入模板.xls";

        public const string BLANK = "BLANK";

        /// <summary>
        /// 地址栏
        /// </summary>
        protected AddressBar addressBar;

        /// <summary>
        /// 模板下载按钮
        /// </summary>
        private Button buttonDownload;

        /// <summary>
        /// 题库Dropdown
        /// </summary>
        private Dropdown dropdownBank;

        /// <summary>
        /// 文件选择按钮
        /// </summary>
        private Button buttonSelect;

        /// <summary>
        /// 文件资源路径
        /// </summary>
        private Text fileUrl;

        /// <summary>
        /// 提交栏
        /// </summary>
        private SubmitBar submitBar;

        /// <summary>
        /// 题库警告文本
        /// </summary>
        private Text bankWarningText;

        /// <summary>
        /// 文件警告文本
        /// </summary>
        private Text fileWarningText;

        /// <summary>
        /// 选项名称对应
        /// </summary>
        Dictionary<int, string> AlisaMap = new Dictionary<int, string>();

        /// <summary>
        /// 试题模块
        /// </summary>
        private QuestionModule questionModule;

        /// <summary>
        /// 试题库模块
        /// </summary>
        private QuestionBankModule questionBankModule;

        /// <summary>
        /// 试题库列表
        /// </summary>
        private List<QuestionBank> m_QuestionBanks;


        protected override void OnAwake()
        {
            AlisaMap.Add(1, "A");
            AlisaMap.Add(2, "B");
            AlisaMap.Add(3, "C");
            AlisaMap.Add(4, "D");
            AlisaMap.Add(5, "E");
            AlisaMap.Add(6, "F");
            AlisaMap.Add(7, "G");
            AlisaMap.Add(8, "H");
            AlisaMap.Add(9, "I");

            base.OnAwake();
            InitGUI();
            InitEvent();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        public void InitGUI()
        {
            addressBar = transform.Find("AddressBar").GetComponent<AddressBar>();
            buttonDownload = transform.Find("Panel/GuideSecond/ButtonDownload").GetComponent<Button>();
            dropdownBank = transform.Find("Panel/DropdownBank").GetComponent<Dropdown>();
            buttonSelect = transform.Find("Panel/File/ButtonSelect").GetComponent<Button>();
            fileUrl = transform.Find("Panel/File/FileUrl").GetComponent<Text>();
            submitBar = transform.Find("SubmitBar").GetComponent<SubmitBar>();
            bankWarningText = transform.Find("Panel/BankWarningText").GetComponent<Text>();
            fileWarningText = transform.Find("Panel/File/FileWarningText").GetComponent<Text>();
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        public void InitEvent()
        {
            addressBar.OnClicked.AddListener(addressBar_OnClicked);
            buttonDownload.onClick.AddListener(buttonDownload_onClick);
            dropdownBank.onValueChanged.AddListener(dropdownBank_onValueChanged);
            buttonSelect.onClick.AddListener(buttonSelect_onClick);
            submitBar.OnSubmit.AddListener(submitBar_OnSubmit);
            submitBar.OnCancel.AddListener(submitBar_OnCancel);
        }

        protected override void OnStart()
        {
            base.OnStart();
            questionModule = ModuleManager.Instance.GetModule<QuestionModule>();
            questionBankModule = ModuleManager.Instance.GetModule<QuestionBankModule>();
            //先获取所有试题库
            questionBankModule.ListQuestionBankByCondition(SqlCondition.ListBySoftwareId(), ReceiveListQuestionBankByConditionResp);
        }

        private void addressBar_OnClicked(EnumPanelType type)
        {
            Release();
            PanelManager.Instance.OpenPanelCloseOthers(Parent, type);
        }

        protected override void SetPanel(params object[] PanelParams)
        {
            base.SetPanel(PanelParams);
            addressBar.AddHyperButton(EnumPanelType.ExamHomePanel, PanelDefine.GetPanelComment(EnumPanelType.ExamHomePanel));
            addressBar.AddHyperButton(EnumPanelType.AddOrModifyQuestionBankPanel, "添加题库");
        }

        /// <summary>
        /// 下载试题模板
        /// </summary>
        private void buttonDownload_onClick()
        {
            try
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                WebGLUtils.OpenUrl(AppSettings.Settings.AssetServerUrl + questionTemplate);
#else
                string path = FileBrowser.SaveFile("保存文件", "", "批量导入试题模板", "xls");
                if (!string.IsNullOrEmpty(path))
                {
                    string sourcePath = Application.streamingAssetsPath + questionTemplate;
                    string targetPath = path;
                    bool isrewrite = true; // true=覆盖已存在的同名文件,false则反之
                    System.IO.File.Copy(sourcePath, targetPath, isrewrite);
                    Debug.LogFormat("下载试题模板成功.");
                }
#endif
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("下载试题模板时，出现异常：{0}", e.Message);
            }
        }

        private void dropdownBank_onValueChanged(int arg0)
        {

        }

        /// <summary>
        /// 选择文件导入的试题
        /// </summary>
        List<Question> questionList = new List<Question>();
        List<string> invalids = new List<string>();//无效

        /// <summary>
        /// 选择文件
        /// </summary>
        private void buttonSelect_onClick()
        {
            try
            {
                questionList.Clear();//清空
                invalids.Clear();

#if UNITY_WEBGL && !UNITY_EDITOR
                WebGLUtils.UploadFile(gameObject.name, "OnUploadFile", ".xls", false);
#else
                var extensionList = new[] {
                    new ExtensionFilter("excel 97-2003 工作簿(*.xls)", "xls"),
                };
                // 选择文件
                string path = FileBrowser.OpenSingleFile("选择文件", "", extensionList);
                if (!string.IsNullOrEmpty(path))
                {
                    fileUrl.text = System.IO.Path.GetFileName(path);

                    using (FileStream fs = System.IO.File.OpenRead(path))
                    {
                        LoadFromSteam(fs);
                    }
                }
#endif
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("批量导入试题时，出现异常：{0}", e.Message);
            }
        }

        public void OnUploadFile(string url)
        {
            StartCoroutine(LoadFromUrl(url));
        }

        public IEnumerator LoadFromUrl(string url)
        {
            fileUrl.text = "临时.xls";
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();

            if (request.isHttpError || request.isNetworkError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                try
                {
                    Debug.Log("LoadFromUrl : " + request.downloadHandler.text);
                    using (MemoryStream stream = new MemoryStream(request.downloadHandler.data))
                    {
                        LoadFromSteam(stream);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogErrorFormat("批量导入试题时，出现异常：{0}", e.Message);
                }
            }
        }

        public void LoadFromSteam(Stream stream)
        {
            HSSFWorkbook wk = new HSSFWorkbook(stream);
            ISheet sheet = wk.GetSheetAt(0);//读取当前表格
            for (int i = 1; i <= sheet.LastRowNum; i++)  //LastRowNum 是当前表的总行数
            {
                IRow row = sheet.GetRow(i); //读取当前行数据
                if (row != null)
                {
                    Question question = new Question();
                    question.Level = 2;
                    question.From = "批量导入";
                    question.Status = 1;
                    question.Content = row.GetCell(2) == null ? "" : row.GetCell(2).ToString();
                    question.Resolve = row.GetCell(5) == null ? "" : row.GetCell(5).ToString();
                    string id = row.GetCell(0) == null ? "" : row.GetCell(0).ToString();
                    //题目类型
                    string type = row.GetCell(1) == null ? "" : row.GetCell(1).ToString();
                    string cell_3 = row.GetCell(3) == null ? "" : row.GetCell(3).ToString();//选项
                    string cell_4 = row.GetCell(4) == null ? "" : row.GetCell(4).ToString();//答案

                    // 题目内容为空 或 题目类型为空
                    if (string.IsNullOrEmpty(question.Content) || string.IsNullOrEmpty(type))
                    {
                        continue;
                    }

                    switch (type)
                    {
                        case "单选题":
                            question.Type = 1;
                            question.Key = cell_4;
                            List<Option> options_1 = BuildOptions(cell_3);
                            question.Data = JsonConvert.SerializeObject(options_1);//将单选题选项序列化成json字符串
                                                                                   //判断是否添加
                            if (options_1.Count >= 3 && Regex.IsMatch(question.Key, "^[A-I]+$") && question.Key.Length == 1)
                            {
                                questionList.Add(question);
                            }
                            else
                            {
                                Debug.LogWarningFormat("添加单选题失败，序号：{0}", id);
                                invalids.Add(id);
                            }
                            break;
                        case "多选题":
                            question.Type = 2;
                            question.Key = cell_4;
                            List<Option> options_2 = BuildOptions(cell_3);
                            question.Data = JsonConvert.SerializeObject(BuildOptions(cell_3));//将单选题选项序列化成json字符串
                                                                                              //判断是否添加
                            if (options_2.Count >= 3 && Regex.IsMatch(question.Key, "^[A-I]+$"))
                            {
                                questionList.Add(question);
                            }
                            else
                            {
                                Debug.LogWarningFormat("添加多选题失败，序号：{0}", id);
                                invalids.Add(id);
                            }
                            break;
                        case "判断题":
                            question.Type = 3;
                            question.Data = "";
                            if (cell_4.Equals("正确"))
                            {
                                question.Key = "Y";
                                questionList.Add(question);
                            }
                            else if (cell_4.Equals("错误"))
                            {
                                question.Key = "N";
                                questionList.Add(question);
                            }
                            else
                            {
                                Debug.LogWarningFormat("添加判断题失败，序号：{0}", id);
                                invalids.Add(id);
                            }
                            break;
                        case "填空题":
                            question.Type = 4;
                            List<QBlank> blanks = BuildBlanks(cell_4);
                            question.Key = "";
                            for (int j = 0; j < blanks.Count; j++)
                            {
                                QBlank blank = blanks[j];
                                question.Key += blank.Value;
                                if (j < blanks.Count - 1)
                                {
                                    question.Key += ",";
                                }
                            }

                            QBlankFillData jsonData = new QBlankFillData();
                            jsonData.IsComplex = false;
                            jsonData.Blanks = blanks;
                            question.Data = JsonConvert.SerializeObject(jsonData);

                            if (blanks.Count > 0)
                            {
                                questionList.Add(question);
                            }
                            else
                            {
                                Debug.LogWarningFormat("添加填空题失败，序号：{0}", id);
                                invalids.Add(id);
                            }
                            break;
                        case "名词解释":
                            question.Type = 5;
                            question.Key = cell_4;
                            question.Data = "";
                            questionList.Add(question);
                            break;
                        case "简答题":
                            question.Type = 6;
                            question.Key = cell_4;
                            question.Data = "";
                            questionList.Add(question);
                            break;
                        case "操作题":
                            question.Type = 7;
                            question.Key = "";
                            question.Data = cell_3;
                            questionList.Add(question);
                            break;
                        default:
                            break;
                    }
                }
            }


            if (invalids.Count > 0)
            {
                string text = string.Format("经过检测，文件中[{0}]道试题不符合规范。", invalids.Count);
                fileWarningText.text = text;

                foreach (string item in invalids)
                {
                    Debug.Log(item);
                }
            }
        }

        /// <summary>
        /// 导入试题
        /// </summary>
        private void submitBar_OnSubmit()
        {
            if (Validate() && questionList.Count > 0)
            {
                QuestionBank questionBank = m_QuestionBanks[dropdownBank.value - 1];
                string bankId = questionBank.Id;

                foreach (var item in questionList)
                {
                    item.BankId = bankId;
                }

                Debug.Log(questionList.Count);

                questionModule.BatchInsertQuestion(questionList, ReceiveBatchInsertQuestionResp);
            }
        }

        private void submitBar_OnCancel()
        {

        }

        /// <summary>
        /// 接受获取所有试题库的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveListQuestionBankByConditionResp(NetworkPackageInfo packageInfo)
        {
            ListQuestionBankByConditionResp resp = ListQuestionBankByConditionResp.Parser.ParseFrom(packageInfo.Body);
            List<QuestionBank> questionBanks = new List<QuestionBank>();
            for (int i = 0; i < resp.QuestionBanks.Count; i++)
            {
                QuestionBankProto questionBankProto = resp.QuestionBanks[i];
                //questionBank
                QuestionBank questionBank = new QuestionBank();
                questionBank.Id = questionBankProto.Id;
                questionBank.Name = questionBankProto.Name;
                questionBank.Status = questionBankProto.Status;
                questionBank.Poster = questionBankProto.Poster;
                questionBank.CreateTime = Converter.NewDateTime(questionBankProto.CreateTime);
                questionBank.Modifier = questionBankProto.Modifier;
                questionBank.UpdateTime = Converter.NewDateTime(questionBankProto.UpdateTime);
                questionBank.Remark = questionBankProto.Remark;
                questionBanks.Add(questionBank);
            }
            //设置试题库下拉框
            SetQBank(questionBanks);
            m_QuestionBanks = questionBanks;
        }

        /// <summary>
        /// 设置试题库Dropdown
        /// </summary>
        /// <param name="questionBanks"></param>
        public void SetQBank(List<QuestionBank> questionBanks)
        {
            if (questionBanks != null && questionBanks.Count > 0)
            {
                m_QuestionBanks = questionBanks;

                List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();
                optionDatas.Add(new Dropdown.OptionData("所属题库"));
                for (int i = 0; i < m_QuestionBanks.Count; i++)
                {
                    QuestionBank questionBank = m_QuestionBanks[i];
                    Dropdown.OptionData optionData = new Dropdown.OptionData(questionBank.Name);
                    optionDatas.Add(optionData);
                }

                dropdownBank.options.Clear();
                dropdownBank.options = optionDatas;

                //设置默认值
                dropdownBank.value = 0;
            }
        }

        /// <summary>
        /// 构建选项
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private List<Option> BuildOptions(string column)
        {
            List<Option> options = new List<Option>();

            Regex reg = new Regex(@"\{(.+?)}");//{}
            List<string> optionStrs = reg.Matches(column).Cast<Match>().Select(m => m.Value).ToList();
            for (int j = 0; j < optionStrs.Count; j++)
            {
                string optionStr = optionStrs[j];
                Option option = new Option();
                string alisa = optionStr.Substring(1, 1);//A B C D E F G H I
                if (alisa.Equals(AlisaMap[j + 1]))//是否在选项之内
                {
                    string part = optionStr.Substring(2, 1);//:
                    if (part.Equals(":"))
                    {
                        string text = optionStr.Substring(3, optionStr.Length - 4);
                        option.Alisa = alisa;
                        option.Text = text;
                        options.Add(option);
                    }
                }
            }

            return options;
        }

        /// <summary>
        /// 构建填空
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private List<QBlank> BuildBlanks(string column)
        {
            List<QBlank> blanks = new List<QBlank>();

            Regex reg = new Regex(@"\{(.+?)}");//{}
            List<string> blankStrs = reg.Matches(column).Cast<Match>().Select(m => m.Value).ToList();
            for (int i = 0; i < blankStrs.Count; i++)
            {
                string blankStr = blankStrs[i];
                QBlank blank = new QBlank();
                string str = blankStr.Substring(1, 6);
                if (str.Equals(BLANK + (i + 1)))
                {
                    string part = blankStr.Substring(7, 1);//:
                    if (part.Equals(":"))
                    {
                        string value = blankStr.Substring(8, blankStr.Length - 9);
                        blank.Id = i + 1;
                        blank.Name = BLANK + (i + 1);
                        blank.Value = value;
                        blanks.Add(blank);
                    }
                }
            }

            return blanks;
        }

        /// <summary>
        /// 校验
        /// </summary>
        /// <returns></returns>
        private bool Validate()
        {
            bool result = true;
            bankWarningText.text = "";
            fileWarningText.text = "";

            if (dropdownBank.value == 0)
            {
                bankWarningText.text = "*请选择一个试题库*";
                result = false;
            }

            if (!fileUrl.text.Contains(".xls"))
            {
                fileWarningText.text = "*请选择一个文件*";
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 接受批量导入试题的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveBatchInsertQuestionResp(NetworkPackageInfo packageInfo)
        {
            BatchInsertQuestionResp resp = BatchInsertQuestionResp.Parser.ParseFrom(packageInfo.Body);
            string json = resp.BatchResult;
            PanelManager.Instance.OpenPanel(PanelContainerType.PopupPanelContainer, EnumPanelType.BatchResultDetailPanel, json);
        }

    }
}
