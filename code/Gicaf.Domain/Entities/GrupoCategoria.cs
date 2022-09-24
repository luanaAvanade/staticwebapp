using Gicaf.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Gicaf.Domain.Entities.Fornecedores;

namespace Gicaf.Domain.Entities
{
    public enum TipoItem
    {
        Material,
        Servico,
        Ambos
    }
  
    public class GrupoCategoria : BaseTrailEntity
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public long CategoriaId { get; set; }
        public Categoria Categoria { get; set; } 
        public ICollection<GrupoFornecimento> GruposFornecimento { get; set; }
        public ICollection<ExigenciaGrupoQualificacao> ExigenciasGrupoPerguntaQualificacao { get; set; }
        public ICollection<Sku> Skus { get; set; }
  }
}
