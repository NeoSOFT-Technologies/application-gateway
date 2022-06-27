using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Domain.Common
{
    public class SortSearchData
    {
        public bool sort { get; set; } = false;
        public bool search { get; set; } = false;

#nullable enable
        public SortParam? sortParam { get; set; } = null;
        public SearchParam? searchParam { get; set; } = null;
#nullable disable
    }
    public class SearchParam
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class SortParam
    {
        public string param { get; set; }
        public bool isDesc { get; set; } = false;
    }
}

