using Gicaf.Domain.Entities.Fornecedores;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings.FornecedorMap
{
    public class DocumentoEmpresaMap : BaseMap<DocumentoEmpresa>
    {
        public DocumentoEmpresaMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }
        public DocumentoEmpresaMap() : this(new DefaultMapSettings())
        {

        }

        public override void Configure(EntityTypeBuilder<DocumentoEmpresa> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.Arquivo).WithMany().HasForeignKey(x => x.ArquivoId);
            builder.HasOne(x => x.Empresa).WithMany(x => x.Documentos).HasForeignKey(x => x.EmpresaId);
        }
    }
}
