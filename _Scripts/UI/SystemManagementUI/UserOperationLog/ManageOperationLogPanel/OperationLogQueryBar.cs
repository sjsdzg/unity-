using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Module;

namespace XFramework.UI
{
    public class OperationLogQueryBar : QueryBar
    {
        public const string USER_NAME = "user_name";//用户名
        public const string REAL_NAME = "real_name";//姓名
        public const string BRANCH_ID = "branch_id";//专业Id
        public const string Grade = "grade";//年级
        public const string POSITION_ID = "position_id";//班级Id
        public const string STATUS = "status";//状态

        /// <summary>
        /// 用户名InputField
        /// </summary>
        private InputField inputFieldUserName;

        /// <summary>
        /// 姓名InputField
        /// </summary>
        private InputField inputFieldRealName;

        /// <summary>
        /// 专业Dropdown
        /// </summary>
        private Dropdown dropdownBranch;

        /// <summary>
        /// 班级Dropdown
        /// </summary>
        private Dropdown dropdownPosition;

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
        private List<Role> m_UserRoles;

        public override void OnAwake()
        {
            base.OnAwake();
            inputFieldUserName = transform.Find("Conditions/InputFieldUserName").GetComponent<InputField>();
            inputFieldRealName = transform.Find("Conditions/InputFieldRealName").GetComponent<InputField>();
            dropdownBranch = transform.Find("Conditions/DropdownBranch").GetComponent<Dropdown>();
            dropdownPosition = transform.Find("Conditions/DropdownPosition").GetComponent<Dropdown>();
        }

        protected override List<SqlCondition> BuildSqlConditions()
        {
            List<SqlCondition> sqlConditions = new List<SqlCondition>();
            //用户名称
            if (!string.IsNullOrEmpty(inputFieldUserName.text))
            {
                sqlConditions.Add(new SqlCondition(Constants.USER_NAME, SqlOption.Like, SqlType.String, "%" + inputFieldUserName.text + "%"));
            }
            //姓名
            if (!string.IsNullOrEmpty(inputFieldRealName.text))
            {
                sqlConditions.Add(new SqlCondition(Constants.REAL_NAME, SqlOption.Like, SqlType.String, "%" + inputFieldRealName.text + "%"));
            }
            //部门ID
            if (dropdownBranch.value > 0 && m_Branchs != null)
            {
                Branch branch = m_Branchs[dropdownBranch.value - 1];
                sqlConditions.Add(new SqlCondition(Constants.BRANCH_ID, SqlOption.Equal, SqlType.String, branch.Id));
            }
            //班级Id
            if (dropdownPosition.value > 0 && m_Positions != null)
            {
                Position position = m_Positions[dropdownPosition.value - 1];
                sqlConditions.Add(new SqlCondition(Constants.POSITION_ID, SqlOption.Equal, SqlType.String, position.Id));
            }
            //排序 按时间降序
            sqlConditions.Add(new SqlCondition(Constants.CREATE_TIME, SqlOption.OrderBy, Constants.ASC));
            return sqlConditions;
        }

        /// <summary>
        /// 设置专业Dropdown
        /// </summary>
        /// <param name="questionBanks"></param>
        public void SetBranchs(List<Branch> branchs)
        {
            m_Branchs = branchs;

            List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();
            optionDatas.Add(new Dropdown.OptionData("全部"));
            for (int i = 0; i < m_Branchs.Count; i++)
            {
                Branch branch = m_Branchs[i];
                Dropdown.OptionData optionData = new Dropdown.OptionData(branch.Name);
                optionDatas.Add(optionData);
            }
            dropdownBranch.options.Clear();
            dropdownBranch.options = optionDatas;

            dropdownBranch.value = 0;
        }

        /// <summary>
        /// 设置班级Dropdown
        /// </summary>
        /// <param name="questionBanks"></param>
        public void SetPositions(List<Position> positions)
        {
            m_Positions = positions;

            List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();
            optionDatas.Add(new Dropdown.OptionData("全部"));
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
        //public void SetRoles(List<Role> userRoles)
        //{
        //    m_UserRoles = userRoles;
        //    List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();
        //    optionDatas.Add(new Dropdown.OptionData("全部"));
        //    for (int i = 0; i < m_UserRoles.Count; i++)
        //    {
        //        Role userRole = m_UserRoles[i];
        //        Dropdown.OptionData optionData = new Dropdown.OptionData(userRole.Name);
        //        optionDatas.Add(optionData);
        //    }
        //    dropdownRole.options.Clear();
        //    dropdownRole.options = optionDatas;
        //    dropdownRole.value = 0;
        //}
    }
}
