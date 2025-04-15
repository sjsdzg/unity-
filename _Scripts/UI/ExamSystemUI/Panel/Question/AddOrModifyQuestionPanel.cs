using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Core;
using XFramework.Module;
using XFramework.Proto;
using Newtonsoft.Json;
using XFramework.Network;
using XFramework.Common;

namespace XFramework.UI
{
    public class AddOrModifyQuestionPanel : AbstractPanel
    {
        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.AddOrModifyQuestionPanel;
        }

        /// <summary>
        /// 地址栏
        /// </summary>
        protected AddressBar addressBar;

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

        /// <summary>
        /// 题目基础栏
        /// </summary>
        private QuestionBasicBar questionBasicBar;

        /// <summary>
        /// 单选题选项栏
        /// </summary>
        private SingleChoiceOptionBar singleChoiceOptionBar;

        /// <summary>
        /// 多选题选项栏
        /// </summary>
        private MultipleChoiceOptionBar multipleChoiceOptionBar;

        /// <summary>
        /// 判断题选项栏
        /// </summary>
        private JudgmentOptionBar judgmentOptionBar;

        /// <summary>
        /// 填空选项栏
        /// </summary>
        private BlankFillOptionBar blankFillOptionBar;

        /// <summary>
        /// 名称解释答案栏
        /// </summary>
        private ExplainKeyBar explainKeyBar;

        /// <summary>
        /// 简答题答案栏
        /// </summary>
        private EssayKeyBar essayKeyBar;

        /// <summary>
        /// 操作题链接栏
        /// </summary>
        private OperationLinkBar operationLinkBar;

        /// <summary>
        /// 题型对应的Bar
        /// </summary>
        private Dictionary<int, GameObject> qTypeBarMap = new Dictionary<int, GameObject>();

        /// <summary>
        /// 题目描述栏
        /// </summary>
        private QuestionContentBar questionContentBar;

        /// <summary>
        /// 题目解析栏
        /// </summary>
        private QuestionResolveBar questionResolveBar;

        /// <summary>
        /// 试题提交栏
        /// </summary>
        private QuestionSubmitBar questionSubmitBar;

        /// <summary>
        /// 操作方式
        /// </summary>
        private OperationType operationType = OperationType.Add;

        protected override void OnAwake()
        {
            base.OnAwake();
            InitGUI();
            InitEvent();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
        }

        private void InitGUI()
        {
            addressBar = transform.Find("AddressBar").GetComponent<AddressBar>();
            questionBasicBar = transform.Find("QuestionBasicBar").GetComponent<QuestionBasicBar>();

            singleChoiceOptionBar = transform.Find("SingleChoiceOptionBar").GetComponent<SingleChoiceOptionBar>();
            multipleChoiceOptionBar = transform.Find("MultipleChoiceOptionBar").GetComponent<MultipleChoiceOptionBar>();
            judgmentOptionBar = transform.Find("JudgmentOptionBar").GetComponent<JudgmentOptionBar>();
            blankFillOptionBar = transform.Find("BlankFillOptionBar").GetComponent<BlankFillOptionBar>();
            explainKeyBar = transform.Find("ExplainKeyBar").GetComponent<ExplainKeyBar>();
            essayKeyBar = transform.Find("EssayKeyBar").GetComponent<EssayKeyBar>();
            operationLinkBar = transform.Find("OperationLinkBar").GetComponent<OperationLinkBar>();

            qTypeBarMap.Add(ExamSystemConstants.QuestionType.SINGLE_CHOICE, singleChoiceOptionBar.gameObject);
            qTypeBarMap.Add(ExamSystemConstants.QuestionType.MULTIPLE_CHOICE, multipleChoiceOptionBar.gameObject);
            qTypeBarMap.Add(ExamSystemConstants.QuestionType.JUDGMENT, judgmentOptionBar.gameObject);
            qTypeBarMap.Add(ExamSystemConstants.QuestionType.BLANK_FILL, blankFillOptionBar.gameObject);
            qTypeBarMap.Add(ExamSystemConstants.QuestionType.EXPLAIN, explainKeyBar.gameObject);
            qTypeBarMap.Add(ExamSystemConstants.QuestionType.ESSAY, essayKeyBar.gameObject);
            qTypeBarMap.Add(ExamSystemConstants.QuestionType.OPERATION, operationLinkBar.gameObject);

            questionContentBar = transform.Find("QuestionContentBar").GetComponent<QuestionContentBar>();
            questionResolveBar = transform.Find("QuestionResolveBar").GetComponent<QuestionResolveBar>();
            questionSubmitBar = transform.Find("QuestionSubmitBar").GetComponent<QuestionSubmitBar>();
        }

