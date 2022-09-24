using Gicaf.Domain.Entities;
using Gicaf.Domain.Entities.Fornecedores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gicaf.Domain.Interfaces.Repository;
using Newtonsoft.Json;
using Gicaf.Domain.Interfaces;


namespace Gicaf.Domain.Services
{
  public interface IAuthenticatorService
    {
        Task<object> RegisterUserAsync(Usuario usuario);
        void SendResultadoAnaliseEmail(Usuario usuario, string empresa, List<ItemAnalise> itensAnalise, string linkCadastro);
    }

    public class EmpresaService : ServiceBase<Empresa>
    {
        IAuthenticatorService _authenticatorService;
        public EmpresaService(IServiceProvider serviceProvider, IAuthenticatorService authenticatorService) : base(serviceProvider)
        {
            _authenticatorService = authenticatorService;
        }

        public override Empresa Add(Empresa empresa, IDictionary<string, object> input)
        {
            if(empresa.Usuarios != null)
            {
                var usuario = empresa.Usuarios.FirstOrDefault();
                if (usuario != null && usuario.PassWord != usuario.ConfirmPassWord)
                {
                    throw new Exception("Confirmação de Password inválida!");
                }
                if (usuario != null && usuario.Id == default(long))
                {
                    // Adicionado a perfil de fornecedor para todo novo usuario criado atraves do 
                    // cadastro de empresa
                    usuario.Roles = new List<string>(){"Fornecedor"};
                    _authenticatorService.RegisterUserAsync(usuario);
                }
            }

            if(empresa.Id == 0L){
                var termosAceite = GetRepository<IRepositoryBase<TermosAceite>>()
                                     .GetWhere(null, x => x.TipoCadastro == empresa.TipoCadastro 
                                     && x.TipoFornecedor == empresa.TipoEmpresa);
                empresa.TermoAceiteEmpresa = empresa.TermoAceiteEmpresa ?? new List<TermoAceiteEmpresa>();
            
                foreach(var termoAceite in termosAceite)
                {  
                    var termoAceiteEmpresa = new TermoAceiteEmpresa
                    {
                        TermosAceiteId = termoAceite.Id,
                        Aceite = false,
                    };
                    empresa.TermoAceiteEmpresa.Add(termoAceiteEmpresa); 
                }

                AnaliseCadastro analiseCadastro = new AnaliseCadastro{
                    StatusAnalise = StatusAnalise.Criado,
                    ItensAnalise = new List<ItemAnalise>()
                };

                List<EnumItemAnalise> ItensParaAnalisar;
                switch(empresa.TipoEmpresa){
                    //Pessoa Juridica
                    case "1":
                        if(empresa.TipoCadastro == "cadastroCentralizado" ){
                            ItensParaAnalisar = new List<EnumItemAnalise> { EnumItemAnalise.Dados_Gerais,
                                EnumItemAnalise.Dados_Endereco, EnumItemAnalise.Acesso_Sistema, EnumItemAnalise.Dados_Contatos_Adicionais, 
                                EnumItemAnalise.Grupos_Fornecimento, EnumItemAnalise.Dados_Bancarios, EnumItemAnalise.Dados_Balanco_Patrimonial,
                                EnumItemAnalise.Dados_DRE, EnumItemAnalise.Dados_Contrato_Social, 
                                EnumItemAnalise.Cadastro_Socios, /*EnumItemAnalise.Qualificacao_Risco_Financeiro, EnumItemAnalise.Cadastro_Procuradores,*/ EnumItemAnalise.Cadastro_Signatario            
                            }; 
                        }else{
                            ItensParaAnalisar = new List<EnumItemAnalise> { EnumItemAnalise.Dados_Gerais,
                                EnumItemAnalise.Dados_Endereco, EnumItemAnalise.Acesso_Sistema, EnumItemAnalise.Dados_Contatos_Adicionais, 
                                EnumItemAnalise.Dados_Bancarios, EnumItemAnalise.Contato_Cliente            
                            };    
                        }
                        break;
                    //MEI
                    case "2":
                        if(empresa.TipoCadastro == "cadastroCentralizado" ){
                            ItensParaAnalisar = new List<EnumItemAnalise> { EnumItemAnalise.Dados_Gerais,
                                EnumItemAnalise.Dados_Endereco, EnumItemAnalise.Acesso_Sistema, EnumItemAnalise.Dados_Contatos_Adicionais, 
                                EnumItemAnalise.Grupos_Fornecimento, EnumItemAnalise.Dados_Bancarios, EnumItemAnalise.Dados_Balanco_Patrimonial,
                                EnumItemAnalise.Dados_DRE, /*EnumItemAnalise.Qualificacao_Risco_Financeiro,*/        
                            };
                        }else{
                            ItensParaAnalisar = new List<EnumItemAnalise> { EnumItemAnalise.Dados_Gerais,
                                EnumItemAnalise.Dados_Endereco, EnumItemAnalise.Acesso_Sistema, EnumItemAnalise.Dados_Contatos_Adicionais, 
                                EnumItemAnalise.Dados_Bancarios, EnumItemAnalise.Contato_Cliente       
                            };   
                        }
                        break;
                    default: ItensParaAnalisar = new List<EnumItemAnalise>(); break;
                }        
                
                List<ItemAnalise> itensAnalise = new List<ItemAnalise>();

                foreach(EnumItemAnalise item in ItensParaAnalisar){
                    ItemAnalise itemAnalise = new ItemAnalise{
                        TipoItem = item
                    };    
                    itensAnalise.Add(itemAnalise);                
                }

                analiseCadastro.ItensAnalise = itensAnalise;
                empresa.AnaliseCadastro = analiseCadastro;
            }else{
                if(empresa.AnaliseCadastro != null)
                    if(empresa.AnaliseCadastro.StatusAnalise == StatusAnalise.Aprovado || 
                        empresa.AnaliseCadastro.StatusAnalise == StatusAnalise.Reprovado){
                        StatusAnalise statusAtual = GetRepository<IRepositoryBase<AnaliseCadastro>>()
                            .GetWhere(null, x => x.EmpresaFornecedoraId == empresa.Id).Select(x => x.StatusAnalise).FirstOrDefault();
                        Usuario u = GetRepository<IRepositoryBase<Usuario>>().GetWhere(null, x => x.Email == "mariana.garcia@axxiom.com.br").FirstOrDefault();
                        Empresa empresaBanco = GetRepository<IRepositoryBase<Empresa>>()
                            .GetWhere(null, x => x.Id == empresa.Id, y => y.AnaliseCadastro .StatusAnalise).FirstOrDefault();
                        // Empresa e2 = GetRepository<IRepositoryBase<Empresa>>()
                        //     .GetWhere(null, x => x.Id == empresa.Id, y => y.AnaliseCadastro.ItensAnalise.Select(z => z.TipoItem)).FirstOrDefault();
                        if(statusAtual != empresa.AnaliseCadastro.StatusAnalise){
                            _authenticatorService.SendResultadoAnaliseEmail(u, empresaBanco.NomeEmpresa, empresa.AnaliseCadastro.ItensAnalise.ToList(), empresa.LinkCadastro);
                            //envia email
                        }
                    }
            }                              
            

            if(empresa.Id > 0 && empresa?.AnaliseCadastro?.StatusAnalise == StatusAnalise.Pendente_Analise)
            {
                var balancos = GetRepository<IRepositoryBase<DadosBalancoPatrimonial>>().GetWhere(null, x => x.EmpresaId == empresa.Id);

                var dres = GetRepository<IRepositoryBase<DadosDRE>>()
                                    .GetWhere(null, x => x.EmpresaId == empresa.Id);

                if(balancos?.Any() ?? false)
                {
                    empresa.CalculoRiscoLista = new List<CalculoRisco>();
                }

                dres = dres.OrderBy(x => x.DataReferencia).ToList();
                var enumerator = dres.GetEnumerator();

                foreach (var dadoBalancoPatrimonial in balancos.OrderBy(x => x.DataReferencia))
                {
                    enumerator.MoveNext();
                    var calculoRisco = new CalculoRisco();
                    calculoRisco.Data = dadoBalancoPatrimonial.DataReferencia;
                    calculoRisco.CalcularFase1(dadoBalancoPatrimonial);
                    calculoRisco.CalcularFase2(dadoBalancoPatrimonial, enumerator.Current);
                    empresa.CalculoRiscoLista.Add(calculoRisco);
                }
            }

            var settings = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
            var jsonObj = Newtonsoft.Json.JsonConvert.SerializeObject(empresa.CalculoRiscoLista, settings);
            var inputObj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonObj);

