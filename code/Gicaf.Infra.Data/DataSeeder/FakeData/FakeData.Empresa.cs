using Bogus;
using Gicaf.Domain.Entities;
using Gicaf.Domain.Entities.Fornecedores;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.DataSeeder
{
    public static partial class FakeData
    {
        static List<Empresa> empresas;

        public static List<Empresa> Empresas
        {
            get
            {
                empresas = empresas ??
                new Faker<Empresa>()
                .RuleFor(o => o.NomeEmpresa, f => f.Company.CompanyName())
                .RuleFor(o => o.IsentoIE, f => f.PickRandom(true, false))
                .RuleFor(o => o.CNPJ, f => "00000000000")
                .Generate(1);
                
                return empresas;
            }
        }
    }
}
