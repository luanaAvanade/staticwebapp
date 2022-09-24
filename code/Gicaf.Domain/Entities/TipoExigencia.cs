using Gicaf.Domain.Entities.Base;
using Gicaf.Domain.Entities.Enums;

namespace Gicaf.Domain.Entities{

  public class TipoExigencia : BaseTrailEntity{
    public string Nome {get; set;}
    public string Descricao {get; set;}
    public NivelTipoExigencia NivelExigencia {get; set;}
    public bool Status {get; set;}

  }
}