            input.Add(nameof(Empresa.CalculoRiscoLista), inputObj);
            
            
            ProcessarArquivosRobo(empresa);

            return base.Add(empresa, input);
            
        }
    

        public override Empresa Update(Empresa empresa, IDictionary<string, object> input)
        {
            if(empresa?.AnaliseCadastro?.StatusAnalise == StatusAnalise.Pendente_Analise)
            {
                var balancos = GetRepository<IRepositoryBase<DadosBalancoPatrimonial>>()
                                    .GetWhere(null, x => x.EmpresaId == empresa.Id);

                var dres = GetRepository<IRepositoryBase<DadosDRE>>()
                                    .GetWhere(null, x => x.EmpresaId == empresa.Id);

                if(balancos?.Any() ?? false)
                {
                    empresa.CalculoRiscoLista = new List<CalculoRisco>();
                }

                dres = dres.OrderBy(x => x.DataReferencia).ToList();
                var enumerator = dres.GetEnumerator();

                foreach (var dadoBalancoPatrimonial in balancos.OrderBy(x => x.DataReferencia))
                {
                    enumerator.MoveNext();
                    var calculoRisco = new CalculoRisco();
                    calculoRisco.Data = dadoBalancoPatrimonial.DataReferencia;
                    calculoRisco.CalcularFase1(dadoBalancoPatrimonial);
                    calculoRisco.CalcularFase2(dadoBalancoPatrimonial, enumerator.Current);
                    empresa.CalculoRiscoLista.Add(calculoRisco);
                }
            }
            return base.Update(empresa, input);
        }

