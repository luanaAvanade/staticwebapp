
using Gicaf.Domain.Entities.Base;
using System;

namespace Gicaf.Domain.Entities.Fornecedores
{
  public class DadosDRE : BaseTrailEntity
  {
    public string DadosCalculadosPeloRobo { get; set; }
    public DateTime DataReferencia { get; set; }
   
        //Receita Operacional Líquida
        public double? ReceitaOperacionalLiquida { get; set; }
        public double? CustoProdutosVendidosMercadoriasVendidasServicosPrestados { get; set; }

        //Resultado Operacional Bruto
        public double? ResultadoOperacionalBruto { get; set; }
        public double? DespesasVendasAdministrativasGeraisOutras { get; set; }
        public double? DespesasFinanceiras { get; set; }
        public double? ReceitasFinanceiras { get; set; }

        //Resultado Operacional Antes do IR e CSSL
        public double? ResultadoOperacionalAntesIrCssl { get; set; }

        //Resultado Líquido do Período
        public double? ResultadoLiquidoPeriodo { get; set; }

        public long EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
    }
}