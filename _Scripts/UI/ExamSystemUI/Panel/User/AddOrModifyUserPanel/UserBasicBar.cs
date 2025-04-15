using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XFramework.Module;

namespace XFramework.UI
{
    public class UserBasicBar : MonoBehaviour, IValidate
    {
        private string userName;
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get
            {
                userName = inputFieldUserName.text;
                return userName;
            }
            set
            {
                userName = value;
                inputFieldUserName.text = userName;
            }
        }

        private string userPassword;
        /// <summary>
        /// 密码
        /// </summary>
        public string UserPassword
        {
            get
            {
                userPassword = inputFieldUserPassword.text;
                return userPassword;
            }
            set
            {
                userPassword = value;
                inputFieldUserPassword.text = userPassword;
            }
        }

        private string realName = "";
        /// <summary>
        /// 用户名
        /// </summary>
        public string RealName
        {
            get
            {
                realName = inputFieldRealName.text;
                return realName;
            }
            set
            {
                realName = value;
                inputFieldRealName.text = realName;
            }
        }

        private string remark = "";
        /// <summary>
        /// 用户名
        /// </summary>
        public string Remark
        {
            get
            {
                remark = inputFieldRemark.text;
                return remark;
            }
            set
            {
                remark = value;
                inputFieldRemark.text = remark;
            }
        }

        private int sex;
        /// <summary>
        /// 性别Id
        /// </summary>
        public int Sex
        {
            get
            {
                sex = dropdownSex.value;
                return sex;
            }
            set
            {
                sex = value;
                dropdownSex.value = sex;
            }
        }

        private string branchId = "";
        /// <summary>
        /// 用户分类Id
        /// </summary>
        public string BranchId
        {
            get
            {
                if (dropdownBranch.value > 0)
                {
                    Branch questionBank = m_Branchs[dropdownBranch.value - 1];
                    branchId = questionBank.Id;
                }
                return branchId;
            }
            set
            {
                branchId = value;
                if (m_Branchs != null)
                {
                    for (int i = 0; i < m_Branchs.Count; i++)
                    {
                        if (m_Branchs[i].Id == branchId)
                        {
                            dropdownBranch.value = i + 1;
                        }
                    }
                }
            }
        }

        private string grade = "";
        /// <summary>
        /// 用户分类Id
        /// </summary>
        public string Grade
        {
            get
            {
                if (dropdownGrade.value > 0)
                {
                    grade = dropdownGrade.options[dropdownGrade.value].text;
                }
                return grade;
            }
            set
            {
                grade = value;
                if (dropdownGrade.options != null)
                {
                    for (int i = 0; i < dropdownGrade.options.Count; i++)
                    {
                        if (dropdownGrade.options[i].text == grade)
                        {
                            dropdownGrade.value = i;
                        }
                    }
                }
            }
        }

        private string positionId = "";
        /// <summary>
        /// 用户分类Id
        /// </summary>
        public string PositionId
        {
            get
            {
                if (dropdownBranch.value > 0)
                {
                    Position position = m_Positions[dropdownPosition.value - 1];
                    positionId = position.Id;
                }
                return positionId;
            }
            set
            {
                positionId = value;
                if (m_Positions != null)
                {
                    for (int i = 0; i < m_Positions.Count; i++)
                    {
                        if (m_Positions[i].Id == positionId)
                        {
                            dropdownPosition.value = i + 1;
                        }
                    }
                }
            }
        }

        private string roleId = "";
        /// <summary>
        /// 角色Id
        /// </summary>
        public string RoleId
        {
            get
            {
                if (dropdownRole.value > 0)
                {
                    Role role = m_Roles[dropdownRole.value - 1];
                    roleId = role.Id;
                }
                return roleId;
            }
            set
            {
                roleId = value;
                if (m_Roles != null)
                {
                    for (int i = 0; i < m_Roles.Count; i++)
                    {
                        if (m_Roles[i].Id == roleId)
                        {
                            dropdownRole.value = i + 1;
                        }
                    }
                }
            }
        }

        private int status = 1;
        /// <summary>
        /// 用户状态
        /// </summary>
        public int Status
        {
            get
            {
                status = dropdownStatus.value;
                return status;
            }
            set
            {
                status = value;
                dropdownStatus.value = Status;
            }
        }

        /// <summary>
        /// 用户名InputField
        /// </summary>
        private InputField inputFieldUserName;

        /// <summary>
        /// 密码InputField
        /// </summary>
        private InputField inputFieldUserPassword;

        /// <summary>
        /// 姓名InputField
        /// </summary>
        private InputField inputFieldRealName;

