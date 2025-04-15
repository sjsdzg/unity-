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
    public class ImprotUserPanel : AbstractPanel
    {
        public override EnumPanelType GetPanelType()
        {
            return EnumPanelType.ImprotUserPanel;
        }

        public const string userTemplate = "/Files/template/用户_批量导入模板.xls";


        /// <summary>
        /// 地址栏
        /// </summary>
        protected AddressBar addressBar;

        /// <summary>
        /// 模板下载按钮
        /// </summary>
        private Button buttonDownload;

        /// <summary>
        /// 专业Dropdown
        /// </summary>
        private Dropdown dropdownBranch;

        /// <summary>
        /// 年级Dropdown
        /// </summary>
        private Dropdown dropdownGrade;

        /// <summary>
        /// 班级Dropdown
        /// </summary>
        private Dropdown dropdownPosition;

        /// <summary>
        /// 角色Dropdown
        /// </summary>
        private Dropdown dropdownRole;

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
        /// 专业警告文本
        /// </summary>
        private Text branchWarningText;

        /// <summary>
        /// 年级警告文本
        /// </summary>
        private Text gradeWarningText;

        /// <summary>
        /// 班级警告文本
        /// </summary>
        private Text positionWarningText;

        /// <summary>
        /// 角色警告文本
        /// </summary>
        private Text roleWarningText;

        /// <summary>
        /// 文件警告文本
        /// </summary>
        private Text fileWarningText;

        /// <summary>
        /// 用户模块
        /// </summary>
        private UserModule userModule;

        /// <summary>
        /// 用户库模块
        /// </summary>
        private BranchModule branchModule;

        /// <summary>
        /// 班级模块
        /// </summary>
        private PositionModule positionModule;

        /// <summary>
        /// 用户角色模块
        /// </summary>
        private RoleModule roleModule;

        /// <summary>
        /// 用户库列表
        /// </summary>
        private List<Branch> m_Branchs;

        /// <summary>
        /// 班级列表
        /// </summary>
        private List<Position> m_Positions;

        /// <summary>
        /// 用户角色列表
        /// </summary>
        private List<Role> m_Roles;

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

        /// <summary>
        /// 初始化界面
        /// </summary>
        public void InitGUI()
        {
            addressBar = transform.Find("AddressBar").GetComponent<AddressBar>();
            buttonDownload = transform.Find("Panel/GuideSecond/ButtonDownload").GetComponent<Button>();

            dropdownBranch = transform.Find("Panel/Branch/Dropdown").GetComponent<Dropdown>();
            dropdownGrade = transform.Find("Panel/Grade/Dropdown").GetComponent<Dropdown>();
            dropdownPosition = transform.Find("Panel/Position/Dropdown").GetComponent<Dropdown>();
            dropdownRole = transform.Find("Panel/Role/Dropdown").GetComponent<Dropdown>();

            fileUrl = transform.Find("Panel/File/FileUrl").GetComponent<Text>();
            buttonSelect = transform.Find("Panel/File/ButtonSelect").GetComponent<Button>();

            branchWarningText = transform.Find("Panel/Branch/WarningText").GetComponent<Text>();
            gradeWarningText = transform.Find("Panel/Grade/WarningText").GetComponent<Text>();
            positionWarningText = transform.Find("Panel/Position/WarningText").GetComponent<Text>();
            roleWarningText = transform.Find("Panel/Role/WarningText").GetComponent<Text>();
            fileWarningText = transform.Find("Panel/File/WarningText").GetComponent<Text>();

            submitBar = transform.Find("SubmitBar").GetComponent<SubmitBar>();
        }

        /// <summary>
        /// 初始化事件
        /// </summary>
        public void InitEvent()
        {
            addressBar.OnClicked.AddListener(addressBar_OnClicked);
            buttonDownload.onClick.AddListener(buttonDownload_onClick);
            buttonSelect.onClick.AddListener(buttonSelect_onClick);
            submitBar.OnSubmit.AddListener(submitBar_OnSubmit);
            submitBar.OnCancel.AddListener(submitBar_OnCancel);
        }

        protected override void OnStart()
        {
            base.OnStart();
            userModule = ModuleManager.Instance.GetModule<UserModule>();
            branchModule = ModuleManager.Instance.GetModule<BranchModule>();
            positionModule = ModuleManager.Instance.GetModule<PositionModule>();
            roleModule = ModuleManager.Instance.GetModule<RoleModule>();
            //先获取所有用户库
            SetGrade();
            branchModule.ListAllBranch(ReceiveListAllBranchResp);
            positionModule.ListAllPosition(ReceiveListAllPositionResp);
            roleModule.ListAllRole(ReceiveListAllRoleResp);
        }

        private void addressBar_OnClicked(EnumPanelType type)
        {
            Release();
            PanelManager.Instance.OpenPanelCloseOthers(Parent, type);
        }

        protected override void SetPanel(params object[] PanelParams)
        {
            base.SetPanel(PanelParams);
            addressBar.AddHyperButton(EnumPanelType.SystemHomePanel, PanelDefine.GetPanelComment(EnumPanelType.SystemHomePanel));
            addressBar.AddHyperButton(EnumPanelType.ImprotUserPanel, PanelDefine.GetPanelComment(EnumPanelType.ImprotUserPanel));
        }

        /// <summary>
        /// 下载用户模板
        /// </summary>
        private void buttonDownload_onClick()
        {
            try
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                WebGLUtils.OpenUrl(AppSettings.Settings.AssetServerUrl + userTemplate);
#else
                string path = FileBrowser.SaveFile("保存文件", "", "用户_批量导入模板", "xls");
                if (!string.IsNullOrEmpty(path))
                {
                    string sourcePath = Application.streamingAssetsPath + userTemplate;
                    string targetPath = path;
                    bool isrewrite = true; // true=覆盖已存在的同名文件,false则反之
                    System.IO.File.Copy(sourcePath, targetPath, isrewrite);
                    Debug.LogFormat("下载用户模板成功.");
                }
#endif
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("下载用户模板时，出现异常：{0}", e.Message);
            }
        }

        /// <summary>
        /// 选择文件导入的用户
        /// </summary>
        List<User> userList = new List<User>();
        List<string> invalids = new List<string>();//无效

        /// <summary>
        /// 选择文件
        /// </summary>
        private void buttonSelect_onClick()
        {
            try
            {
                userList.Clear();//清空
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
                Debug.LogErrorFormat("批量导入用户时，出现异常：{0}", e.Message);
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
                    Debug.LogErrorFormat("批量导入用户时，出现异常：{0}", e.Message);
                }
            }
        }

        public void LoadFromSteam(Stream stream)
        {
            HSSFWorkbook wk = new HSSFWorkbook(stream);
            Debug.Log("HSSFWorkbook wk = new HSSFWorkbook(stream);");

            ISheet sheet = wk.GetSheetAt(0);//读取当前表格
            for (int i = 1; i <= sheet.LastRowNum; i++)  //LastRowNum 是当前表的总行数
            {
                IRow row = sheet.GetRow(i); //读取当前行数据
                if (row != null)
                {
                    User user = new User();
                    string id = row.GetCell(0) == null ? "" : row.GetCell(0).ToString();
                    //题目类型
                    user.UserName = row.GetCell(1) == null ? "" : row.GetCell(1).ToString();

                    // 用户名称为空
                    if (string.IsNullOrEmpty(user.UserName))
                    {
                        continue;
                    }

                    user.UserPassword = row.GetCell(2) == null ? "" : row.GetCell(2).ToString();
                    user.RealName = row.GetCell(3) == null ? "" : row.GetCell(3).ToString();
                    string cell_4 = row.GetCell(4) == null ? "" : row.GetCell(4).ToString();

                    user.Remark = row.GetCell(5) == null ? "" : row.GetCell(5).ToString();
                    //添加
                    user.UserNo = "";
                    user.Phone = "";
                    user.Email = "";
                    user.Status = 1;
                    user.Modifier = "";

                    if (cell_4.Equals("男"))
                    {
                        user.Sex = 0;
                        userList.Add(user);
                    }
                    else if (cell_4.Equals("女"))
                    {
                        user.Sex = 1;
                        userList.Add(user);
                    }
                    else //默认男
                    {
                        user.Sex = 0;
                        userList.Add(user);
                    }
                }
            }

            if (invalids.Count > 0)
            {
                string text = string.Format("经过检测，文件中[{0}]道用户不符合规范。", invalids.Count);
                fileWarningText.text = text;

                foreach (string item in invalids)
                {
                    Debug.Log(item);
                }
            }
        }


        /// <summary>
        /// 导入用户
        /// </summary>
        private void submitBar_OnSubmit()
        {
            if (Validate() && userList.Count > 0)
            {

                string branchId = "";

                if (dropdownBranch.value > 0)
                {
                    Branch branch = m_Branchs[dropdownBranch.value - 1];
                    branchId = branch.Id;
                }

                string positionId = "";
                if (dropdownPosition.value > 0)
                {
                    Position position = m_Positions[dropdownPosition.value - 1];
                    positionId = position.Id;
                }

                Role role = m_Roles[dropdownRole.value - 1];
                string roleId = role.Id;

                string grade = dropdownGrade.options[dropdownGrade.value].text;

                foreach (var item in userList)
                {
                    item.BranchId = branchId;
                    item.Grade = grade;
                    item.PositionId = positionId;
                    item.RoleId = roleId;
                }

                Debug.Log(userList.Count);

                userModule.BatchInsertUser(userList, ReceiveBatchInsertUserResp);
            }
        }

        private void submitBar_OnCancel()
        {

        }

        /// <summary>
        /// 接受获取所有用户库的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveListAllBranchResp(NetworkPackageInfo packageInfo)
        {
            ListAllBranchResp resp = ListAllBranchResp.Parser.ParseFrom(packageInfo.Body);
            List<Branch> branchs = new List<Branch>();
            for (int i = 0; i < resp.Branchs.Count; i++)
            {
                BranchProto branchProto = resp.Branchs[i];
                //branch
                Branch branch = new Branch();
                branch.Id = branchProto.Id;
                branch.Name = branchProto.Name;
                branch.Status = branchProto.Status;
                branch.Poster = branchProto.Poster;
                branch.CreateTime = Converter.NewDateTime(branchProto.CreateTime);
                branch.Modifier = branchProto.Modifier;
                branch.UpdateTime = Converter.NewDateTime(branchProto.UpdateTime);
                branch.Remark = branchProto.Remark;
                branchs.Add(branch);
            }
            //设置用户库下拉框
            SetBranch(branchs);
            m_Branchs = branchs;
        }

        /// <summary>
        /// 接受获取所有班级的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveListAllPositionResp(NetworkPackageInfo packageInfo)
        {
            ListAllPositionResp resp = ListAllPositionResp.Parser.ParseFrom(packageInfo.Body);
            List<Position> positions = new List<Position>();
            for (int i = 0; i < resp.Positions.Count; i++)
            {
                PositionProto branchProto = resp.Positions[i];
                //branch
                Position branch = new Position();
                branch.Id = branchProto.Id;
                branch.Name = branchProto.Name;
                branch.Status = branchProto.Status;
                branch.Poster = branchProto.Poster;
                branch.CreateTime = Converter.NewDateTime(branchProto.CreateTime);
                branch.Modifier = branchProto.Modifier;
                branch.UpdateTime = Converter.NewDateTime(branchProto.UpdateTime);
                branch.Remark = branchProto.Remark;
                positions.Add(branch);
            }
            //设置用户库下拉框
            SetPosition(positions);
            m_Positions = positions;
        }

        private void ReceiveListAllRoleResp(NetworkPackageInfo packageInfo)
        {
            ListAllRoleResp resp = ListAllRoleResp.Parser.ParseFrom(packageInfo.Body);
            List<Role> roles = new List<Role>();
            for (int i = 0; i < resp.Roles.Count; i++)
            {
                RoleProto roleProto = resp.Roles[i];
                //branch
                Role role = new Role();
                role.Id = roleProto.Id;
                role.Name = roleProto.Name;
                role.Status = roleProto.Status;
                role.Privilege = roleProto.Privilege;
                role.Poster = roleProto.Poster;
                role.CreateTime = Converter.NewDateTime(roleProto.CreateTime);
                role.Modifier = roleProto.Modifier;
                role.UpdateTime = Converter.NewDateTime(roleProto.UpdateTime);
                role.Remark = roleProto.Remark;
                roles.Add(role);
            }
            //设置用户库下拉框
            SetRoles(roles);
            m_Roles = roles;
        }

        /// <summary>
        /// 设置用户库Dropdown
        /// </summary>
        /// <param name="branchs"></param>
        public void SetBranch(List<Branch> branchs)
        {
            if (branchs != null && branchs.Count > 0)
            {
                m_Branchs = branchs;

                List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();
                optionDatas.Add(new Dropdown.OptionData("请选择"));
                for (int i = 0; i < m_Branchs.Count; i++)
                {
                    Branch branch = m_Branchs[i];
                    Dropdown.OptionData optionData = new Dropdown.OptionData(branch.Name);
                    optionDatas.Add(optionData);
                }

                dropdownBranch.options.Clear();
                dropdownBranch.options = optionDatas;

                //设置默认值
                dropdownBranch.value = 0;
            }
        }

        private void SetGrade()
        {
            //初始化年级
            List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();
            optionDatas.Add(new Dropdown.OptionData("请选择"));
            DateTime now = DateTime.Now;
            for (int i = 0; i < 6; i++)
            {
                string name = now.AddYears(-i).ToString("yyyy") + "级";
                Dropdown.OptionData optionData = new Dropdown.OptionData(name);
                optionDatas.Add(optionData);
            }
            dropdownGrade.options.Clear();
            dropdownGrade.options = optionDatas;
            dropdownGrade.value = 0;

        }

        /// <summary>
        /// 设置班级Dropdown
        /// </summary>
        /// <param name="questionBanks"></param>
        public void SetPosition(List<Position> positions)
        {
            m_Positions = positions;

            List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();
            optionDatas.Add(new Dropdown.OptionData("请选择"));
            for (int i = 0; i < m_Positions.Count; i++)
            {
                Position position = m_Positions[i];
                Dropdown.OptionData optionData = new Dropdown.OptionData(position.Name);
                optionDatas.Add(optionData);
            }
            dropdownPosition.options.Clear();
            dropdownPosition.options = optionDatas;

            dropdownPosition.value = 0;
        }

        /// <summary>
        /// 设置角色Dropdown
        /// </summary>
        /// <param name="questionBanks"></param>
        public void SetRoles(List<Role> userRoles)
        {
            m_Roles = userRoles;
            List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();
            optionDatas.Add(new Dropdown.OptionData("请选择"));
            for (int i = 0; i < m_Roles.Count; i++)
            {
                Role userRole = m_Roles[i];
                Dropdown.OptionData optionData = new Dropdown.OptionData(userRole.Name);
                optionDatas.Add(optionData);
            }
            dropdownRole.options.Clear();
            dropdownRole.options = optionDatas;
            dropdownRole.value = 0;
        }



        /// <summary>
        /// 校验
        /// </summary>
        /// <returns></returns>
        private bool Validate()
        {
            bool result = true;
            branchWarningText.text = "";
            gradeWarningText.text = "";
            positionWarningText.text = "";
            fileWarningText.text = "";

            if (dropdownRole.value == 0)
            {
                roleWarningText.text = "*请选择一个角色*";
                result = false;
            }

            //if (dropdownBranch.value == 0)
            //{
            //    branchWarningText.text = "*请选择一个专业*";
            //    result = false;
            //}

            //if (dropdownGrade.value == 0)
            //{
            //    gradeWarningText.text = "*请选择一个年级*";
            //    result = false;
            //}

            //if (dropdownPosition.value == 0)
            //{
            //    positionWarningText.text = "*请选择一个班级*";
            //    result = false;
            //}

            if (!fileUrl.text.Contains(".xls"))
            {
                fileWarningText.text = "*请选择一个文件*";
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 接受批量导入用户的响应
        /// </summary>
        /// <param name="packageInfo"></param>
        private void ReceiveBatchInsertUserResp(NetworkPackageInfo packageInfo)
        {
            BatchInsertUserResp resp = BatchInsertUserResp.Parser.ParseFrom(packageInfo.Body);
            string json = resp.BatchResult;
            PanelManager.Instance.OpenPanel(PanelContainerType.PopupPanelContainer, EnumPanelType.BatchResultDetailPanel, json);
        }

    }
}
