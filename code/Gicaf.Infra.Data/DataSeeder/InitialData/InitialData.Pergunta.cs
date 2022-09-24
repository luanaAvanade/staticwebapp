using Gicaf.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.DataSeeder
{
    public static partial class InitialData
    {
        public static List<Pergunta> Perguntas =>
            new List<Pergunta>
            {
                new Pergunta { Id = 1, Codigo = 1, Nome = "Volume de Compras (R../../)",                                         Descricao = "Volume de Compras (R../../)",                                       Eixo = Eixo.X, OrigemDados = OrigemDados.Sistema },
                new Pergunta { Id = 2, Codigo = 2, Nome = "Impacto da falta de material/serviço nas operações da Cemig",    Descricao = "Impacto do item/serviço na operação da CEMIG (garantia de abastecimento). <br>Nota 10: Alto - paralisa um processo crítico da CEMIG com dificuldade de retorno à normalidade operacional. <br> Nota 1: Baixo - Não paralisa processo crítico da Cemig sem dificuldade de retorno à normalidade operacional.",  Eixo = Eixo.X, OrigemDados = OrigemDados.Formulario },
                new Pergunta { Id = 3, Codigo = 3, Nome = "Nível de exigência requerido do fornecedor",                     Descricao = "Representatividade do volume de compras da CEMIG frente ao mercado. <br>Nota 10: Alto - sem tolerância/desvio em relação à especificação técnica.<br>Nota 1: Baixo - alguma tolerância/desvio em relação à especificação técnica.",                   Eixo = Eixo.X, OrigemDados = OrigemDados.Formulario },
                new Pergunta { Id = 4, Codigo = 4, Nome = "Prazo médio para substituição de um fornecedor",                 Descricao = "Prazo médio para substituição de um fornecedor",               Eixo = Eixo.X, OrigemDados = OrigemDados.Sistema },
                new Pergunta { Id = 5, Codigo = 5, Nome = "Número de Fornecedores que atendam à Cemig",                     Descricao = "Número de Fornecedores que atendam à Cemig",                   Eixo = Eixo.Y, OrigemDados = OrigemDados.Sistema },
                new Pergunta { Id = 6, Codigo = 6, Nome = "Grau de regulamentação para a categoria",                        Descricao = "Grau de regulamentação para a categoria",                      Eixo = Eixo.Y, OrigemDados = OrigemDados.Sistema },
                new Pergunta { Id = 7, Codigo = 7, Nome = "Escala da CEMIG relativa à indústria",                           Descricao = "Percepção do gestor da categoria sobre a representatividade (atratividade e volume de compras) da CEMIG frente ao mercado, ou seja, qual o tamanho da atratividade / visibilidade de CEMIG para a carteira do fornecedor.<br>Nota 10: Alto – Atratividade e demanda da CEMIG não influenciam totalmente o Mercado Fornecedor desta Categoria.<br>Nota 1: Baixo - Atratividade e demanda da CEMIG influenciam totalmente o Mercado Fornecedor desta Categoria.",                         Eixo = Eixo.Y, OrigemDados = OrigemDados.Formulario },
                new Pergunta { Id = 8, Codigo = 8, Nome = "Maturidade do Mercado",                                          Descricao = "Qual a percepção do gestor de categoria sobre o grau de maturidade/ confiabilidade do mercado fornecedor. Para essa questão o resultado<br>Nota 10: Alto - Mercado em estágio inicial ou com alta volatilidade.<br>Nota 1: Baixo - Mercado maduro, consolidado, com fornecedores atuando há mais de 5 anos.",                                        Eixo = Eixo.Y, OrigemDados = OrigemDados.Formulario },
            };
    }
}
