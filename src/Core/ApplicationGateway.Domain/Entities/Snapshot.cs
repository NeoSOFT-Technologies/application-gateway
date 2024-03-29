﻿using ApplicationGateway.Domain.Common;
using System;
using System.Collections.Generic;

namespace ApplicationGateway.Domain.Entities
{
    public partial class Snapshot : AuditableEntity
    {
        public int Id { get; set; }
        public string Gateway { get; set; } = null!;
        public string ObjectName { get; set; } = null!;
        public string ObjectKey { get; set; } = null!;
        public string JsonData { get; set; } = null!;
#nullable enable
        public bool? IsActive { get; set; }
        public string? Comment { get; set; }
#nullable disable
    }
}
