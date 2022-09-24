using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Domain.Interfaces
{
    interface IEntityValidation
    {
        bool IsValid();
        IEnumerable<string[]> ValidationErrors();        
    }
}
