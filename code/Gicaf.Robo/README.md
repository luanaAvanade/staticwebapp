# Crawly_cemig-dotnet-pdf

Publicar no IIS a pasta **"/dist"**

Substituir os arquivos **"SinonimosBalanco.csv"** e **"SinonimosDemonstracao.csv"** na 
pasta "/dist/wwwroot" para os arquivos de uso final e/ou sempre que forem criados novas colunas de extração.

Os dois arquivos servem como base na montagem dos itens do JSON de resposta, 
indicando os dados a serem extraídos do PDF de entrada.

Exemplo:

O conteúdo da coluna do **"SinonimosDemonstracao.csv"**

    A1 = **"RECEITA BRUTA"** se torna a propriedade do item `receitaBruta`

    A3 = **"PREJUÍZO ANTES DO IMPOSTO DE RENDA E  DACONTRIBUIÇÃO SOCIAL"** se torna a propriedade do item `prejuizoAntesDoImpostoDeRendaEDacontribuicaoSocial`

    A4 = **"(DESPESAS) RECEITAS OPERACIONAIS"** se torna a propriedade do item `despesasReceitasOperacionais`

A operação de transformação do nome contempla: *CamelCase convertion* e *Diacritics Transformation* (em Unicode)



A separação de itens de origem **B** (SinonimosBalanco) ou **D** (SinonimosDemonstracao) se dá pela propriedade `tipo`

Utilizar o **"/docs/Swagger.yml"** como referência de integração.

