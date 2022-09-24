using Gicaf.Domain.Entities.Base;
using System;

namespace Gicaf.Domain.Entities.Fornecedores
{
    public class Banco : BaseTrailEntity
    {
        public string codigo  { get; set; }
        public string descricao  { get; set; }
    }
}