        private void InitEvent()
        {
            addressBar.OnClicked.AddListener(addressBar_OnClicked);
            questionBasicBar.QTypeChanged.AddListener(questionBasicBar_QTypeChanged);
            blankFillOptionBar.OnAdded.AddListener(blankFillOptionBar_OnAdded);
            blankFillOptionBar.OnRemoved.AddListener(blankFillOptionBar_OnRemoved);
            questionSubmitBar.OnSubmit.AddListener(questionSumbitBar_OnSumbit);
            questionSubmitBar.OnCancel.AddListener(questionSumbitBar_OnCancel);
        }

        protected override void OnStart()
        {
            base.OnStart();

            singleChoiceOptionBar.gameObject.SetActive(true);
            multipleChoiceOptionBar.gameObject.SetActive(false);
            judgmentOptionBar.gameObject.SetActive(false);
            blankFillOptionBar.gameObject.SetActive(false);
            explainKeyBar.gameObject.SetActive(false);
            essayKeyBar.gameObject.SetActive(false);

            questionModule = ModuleManager.Instance.GetModule<QuestionModule>();
            questionBankModule = ModuleManager.Instance.GetModule<QuestionBankModule>();
            //先获取所有试题库
            questionBankModule.ListQuestionBankByCondition(SqlCondition.ListBySoftwareId(), ReceiveListQuestionBankByConditionResp);
        }

        protected override void SetPanel(params object[] PanelParams)
        {
            base.SetPanel(PanelParams);
            addressBar.AddHyperButton(EnumPanelType.ExamHomePanel, PanelDefine.GetPanelComment(EnumPanelType.ExamHomePanel));

            if (PanelParams.Length > 0 && PanelParams[0] != null)
            {
                operationType = (OperationType)PanelParams[0];
                if (operationType == OperationType.Modify)
                {
                    addressBar.AddHyperButton(EnumPanelType.AddOrModifyQuestionPanel, "修改试题");
                    Question question = PanelParams[1] as Question;


                    InitModifyContent(question);
                }
            }
            else
            {
                operationType = OperationType.Add;
                addressBar.AddHyperButton(EnumPanelType.AddOrModifyQuestionPanel, "添加试题");
            }
        }

        /// <summary>
        /// 地址栏点击时，触发
        /// </summary>
        /// <param name="type"></param>
        private void addressBar_OnClicked(EnumPanelType type)
        {
            Release();
            PanelManager.Instance.OpenPanelCloseOthers(Parent, type);
        }

        /// <summary>
        /// 题型选择改变时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void questionBasicBar_QTypeChanged(int oldType, int newType)
        {
            if (!qTypeBarMap.ContainsKey(newType))
                return;

            questionContentBar.Clear();
            questionResolveBar.Clear();

            IClear clear = qTypeBarMap[oldType].GetComponent<IClear>();
            if (clear != null)
            {
                clear.Clear();
            }

            foreach (var key in qTypeBarMap.Keys)
            {
                if (key == newType)
                {
                    qTypeBarMap[key].SetActive(true);
                }
                else
                {
                    qTypeBarMap[key].SetActive(false);
                }
            }
        }

        /// <summary>
        /// 添加填空时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void blankFillOptionBar_OnAdded(int arg0)
        {
            questionContentBar.AddBlank(arg0);
        }

        /// <summary>
        /// 移除填空时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void blankFillOptionBar_OnRemoved(int arg0)
        {
            questionContentBar.RemoveBlank(arg0);
        }

        /// <summary>
        /// 修改题目
        /// </summary>
        private Question modifyQuestion;

