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
    public class PersonalDataBar : MonoBehaviour
    {
        private string userName;
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get
            {
                userName = textUserName.text;
                return userName;
            }
            set
            {
                userName = value;
                textUserName.text = userName;
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

        private string branch = "";
        /// <summary>
        /// 用户分类Id
        /// </summary>
        public string Branch
        {
            get { return branch; }
            set
            {
                branch = value;
                textBranch.text = branch;
            }
        }

        private string grade = "";
        /// <summary>
        /// 用户分类Id
        /// </summary>
        public string Grade
        {
            get { return grade; }
            set
            {
                grade = value;
                textGrade.text = grade;
            }
        }

        private string position = "";
        /// <summary>
        /// 用户分类Id
        /// </summary>
        public string Position
        {
            get{ return position; }
            set
            {
                position = value;
                textPosition.text = position;
            }
        }

        private string role = "";
        /// <summary>
        /// 用户角色Id
        /// </summary>
        public string Role
        {
            get { return role; }
            set
            {
                role = value;
                textRole.text = role;
            }
        }

        /// <summary>
        /// 用户名Text
        /// </summary>
        private Text textUserName;

        /// <summary>
        /// 姓名InputField
        /// </summary>
        private InputField inputFieldRealName;

        /// <summary>
        /// 性别Dropdown
        /// </summary>
        private Dropdown dropdownSex;

        /// <summary>
        /// 专业Text
        /// </summary>
        private Text textBranch;

        /// <summary>
        /// 年级Text
        /// </summary>
        private Text textGrade;

        /// <summary>
        /// 班级Text
        /// </summary>
        private Text textPosition;

        /// <summary>
        /// 角色Text
        /// </summary>
        private Text textRole;

        /// <summary>
        /// 备注InputField
        /// </summary>
        private InputField inputFieldRemark;

        void Awake()
        {
            textUserName = transform.Find("Data/UserName/Text").GetComponent<Text>();
            inputFieldRealName = transform.Find("Data/RealName/InputField").GetComponent<InputField>();
            inputFieldRemark = transform.Find("Data/Remark/InputField").GetComponent<InputField>();
            dropdownSex = transform.Find("Data/Sex/Dropdown").GetComponent<Dropdown>();
            textBranch = transform.Find("Data/Branch/Text").GetComponent<Text>();
            textGrade = transform.Find("Data/Grade/Text").GetComponent<Text>();
            textPosition = transform.Find("Data/Position/Text").GetComponent<Text>();
            textRole = transform.Find("Data/Role/Text").GetComponent<Text>();
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            UserName = userName;
            RealName = realName;
            Remark = remark;
            Sex = sex;
            Branch = branch;
            Grade = grade;
            Position = position;
            Role = role;
        }
    }
}
