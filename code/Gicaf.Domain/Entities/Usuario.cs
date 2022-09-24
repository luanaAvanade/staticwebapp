using Gicaf.Domain.Entities.Base;
using Gicaf.Domain.Entities.Fornecedores;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Domain.Entities
{
    public class Usuario : BaseTrailEntity
    {
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; } 
        public string Email { get; set; }
        public string CargoEmpresa { get; set; }
        public long? GrupoUsuarioId { get; set; }
        public GrupoUsuario GrupoUsuario { get; set; }
        public ICollection<Resposta> Respostas { get; set; }

        public long? EmpresaId { get; set; }
        public Empresa Empresa { get; set; }

        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string ConfirmPassWord { get; set; }
        public string LinkConfirmacao { get; set; }
        public ICollection<string> Roles { get; set; }
            
    }
}
