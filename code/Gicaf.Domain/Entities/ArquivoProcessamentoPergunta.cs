using Gicaf.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Domain.Entities
{
    public enum StatusProcessamento
    {
        Aguardando,
        Processado,
        Erro
    }

    public class ArquivoProcessamentoPergunta: BaseTrailEntity
    {
        public long PerguntaId { get; set; }
        public Pergunta Pergunta { get; set; }
        public long ArquivoId { get; set; }
        public Arquivo Arquivo { get; set; }
        public DateTime? DataProcessamento { get; set; }
        public StatusProcessamento Status { get; set; }
        public string Mensagem { get; set; }
        public TipoItem? Tipo { get; set; }
    }

    public enum OrigemArquivo
    {
        SistemaDeArquivos,
        BancoDeDados,
        Gdrive
    }

    public class Arquivo : BaseTrailEntity
    {
        public string CodigoExterno { get; set; }
        public string Key { get; set; }
        public string Caminho { get; set; }
        public string NomeArquivo { get; set; }
        public string Extensao { get; set; }
        public string URL { get; set; }
        public IEnumerable<byte[]> Conteudo { get; set; }

        public IEnumerable<string> CaminhoCompleto {
            get
            {
                var caminhos = Caminho.Split(";");
                var extensoes = Extensao.Split(";");
                var nomes = NomeArquivo.Split(";");
                for (int i = 0; i < nomes.Length; i++)
                {
                    yield return string.Format("{0}\\{1}{2}", caminhos[i], nomes[i], extensoes[i]);
                } 
            }
        }

        public OrigemArquivo Origem { get; set; }
    }
}