        /// <summary>
        /// 性别Dropdown
        /// </summary>
        private Dropdown dropdownSex;

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
        /// 用户状态Dropdown
        /// </summary>
        private Dropdown dropdownStatus;

        /// <summary>
        /// 密码InputField
        /// </summary>
        private InputField inputFieldRemark;

        /// <summary>
        /// 警告提示文本
        /// </summary>
        private Text warning;


        /// <summary>
        /// 专业列表
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

        void Awake()
        {
            inputFieldUserName = transform.Find("Data/UserName/InputField").GetComponent<InputField>();
            inputFieldUserPassword = transform.Find("Data/UserPassword/InputField").GetComponent<InputField>();
            inputFieldRealName = transform.Find("Data/RealName/InputField").GetComponent<InputField>();
            inputFieldRemark = transform.Find("Data/Remark/InputField").GetComponent<InputField>();

            dropdownSex = transform.Find("Data/Sex/Dropdown").GetComponent<Dropdown>();
            dropdownBranch = transform.Find("Data/Branch/Dropdown").GetComponent<Dropdown>();
            dropdownGrade = transform.Find("Data/Grade/Dropdown").GetComponent<Dropdown>();
            dropdownPosition = transform.Find("Data/Position/Dropdown").GetComponent<Dropdown>();
            dropdownRole = transform.Find("Data/Role/Dropdown").GetComponent<Dropdown>();
            dropdownStatus = transform.Find("Data/Status/Dropdown").GetComponent<Dropdown>();

            warning = transform.Find("Title/Warning").GetComponent<Text>();
            warning.text = "";


            dropdownBranch.options.Add(new Dropdown.OptionData("请选择"));
            dropdownPosition.options.Add(new Dropdown.OptionData("请选择"));

            SetGrade();
        }

        /// <summary>
        /// 设置专业Dropdown
        /// </summary>
        /// <param name="questionBanks"></param>
        public void SetBranchs(List<Branch> branchs)
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
            dropdownBranch.value = 0; 

            if (!string.IsNullOrEmpty(branchId))
            {
                for (int i = 0; i < m_Branchs.Count; i++)
                {
                    if (m_Branchs[i].Id == branchId)
                    {
                        dropdownBranch.value = i + 1;
                    }
                }
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
        public void SetPositions(List<Position> positions)
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

            if (!string.IsNullOrEmpty(positionId))
            {
                for (int i = 0; i < m_Positions.Count; i++)
                {
                    if (m_Positions[i].Id == positionId)
                    {
                        dropdownPosition.value = i + 1;
                    }
                }
            }
        }

        /// <summary>
        /// 设置角色Dropdown
        /// </summary>
        /// <param name="questionBanks"></param>
        public void SetRoles(List<Role> roles)
        {
            m_Roles = roles;
            List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();
            optionDatas.Add(new Dropdown.OptionData("请选择"));
            for (int i = 0; i < m_Roles.Count; i++)
            {
                Role role = m_Roles[i];
                Dropdown.OptionData optionData = new Dropdown.OptionData(role.Name);
                optionDatas.Add(optionData);
            }
            dropdownRole.options.Clear();
            dropdownRole.options = optionDatas;
            dropdownRole.value = 0;

            if (!string.IsNullOrEmpty(roleId))
            {
                for (int i = 0; i < m_Roles.Count; i++)
                {
                    if (m_Roles[i].Id == roleId)
                    {
                        dropdownRole.value = i + 1;
                    }
                }
            }
        }

        /// <summary>
        /// 校验
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            bool result = true;
            string validation = "";

            if (string.IsNullOrEmpty(inputFieldUserName.text))
            {
                validation = "*请输入用户名*";
                result = false;
            }

            if (string.IsNullOrEmpty(inputFieldUserPassword.text))
            {
                validation = "*请输入密码*";
                result = false;
            }

            if (inputFieldUserPassword.text.Length < 6)
            {
                validation = "*密码长度不能少于6位*";
                result = false;
            }

            if (dropdownRole.value == 0)
            {
                validation = "*请选择一个角色*";
                result = false;
            }

            //if (string.IsNullOrEmpty(inputFieldRealName.text))
            //{
            //    validation = "*请输入姓名*";
            //    result = false;
            //}

            //if (dropdownBranch.value == 0)
            //{
            //    validation = "*请选择一个专业*";
            //    result = false;
            //}

            //if (dropdownGrade.value == 0)
            //{
            //    validation = "*请选择一个专业*";
            //    result = false;
            //}


            //if (dropdownPosition.value == 0)
            //{
            //    validation = "*请选择一个班级*";
            //    result = false;
            //}

            warning.text = validation;
            return result;
        }
    }
}
