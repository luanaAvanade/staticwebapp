using Gicaf.Domain.Entities.Base;

namespace Gicaf.Domain.Entities.Fornecedores
{
    public class DadosBancarios : BaseTrailEntity
    {
        public  Banco Banco { get; set; }
        public long? BancoId  { get; set; }
        public string Agencia  { get; set; }
        public int? DigitoAgencia  { get; set; }
        public int? Conta  { get; set; }
        public int? DigitoConta  { get; set; }
    }
}