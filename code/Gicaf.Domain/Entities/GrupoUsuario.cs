using Gicaf.Domain.Entities.Base;
using System.Collections.Generic;

namespace Gicaf.Domain.Entities
{
    public class GrupoUsuario : BaseTrailEntity
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public ICollection<Usuario> Usuarios { get; set; }
        public ICollection<PerguntaGrupoUsuario> PerguntaGrupoUsuario { get; set; }
      
    }
}
