using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.ToolInfo
{
    public record ModelId(Guid Value)
    {
        public static ModelId Empty() => new(Guid.Empty);
        public static ModelId New() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
