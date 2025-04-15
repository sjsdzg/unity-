using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFramework.Core;

namespace XFramework.Module
{
    public class VariableCollection : DataObject<VariableCollection>
    {
        public List<Variable> Variables { get; set; }

        public VariableCollection()
        {
            Variables = new List<Variable>();
        }
    }
}
