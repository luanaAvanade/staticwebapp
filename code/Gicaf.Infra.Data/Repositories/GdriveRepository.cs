using GDrive;
using Gicaf.Domain.Entities.Fornecedores;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GDrive.GEDServiceClient;

namespace Gicaf.Infra.Data.Repositories
{
    public interface IGdriveRepository
    {
        byte[] BuscarConteudoArquivo(string id);
    }

    public class GdriveRepository: IGdriveRepository
    {
        private readonly string URL;
        private readonly Credenciais Credenciais;
        private GEDServiceClient Client;

        private const string _idEmpresaMetadado = "IdEmpresa";

        private const string _fichaDadosBasicos = "DadosBasicos";
        //private const string _arquivoDadosBasicos = "DadosBasicos";

        private const string _fichaBalancoPatrimonial = "BalancoPatrimonial";

        private const string _arquivoBalancoPatrimonial = "BalancoPatrimonial_{0}";

        private const string _fichaDRE = "DRE";
        private const string _arquivoDRE = "DRE_{0}";

        private const string _fichaQualificacoes = "Qualificacoes";
        private const string _arquivoQualificacoes = "Qualificacoes_{0}";

        public GdriveRepository(string url, string login, string senha)
        {
            URL = url;
            Credenciais = new Credenciais { Login = login, Senha = senha };
            Client = new GEDServiceClient(EndpointConfiguration.WSHttpBinding_GEDService);
        }

        private Task<Arquivo> AnexarArquivo(string ficha, string nomeArquivo, string extensaoArquivo, byte[] conteudoArquivo, List<Metadado> metadados = null,  bool controlarversao = true)
        {
            var dadosArquivoCriacao = new DadosArquivoCriacao
            {
                Nome = nomeArquivo,
                Extensao = extensaoArquivo,
                ConteudoBase64Binary = conteudoArquivo,
                ConfiguracoesOpcionais = new ConfigAnexarArquivo
                {
                    EmitirErroSeArquivoJaExiste = !controlarversao
                },
                Ficha = new DadosFicha
                {
                    Nome = ficha,
                    Metadados = metadados
                }
            };

            return Client.AnexarArquivoAsync(Credenciais, dadosArquivoCriacao, null);
        }

        private Task<Arquivo> AnexarDocumentoDaEmpresa(string idEmpresa, string ficha, string nomeDocumento, string extensaoArquivo, byte[] conteudoArquivo, bool controlarversao = true)
        {
            var metadados = new List<Metadado>() { new Metadado { Atributo = _idEmpresaMetadado, Valor = idEmpresa } };
            return AnexarArquivo(ficha, nomeDocumento, extensaoArquivo, conteudoArquivo, metadados);
        }
        /*
                private Task<Ficha> CriarFichaAsync(string nome, bool verificarSeExiste = true) =>
                   Client.CriarFichaAsync(Credenciais, new DadosFicha { Nome = nome, NaoVerificarSeExiste = !verificarSeExiste }, null);
        */

        public byte[] BuscarConteudoArquivo(string id) => BuscarConteudoArquivoAsync(id).Result.ConteudoBase64Binary;

        internal Task<Arquivo> BuscarConteudoArquivoAsync(string id) =>
            Client.BuscarConteudoArquivoAsync(Credenciais, new DadosArquivoRecuperacao { ID = id }, null);

        internal Task<Arquivo> AnexarDadosBasicosDaEmpresa(string idEmpresa, string extensaoArquivo, byte[] conteudoArquivo) =>
            AnexarDocumentoDaEmpresa(idEmpresa, ../../"{_fichaDadosBasicos}_{idEmpresa}", _fichaDadosBasicos, extensaoArquivo, conteudoArquivo);

