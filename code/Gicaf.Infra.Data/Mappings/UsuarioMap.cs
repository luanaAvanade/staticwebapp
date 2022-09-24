using Gicaf.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings
{
    public class UsuarioMap : BaseMap<Usuario>
    {
        public UsuarioMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }

        public UsuarioMap() : this(new DefaultMapSettings())
        {

        }
        public override void Configure(EntityTypeBuilder<Usuario> builder)
        {
            base.Configure(builder);
            builder.HasOne(_ => _.GrupoUsuario).WithMany(_ => _.Usuarios).HasForeignKey(_ => _.GrupoUsuarioId);
            builder.HasOne(_ => _.Empresa).WithMany(_ => _.Usuarios).HasForeignKey(_ => _.EmpresaId);
            builder.Ignore(_ => _.UserName);
            builder.Ignore(_ => _.PassWord);
            builder.Ignore(_ => _.ConfirmPassWord);
            builder.Ignore(_ => _.LinkConfirmacao);
            builder.Ignore(_ => _.Roles);
        }
    }
}
