using Gicaf.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Domain.Entities
{
    public enum QuadranteEnum
    {
        Estrategico,
        Alavancagem,
        NaoCritico,
        Critico
    }

    public enum NivelRisco
    {
        Baixo,
        Medio,
        Alto
    }

    public class AvaliacaoCategoria: BaseTrailEntity
    {
        public long CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        //public long QuadranteId { get; set; }
        //public Quadrante Quadrante { get; set; }

        public float EixoX { get; set; }
        public float EixoY { get; set; }

        //public Quadrante Quadrante { get; set; }
        public long? QuadranteId { get; set; }
        public Quadrante Quadrante { get; set; }

        public double EstimativaGastoMensal { get; set; }
        public float GastoTotal { get; set; }

        /*
        public int NotaVolumeCompras { get; set; }
        public int NotaImpactoFaltaMaterial { get; set; }
        public int NotaNivelExigidoFornecedor { get; set; }
        public int NotaSubstituicaoFornecedor { get; set; }
        public int NotaNumeroFornecedores { get; set; }
        public int NotaRegulamentacao { get; set; }
        public int NotaEscala { get; set; }
        public int NotaMaturidade { get; set; }
        */
        public NivelRisco RiscoOperativo { get; set; }
        public NivelRisco RiscoQualidade { get; set; }

        public ICollection<Resultado> Resultados { get; set; }

        /*
        public NivelRisco RiscoSegurancaSaude { get; set; }
        public NivelRisco RiscoAmbiental { get; set; }
        public NivelRisco RiscoResponsabilidadeSocial { get; set; }
        public NivelRisco RiscoIntegridade { get; set; }
        */

        public VersaoMec VersaoMec { get; set; }
        public long? VersaoMecId { get; set; }
    }
}