        private void ProcessarArquivosRobo(Empresa empresa)
        {
            if(empresa?.Documentos?.Any() ?? false)
            {
                foreach (var documento in empresa.Documentos)
                {
                    if (documento.TipoDocumentoId == 172)
                    {
                        empresa.DadosBalancosPatrimoniais = empresa.DadosBalancosPatrimoniais ?? new List<DadosBalancoPatrimonial>();
                        empresa.DadosBalancosPatrimoniais.Add(ProcessarBalancoPatrimonial(documento));
                    }
                    else if (documento.TipoDocumentoId == 88)
                    {
                        empresa.DadosDREs = empresa.DadosDREs ?? new List<DadosDRE>();
                        empresa.DadosDREs.Add(ProcessarDre(documento));
                    }
                }
            }
        }

        private DadosDRE ProcessarDre(DocumentoEmpresa documento)
        {
            return GetService<IDocumentInfoExtractor>().RetornarDadosDRE(documento.Arquivo.Conteudo.FirstOrDefault());
        }

        private DadosBalancoPatrimonial ProcessarBalancoPatrimonial(DocumentoEmpresa documento)
        {
            return GetService<IDocumentInfoExtractor>().RetornarDadosBalancoPatrimonial(documento.Arquivo.Conteudo.FirstOrDefault());
        }
    }

public class TermoAceiteService : ServiceBase<TermosAceite>
{
        public TermoAceiteService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            
        }

        public override TermosAceite Add(TermosAceite termoAceite, IDictionary<string, object> input)
        {

            var empresas = GetRepository<IRepositoryBase<Empresa>>()
                                    .GetWhere(null, x => x.TipoCadastro == termoAceite.TipoCadastro 
                                    && x.TipoEmpresa == termoAceite.TipoFornecedor);

            
            foreach(var empresa in empresas)
            {
                var termoAceiteEmpresa = new TermoAceiteEmpresa
                {
                    EmpresaId = empresa.Id,
                    Aceite = false,
                };
                termoAceite.TermoAceiteEmpresa = new List<TermoAceiteEmpresa>();
                termoAceite.TermoAceiteEmpresa.Add(termoAceiteEmpresa); 
            }                                    
            return base.Add(termoAceite, input);
        }
    }
}
