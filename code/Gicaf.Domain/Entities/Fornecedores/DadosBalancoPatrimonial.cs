
using Gicaf.Domain.Entities.Base;
using System;

namespace Gicaf.Domain.Entities.Fornecedores
{
  public class DadosBalancoPatrimonial : BaseTrailEntity
  {
    public string DadosCalculadosPeloRobo { get; set; }
    public DateTime DataReferencia { get; set; }
    //Ativo Total
    public double? AtivoTotal { get; set; }

    // Circulante'
    public double? CirculanteAtivo { get; set; } 
    public double? Disponibilidades { get; set; }
    public double? Estoques { get; set; }
    public double? OutrosAtivosCirculante { get; set; }
    // Não Circulante
    public double? AtivoNaoCirculante { get; set; }


    //Passivo Total
    public double? PassivoTotal { get; set; }

    // Circulante
    public double? CirculantePassivo { get; set; }
    public double? EmprestimosFinanciamentoCirculante { get; set; }
    public double? OutrosPassivosCirculantes { get; set; }
    // Não Circulante
    public double? NaoCirculantePassivo { get; set; }
    public double? EmprestimosFinanciamentoNaoCirculante { get; set; }
    public double? OutrosPassivosNaoCirculantes { get; set; }

    //Patrimônio Líquido
    public double? PatrimonioLiquido { get; set; }

    public long EmpresaId { get; set; }
    public Empresa Empresa { get; set; }
  }
}