        /// <summary>
        /// 初始化修改内容
        /// </summary>
        /// <param name="question"></param>
        private void InitModifyContent(Question question)
        {
            modifyQuestion = question;

            questionBasicBar.QType = question.Type;
            questionBasicBar.QBankId = question.BankId;
            questionBasicBar.QLevel = question.Level;
            questionBasicBar.QStatus = question.Status;
            questionBasicBar.QFrom = question.From;
            questionContentBar.QContent = question.Content;
            questionResolveBar.QResolve = question.Resolve;

            switch (question.Type)
            {
                case ExamSystemConstants.QuestionType.SINGLE_CHOICE:
                    singleChoiceOptionBar.OptionList = JsonConvert.DeserializeObject<List<Option>>(question.Data);
                    singleChoiceOptionBar.Key = question.Key;
                    break;
                case ExamSystemConstants.QuestionType.MULTIPLE_CHOICE:
                    multipleChoiceOptionBar.OptionList = JsonConvert.DeserializeObject<List<Option>>(question.Data);
                    multipleChoiceOptionBar.Key = question.Key;
                    break;
                case ExamSystemConstants.QuestionType.JUDGMENT:
                    judgmentOptionBar.Key = question.Key;
                    break;
                case ExamSystemConstants.QuestionType.BLANK_FILL:
                    QBlankFillData data = JsonConvert.DeserializeObject<QBlankFillData> (question.Data);
                    blankFillOptionBar.BlankList = data.Blanks;
                    blankFillOptionBar.IsComplex = data.IsComplex;
                    blankFillOptionBar.Key = question.Key;
                    break;
                case ExamSystemConstants.QuestionType.EXPLAIN:
                    explainKeyBar.Key = question.Key;
                    break;
                case ExamSystemConstants.QuestionType.ESSAY:
                    essayKeyBar.Key = question.Key;
                    break;
                case ExamSystemConstants.QuestionType.OPERATION:
                    operationLinkBar.Link = question.Data;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 点击提交时，触发
        /// </summary>
        private void questionSumbitBar_OnSumbit()
        {
            int currentQType = questionBasicBar.QType;
            IValidate validateKey = qTypeBarMap[currentQType].GetComponent<IValidate>();
            bool basic = questionBasicBar.Validate();
            bool content = questionContentBar.Validate();
            bool key = true;
            if (validateKey != null)
            {
                key = validateKey.Validate();
            }

            if (basic && content && key)
            {
                Question question = new Question();
                question.Type = questionBasicBar.QType;
                question.BankId = questionBasicBar.QBankId;
                question.Level = questionBasicBar.QLevel;
                question.Status = questionBasicBar.QStatus;
                question.From = questionBasicBar.QFrom;
                question.Content = questionContentBar.QContent;
                question.Resolve = questionResolveBar.QResolve;

                switch (currentQType)
                {
                    case ExamSystemConstants.QuestionType.SINGLE_CHOICE:
                        question.Key = singleChoiceOptionBar.Key;
                        question.Data = JsonConvert.SerializeObject(singleChoiceOptionBar.OptionList);
                        break;
                    case ExamSystemConstants.QuestionType.MULTIPLE_CHOICE:
                        question.Key = multipleChoiceOptionBar.Key;
                        question.Data = JsonConvert.SerializeObject(multipleChoiceOptionBar.OptionList);
                        break;
                    case ExamSystemConstants.QuestionType.JUDGMENT:
                        question.Key = judgmentOptionBar.Key;
                        question.Data = "";
                        break;
                    case ExamSystemConstants.QuestionType.BLANK_FILL:
                        question.Key = blankFillOptionBar.Key;
                        QBlankFillData data = new QBlankFillData();
                        data.Blanks = blankFillOptionBar.BlankList;
                        data.IsComplex = blankFillOptionBar.IsComplex;
                        question.Data = JsonConvert.SerializeObject(data);
                        break;
                    case ExamSystemConstants.QuestionType.EXPLAIN:
                        question.Key = explainKeyBar.Key;
                        question.Data = "";
                        break;
                    case ExamSystemConstants.QuestionType.ESSAY:
                        question.Key = essayKeyBar.Key;
                        question.Data = "";
                        break;
                    case ExamSystemConstants.QuestionType.OPERATION:
                        question.Key = "";
                        question.Data = operationLinkBar.Link;
                        break;
                    default:
                        break;
                }

                switch (operationType)
                {
                    case OperationType.Add:
                        questionModule.InsertQuestion(question, ReceiveInsertQuestionResp);
                        break;
                    case OperationType.Modify:
                        question.Id = modifyQuestion.Id;
                        question.Poster = modifyQuestion.Poster;
                        questionModule.UpdateQuestion(question, ReceiveUpdateQuestionResp);
                        break;
                    default:
                        break;
                }
                
            }

        }

        /// <summary>
        /// 点击取消时，触发
        /// </summary>
        private void questionSumbitBar_OnCancel()
        {
            PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.ManageQuestionPanel);
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
            questionBasicBar.SetQBank(questionBanks);
            m_QuestionBanks = questionBanks;
            //获取完题库之后, 分页查询试题。
        }

        /// <summary>
        /// 接受添加题目的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveInsertQuestionResp(NetworkPackageInfo packageInfo)
        {
            if (packageInfo.Header.Status == Status.OK)
            {
                MessageBoxEx.Show("试题添加成功。", "提示", MessageBoxExEnum.SingleDialog, null);
            }
        }


        private void ReceiveUpdateQuestionResp(NetworkPackageInfo packageInfo)
        {
            if (packageInfo.Header.Status == Status.OK)
            {
                MessageBoxEx.Show("试题修改成功", "提示", MessageBoxExEnum.SingleDialog, dialogResult =>
                {
                    PanelManager.Instance.OpenPanelCloseOthers(this.Parent, EnumPanelType.ManageQuestionPanel);
                });
            }
        }

    }
}
