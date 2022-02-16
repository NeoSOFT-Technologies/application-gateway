using System;
using System.Collections.Generic;

namespace ApplicationGateway.Domain.Entities
{
    public partial class Snapshot
    {
        public int Id { get; set; }
        public string Gateway { get; set; } = null!;
        public string ObjectName { get; set; } = null!;
        public Guid ObjectKey { get; set; }
        public string JsonData { get; set; } = null!;
        public bool? IsActive { get; set; }
        public string? Comment { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
