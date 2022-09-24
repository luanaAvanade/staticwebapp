using System;
using Gicaf.Domain.Entities.Base;
using Gicaf.Domain.Entities.Enums;

namespace Gicaf.Domain.Entities.Fornecedores
{
    public class CalculoRisco : BaseTrailEntity
    {
      public long EmpresaId { get; set; }
      public Empresa Empresa { get; set; }
      public DateTime Data { get; set; }
      public double CCL   { get; set; }
      public double NIG   { get; set; }
      public double SD   { get; set; }
      public ClassificacaoRisco ClassificacaoFase1  { get; set; }
      public double LC { get; set; }
      public double LS { get; set; }
      public double EG { get; set; }
      public double CE { get; set; }
      public double ALDB { get; set; }
      public double ALDL { get; set; }
      public double AL { get; set; }
      public double ICJ { get; set; }
      public double ROE { get; set; }
      public double ME { get; set; }
      public double ML { get; set; }
      public double GA { get; set; }

      public ClassificacaoRisco RiscoLC => LC > 1 ? ClassificacaoRisco.Baixo : ClassificacaoRisco.Alto;
      public ClassificacaoRisco RiscoLS => LS > 0 ? ClassificacaoRisco.Baixo : ClassificacaoRisco.Alto;
      public ClassificacaoRisco RiscoEG => EG < 0.7 ? ClassificacaoRisco.Baixo : ClassificacaoRisco.Alto;
      public ClassificacaoRisco RiscoCE => CE < 0.5 ? ClassificacaoRisco.Baixo : ClassificacaoRisco.Alto;
      public ClassificacaoRisco RiscoALDB => ALDB < 1 ? ClassificacaoRisco.Baixo : ClassificacaoRisco.Alto;
      public ClassificacaoRisco RiscoALDL => ALDL < 0.5 ? ClassificacaoRisco.Baixo : ClassificacaoRisco.Alto;
      public ClassificacaoRisco RiscoAL => AL < 2 ? ClassificacaoRisco.Baixo : ClassificacaoRisco.Alto;
      public ClassificacaoRisco RiscoICJ => ICJ > 1 ? ClassificacaoRisco.Baixo : ClassificacaoRisco.Alto;
      public ClassificacaoRisco RiscoROE => ROE > 0.05 ? ClassificacaoRisco.Baixo : ClassificacaoRisco.Alto;
      public ClassificacaoRisco RiscoME => ME > 0 ? ClassificacaoRisco.Baixo : ClassificacaoRisco.Alto;
      public ClassificacaoRisco RiscoML => ML > 0 ? ClassificacaoRisco.Baixo : ClassificacaoRisco.Alto;
      public ClassificacaoRisco RiscoGA => GA > 0.1 ? ClassificacaoRisco.Baixo : ClassificacaoRisco.Alto;

