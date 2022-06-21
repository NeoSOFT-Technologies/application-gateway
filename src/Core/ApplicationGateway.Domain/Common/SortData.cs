using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Domain.Common
{
    public class SortData
    {
        public bool sort { get; set; } = false;

#nullable enable
        public SortParam? sortParam { get; set; } = null;
#nullable disable
    }

    public class SortParam
    {
        public string param { get; set; }
        public bool isDesc { get; set; } = false;
    }
}

