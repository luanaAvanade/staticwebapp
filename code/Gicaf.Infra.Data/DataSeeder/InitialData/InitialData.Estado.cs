using Gicaf.Domain.Entities;
using Gicaf.Domain.Entities.Fornecedores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gicaf.Infra.Data.DataSeeder
{
    public static partial class InitialData
    {
        public static List<Estado> Estados =>
            new List<Estado>
            {
                new Estado { Id = 1 },
                new Estado { Id = 2 },
                new Estado { Id = 3 },
            };
    }
}
