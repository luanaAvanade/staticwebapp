using Gicaf.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gicaf.Infra.Data.DataSeeder
{
    public static partial class InitialData
    {
        public static List<TipoContato> TiposContato =>
            new List<TipoContato>
            {
                new TipoContato { Id = 1, Nome = "Comercial",   Descricao = "Departamento Comercial",   Status = true },
                new TipoContato { Id = 2, Nome = "Financeiro",  Descricao = "Departamento Financeiro",  Status = true },
                new TipoContato { Id = 3, Nome = "Marketing",   Descricao = "Departamento Marketing",   Status = true },
            };
    }
}