        internal Task<Arquivo> AnexarBalancoPatrimonialDaEmpresa(string idEmpresa, int ano, string extensaoArquivo, byte[] conteudoArquivo) =>
            AnexarDocumentoDaEmpresa(idEmpresa, ../../"{_fichaBalancoPatrimonial}_{idEmpresa}", ../../"{_fichaBalancoPatrimonial}_{ano}", extensaoArquivo, conteudoArquivo);

        internal Task<Arquivo> AnexarDREdaEmpresa(string idEmpresa, int ano, string extensaoArquivo, byte[] conteudoArquivo) =>
            AnexarDocumentoDaEmpresa(idEmpresa, ../../"{_fichaDRE}_{idEmpresa}", ../../"{_fichaDRE}_{ano}", extensaoArquivo, conteudoArquivo);

//NÃO IMPLEMENTADO ATÉ O MOMENTO
/*
        internal Task<List<Ficha>> PesquisarDocumentosDaEmpresa(string id) =>
            Client.PesquisarDocumentosPorMetadadosAsync(Credenciais, new List<Metadado> { new Metadado { Atributo = _idEmpresaMetadado, Valor = id } }, null);
*/
        internal Task<Ficha> ListarDadosBasicosDaEmpresa(string id) =>
            Client.ListarArquivosFichaAsync(Credenciais, new DadosFicha { Nome = ../../"{_fichaDadosBasicos}_{id}", NaoVerificarSeExiste = true }, null);

        internal Task<Ficha> ListarBalancoPatrimonialDaEmpresa(string id) =>
            Client.ListarArquivosFichaAsync(Credenciais, new DadosFicha { Nome = ../../"{_fichaBalancoPatrimonial}_{id}", NaoVerificarSeExiste = true }, null);

        internal Task<Ficha> ListarDREdaEmpresa(string id) =>
            Client.ListarArquivosFichaAsync(Credenciais, new DadosFicha { Nome = ../../"{_fichaDRE}_{id}", NaoVerificarSeExiste = true }, null);

        internal void AnexarDocumentoDaEmpresa(DocumentoEmpresa documentoEmpresa)
        {
            var arquivo = documentoEmpresa.Arquivo;
            var empresa = documentoEmpresa.Empresa;
            Arquivo arquivoGdrive = null;
            
        if(documentoEmpresa.TipoDocumento.Codigo == 87){
        //verifica se existe no banco, caso exista, realiza o processo normalmente, caso não exista, salva 
        }

        if(documentoEmpresa.TipoDocumento.Codigo == 172){
        //verifica se existe no banco, caso exista, realiza o processo normalmente, caso não exista, salva 
        }


            //arquivo.Caminho = URL;
            if(arquivo.Conteudo.FirstOrDefault().Length>0)
            switch (documentoEmpresa.TipoDocumentoId) // 05015444000114?? Id empresa???
            {
                case 82:case 83:case 84://TipoDocumento.DadosBasicos:
                    arquivo.CodigoExterno = AnexarDadosBasicosDaEmpresa(documentoEmpresa.EmpresaId.ToString(), arquivo.Extensao, arquivo.Conteudo.FirstOrDefault()).Result.ID;
                    break;
                 case 87:case 88://TipoDocumento.BalancoPatrimonial:
                    arquivoGdrive = AnexarBalancoPatrimonialDaEmpresa(documentoEmpresa.EmpresaId.ToString(), documentoEmpresa.DataBasePeriodo.Year, arquivo.Extensao, arquivo.Conteudo.FirstOrDefault()).Result;
                    break;
                 case 89:case 90://TipoDocumento.Dre:
                    arquivoGdrive = AnexarDREdaEmpresa(documentoEmpresa.EmpresaId.ToString(), documentoEmpresa.DataBasePeriodo.Year, arquivo.Extensao, arquivo.Conteudo.FirstOrDefault()).Result;
                    break;
                default:
                    break;
            }
           // arquivo.CodigoExterno = arquivoGdrive.ID;
            //arquivo.URL = arquivoGdrive.Ficha.URLAcessoViaWebClient;
        }
    }
}   
