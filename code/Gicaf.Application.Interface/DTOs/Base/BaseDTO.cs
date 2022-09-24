using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Application.Interface.DTOs.Base
{
    public abstract class BaseDTO
    {
        public long Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataModificacao { get; set; }
    }
}
