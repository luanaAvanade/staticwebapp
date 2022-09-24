using Bogus;
using Gicaf.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.DataSeeder
{
    public static partial class FakeData
    {
        static List<Usuario> usuarios;

        public static List<Usuario> Usuarios
        {
            get 
            {
                usuarios = usuarios ??
                new Faker<Usuario>()
                .RuleFor(o => o.Nome, f => f.Name.FullName())
                .RuleFor(o => o.Email, f => f.Internet.Email())
                .RuleFor(o => o.CPF, f => "00000000000")
                .Generate(1);

                return usuarios;
            }
        }     
    }
}
