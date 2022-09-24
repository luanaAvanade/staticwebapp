using Gicaf.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Domain.Entities
{
    public class VersaoMec: BaseTrailEntity
    {
        public string Nome { get; set; }
        public string FormulaEixoX { get; set; }
        public string FormulaEixoY { get; set; }
        public DateTime? DataEncerramento { get; set; }
        public bool Encerrado => DataEncerramento != null && DataEncerramento.Value != default(DateTime);
        public string LinkMatriz { get; set; }

        public ICollection<Categoria> Categorias { get; set; }
    }
}
