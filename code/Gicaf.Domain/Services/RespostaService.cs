using Gicaf.Domain.Entities;
using Gicaf.Domain.Interfaces.Repository;
using Gicaf.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gicaf.Domain.Services
{
    public class RespostaService : ServiceBase<Resposta>, IRespostaService
    {
        public RespostaService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Resposta Add(Resposta resposta, IDictionary<string, object> input)
        {
            resposta.VersaoMecId = GetRepository<IRepositoryBase<VersaoMec>>()
                                    .GetWhere(null, x => x.DataEncerramento == null).FirstOrDefault().Id;
            return base.Add(resposta, input);
        }

        public IEnumerable<Resposta> GerarRespostas(IQueryNode queryDetails, long perguntaId, long usuarioId)
        {
            var versaoMec = GetRepository<IRepositoryBase<VersaoMec>>().GetWhere(null, x => !x.Encerrado).FirstOrDefault();
            //var perguntaId = GetRepository<IRepositoryBase<Pergunta>>().GetWhere(null, x => x.Codigo == codigoPergunta).FirstOrDefault().Id;

            var result = GetRepository().CountBy(string.Format("Pergunta.Id = {0} and UsuarioId = {1} and VersaoMecId = {2}", perguntaId, usuarioId, versaoMec.Id), null);
            if(result["total"] == 0)
            {
                var tipos = GetRepository<IRepositoryBase<PerguntaGrupoUsuario>>().GetWhere(null, x => x.GrupoUsuario.Usuarios.Any(y => y.Id == usuarioId)).Select(x => x.Tipo);

                foreach (var categoria in GetRepository<IRepositoryBase<Categoria>>().GetWhere(null, x => x.VersaoMecId == versaoMec.Id && (tipos.Any(tipo => tipo == TipoItem.Ambos || tipo == x.Tipo))))
                {
                    GetRepository().Add(new Resposta { PerguntaId = perguntaId, Nota = 0, UsuarioId = usuarioId, VersaoMecId = versaoMec.Id, CategoriaId = categoria.Id }, null);
                }
                GetRepository().SaveChanges();
            }

            return GetRepository().GetWhere(queryDetails, x => x.UsuarioId == usuarioId && x.PerguntaId == perguntaId && x.VersaoMecId == versaoMec.Id);
        }
    }
}
