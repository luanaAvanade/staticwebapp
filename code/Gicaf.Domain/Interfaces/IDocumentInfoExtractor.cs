using Gicaf.Domain.Entities.Fornecedores;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Domain.Interfaces
{
public interface IDocumentInfoExtractor
{
DadosBalancoPatrimonial RetornarDadosBalancoPatrimonial(byte[] conteudo);
DadosDRE RetornarDadosDRE(byte[] conteudo);
}
}