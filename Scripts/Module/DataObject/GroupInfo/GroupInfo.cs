using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFramework.Core;

namespace XFramework.Module
{
    public class GroupInfo
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class GroupInfoManifest : DataObject<GroupInfoManifest>
    {
        public List<GroupInfo> GroupInfos { get; set; }
    }
}
