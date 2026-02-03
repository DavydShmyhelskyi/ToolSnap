using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.ToolInfo
{
    public record BrandId(Guid Value)
    {
        public static BrandId Empty() => new(Guid.Empty);
        public static BrandId New() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
