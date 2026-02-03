using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.ToolInfo
{
    public record ToolTypeId(Guid Value)
    {
        public static ToolTypeId Empty() => new(Guid.Empty);
        public static ToolTypeId New() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
