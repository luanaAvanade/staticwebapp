using Gicaf.Application.Interface.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Application.Interface.Services
{
    public interface IAppService<TDTO>
    {
        TDTO Get(long id);
        IEnumerable<TDTO> GetAll();
        void Add(TDTO obj);
        void Update(TDTO obj);
        void Remove(long id);
    }

    public static class Extensions
    {
        public static void Teste (this IAppService<PerguntaDTO> pergunta)
        {

        }
    }

    public class teste
    {
        public teste()
        {
            IAppService<PerguntaDTO> teste = Activator.CreateInstance<IAppService<PerguntaDTO>>();
            teste.Teste();
        }
    }
}
