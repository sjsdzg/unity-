using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework.Architectural
{
    public class HeaderVariable
    {
        private string name;
        /// <summary>
        /// the header variable name.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        private object variable;
        /// <summary>
        /// the header variable stored value.
        /// </summary>
        public object Value
        {
            get { return variable; }
            set { variable = value; }
        }

        public HeaderVariable(string name, object value)
        {
            this.name = name;
            this.variable = value;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", this.name, this.variable);
        }
    }
}
