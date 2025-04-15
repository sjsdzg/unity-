using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using XFramework.Core;

namespace XFramework.Module
{
    public class PageBeanJson<T> : DataObject<PageBeanJson<T>>
    {
        [JsonProperty("currentPage")]
        public int CurrentPage { get; set; }

        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        [JsonProperty("totalRecords")]
        public int TotalRecords { get; set; }

        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }

        /** 要显示的数据 */
        public List<T> dataList = new List<T>();
    }
}
