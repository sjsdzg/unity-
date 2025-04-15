using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework.Architectural
{
    public class HeaderVariables
    {
        private readonly Dictionary<string, HeaderVariable> variables;

        public HeaderVariables()
        {
            this.variables = new Dictionary<string, HeaderVariable>
            {
                {HeaderVariableCode.Version, new HeaderVariable(HeaderVariableCode.Version, Architect.version) },
                {HeaderVariableCode.TDCreate, new HeaderVariable(HeaderVariableCode.TDCreate, DateTime.Now) },
                {HeaderVariableCode.TDUpdate, new HeaderVariable(HeaderVariableCode.TDUpdate, DateTime.Now) },
            };
        }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version 
        {
            get { return this.variables[HeaderVariableCode.Version].Value.ToString(); }
            set { this.variables[HeaderVariableCode.Version].Value = value; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime TDCreate
        {
            get { return (DateTime)this.variables[HeaderVariableCode.TDCreate].Value; }
            set { this.variables[HeaderVariableCode.TDCreate].Value = value; }
        }

        /// <summary>
        /// 更新或保存时间 （最后一次）
        /// </summary>
        public DateTime TDUpdate
        {
            get { return (DateTime)this.variables[HeaderVariableCode.TDUpdate].Value; }
            set { this.variables[HeaderVariableCode.TDCreate].Value = value; }
        }

        /// <summary>
        /// Gets the collection of header variables.
        /// </summary>
        public ICollection<HeaderVariable> Values
        {
            get { return this.variables.Values; }
        }
    }
}
