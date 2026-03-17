using System;
using System.Collections.Generic;
using System.Text;

namespace cod.domain.Common
{
    public interface IEntity
    {
        int Id { get; set; }
        string CreatorUser { get; set; }
        string LastModifiedUser { get; set; }
        bool IsActive { get; set; }
        DateTime CreationTime { get; set; }
        DateTime? LastModificationTime { get; set; }
        DateTime? DisableDateTime { get; set; }
        string DisableUser { get; set; }
        DateTime? EnableDateTime { get; set; }
        string EnableUser { get; set; }
    }
}
