using Gicaf.Domain.Entities.Base;

namespace Gicaf.Domain.Entities
{
    public class ExigenciaGrupoQualificacao : BaseTrailEntity
    {
        public long ExigenciaId { get; set; }
        public Exigencia Exigencia {get; set; }
        public long GrupoCategoriaId { get; set; }
        public GrupoCategoria GrupoCategoria { get; set; }
        public bool Fabricante { get; set; }
        public bool Distribuidor { get; set; }

    }
}