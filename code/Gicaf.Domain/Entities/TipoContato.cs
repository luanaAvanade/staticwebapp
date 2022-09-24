using Gicaf.Domain.Entities.Base;

namespace Gicaf.Domain.Entities
{
    public class TipoContato : BaseTrailEntity
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool Status { get; set; }
  }
}