      public void CalcularFase1(DadosBalancoPatrimonial dadosBalancoPatrimonial)
      {
        CCL = SetToZero((dadosBalancoPatrimonial.CirculanteAtivo - dadosBalancoPatrimonial.CirculantePassivo).Value);

        //NIG = Ativo Circulante Operacional - Passivo Circulante Operacional

        //Ativo Circulante Operacional = Ativo Circulante – Disponibilidade
        //Passivo Circulante Operacional = Passivo Circulante - Passivo Circulante Financeiro
        //Passivo Circulante Financeiro = Emprestimos e Financiamentos
        NIG = SetToZero( ((dadosBalancoPatrimonial.CirculanteAtivo - dadosBalancoPatrimonial.Disponibilidades) - 
              (dadosBalancoPatrimonial.CirculantePassivo - dadosBalancoPatrimonial.EmprestimosFinanciamentoCirculante)).Value);

        //SD = Ativo Circulante Finaceiro - Passivo Circulante Financeiro
        //Ativo Circulante Finaceiro = Disponibilidade
        //Passivo Circulante Financeiro = Emprestimos e Financiamentos
        SD = SetToZero( (dadosBalancoPatrimonial.Disponibilidades - dadosBalancoPatrimonial.EmprestimosFinanciamentoCirculante).Value);
      
        if(CCL > 0 && NIG < 0 && CCL > NIG && SD > 0){
          ClassificacaoFase1 = ClassificacaoRisco.Baixo;
        }
        if(CCL > 0 && NIG > 0 && CCL > NIG && SD > 0){
          ClassificacaoFase1 = ClassificacaoRisco.Baixo;
        }
        if(CCL > 0 && NIG > 0 && CCL < NIG && SD < 0){
          ClassificacaoFase1 = ClassificacaoRisco.Medio;
        }
        if(CCL < 0 && NIG > 0 && CCL < NIG && SD < 0){
          ClassificacaoFase1 = ClassificacaoRisco.Alto;
        }
        if(CCL < 0 && NIG < 0 && CCL < NIG && SD < 0){
          ClassificacaoFase1 = ClassificacaoRisco.Alto;
        }
        if(CCL < 0 && NIG < 0 && CCL > NIG && SD > 0){
          ClassificacaoFase1 = ClassificacaoRisco.Alto;
        }
      }
      public void CalcularFase2(DadosBalancoPatrimonial dadosBalancoPatrimonial, DadosDRE DadosDRE)
      {
        
        LC = SetToZero((dadosBalancoPatrimonial.CirculanteAtivo / dadosBalancoPatrimonial.CirculantePassivo).Value);
        LS = SetToZero(((dadosBalancoPatrimonial.CirculanteAtivo - dadosBalancoPatrimonial.Estoques)/dadosBalancoPatrimonial.CirculantePassivo).Value);
        EG = SetToZero(((dadosBalancoPatrimonial.PassivoTotal - dadosBalancoPatrimonial.PatrimonioLiquido)/ dadosBalancoPatrimonial.AtivoTotal).Value);
        CE = SetToZero((dadosBalancoPatrimonial.CirculantePassivo/ (dadosBalancoPatrimonial.PassivoTotal - dadosBalancoPatrimonial.PatrimonioLiquido)).Value);
        ALDB = SetToZero(((dadosBalancoPatrimonial.EmprestimosFinanciamentoCirculante + dadosBalancoPatrimonial.EmprestimosFinanciamentoNaoCirculante)/
                dadosBalancoPatrimonial.PatrimonioLiquido).Value);
        ALDL = SetToZero(((dadosBalancoPatrimonial.EmprestimosFinanciamentoCirculante + dadosBalancoPatrimonial.EmprestimosFinanciamentoNaoCirculante - dadosBalancoPatrimonial.Disponibilidades)/
                dadosBalancoPatrimonial.PatrimonioLiquido).Value);
        AL = SetToZero(((dadosBalancoPatrimonial.EmprestimosFinanciamentoCirculante + dadosBalancoPatrimonial.EmprestimosFinanciamentoNaoCirculante)/
              (DadosDRE.ResultadoOperacionalAntesIrCssl - DadosDRE.ReceitasFinanceiras + Math.Abs(DadosDRE.DespesasFinanceiras.Value))).Value);
        ICJ = SetToZero(((DadosDRE.ResultadoOperacionalAntesIrCssl - DadosDRE.ReceitasFinanceiras + Math.Abs(DadosDRE.DespesasFinanceiras.Value))/
              Math.Abs(DadosDRE.DespesasFinanceiras.Value)).Value);
        ROE = SetToZero((DadosDRE.ResultadoLiquidoPeriodo / dadosBalancoPatrimonial.PatrimonioLiquido).Value);
        ME = SetToZero(((DadosDRE.ResultadoOperacionalAntesIrCssl - DadosDRE.ReceitasFinanceiras + Math.Abs(DadosDRE.DespesasFinanceiras.Value))/
        DadosDRE.ReceitaOperacionalLiquida).Value);
        ML = SetToZero((DadosDRE.ResultadoLiquidoPeriodo / DadosDRE.ReceitaOperacionalLiquida).Value);
        GA = SetToZero((DadosDRE.ReceitaOperacionalLiquida / dadosBalancoPatrimonial.CirculanteAtivo).Value);
      }
    
      private double SetToZero(double value)
      {
        return double.IsNaN(value) || double.IsInfinity(value) ? 0 : value;
      }
    }
}