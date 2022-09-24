
using Gicaf.Domain.Entities.Base;

namespace Gicaf.Domain.Entities.Fornecedores
{
    public class CNAESubClasse : BaseTrailEntity
    {  
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string Ocupacao { get; set; }
        public char Status { get; set; }
    }
}