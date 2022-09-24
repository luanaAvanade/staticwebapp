import theme from '@/theme';
import { translate } from '@/locales';

//const URL_MATRIZ = 'https://charts.qlikcloud.com/5d1e4a00de1aed0010970308/chart.html';

const URL_MATRIZ = window.globalConfig.UrlQlikSense;

const MATERIAL = { id: 0, codigo: 'MATERIAL', nome: 'Material' };

const SERVICO = { id: 1, codigo: 'SERVICO', nome: 'Serviço' };

const SISTEMA = { id: 0, codigo: 'SISTEMA', nome: 'Sistema' };

const FORMULARIO = { id: 1, codigo: 'FORMULARIO', nome: 'Formulário' };

const PRINCIPAL = { codigo: 'PRINCIPAL' };

const FUNCIONALIDADE = {
	CADASTRO_PESSOA_JURIDICA: 2,
	Cadastro_Financeiro_Pessoa_Juridica: 8
};

const TIPO_DOCUMENTO = {
	Balanco_Patrimonial: 'Balanço Patrimonial (BP)',
	Demonstracao_Resultado_Exercicio: 'Demonstração do Resultado do Exercício (DRE)'
};

const TIPO_RESPOSTA = [
	{
		codigo: 1,
		value: 'Numero_Inteiro',
		label: 'Número Inteiro'
	},
	{
		codigo: 2,
		value: 'Numero_Decimal',
		label: 'Número Decimal'
	},
	{
		codigo: 3,
		value: 'Texto_Livre',
		label: 'Texto Livre'
	},
	{
		codigo: 4,
		value: 'Sim_Nao',
		label: 'Sim/Não'
	},
	{
		codigo: 5,
		value: 'Lista',
		label: 'Lista'
	},
	{
		codigo: 6,
		value: 'Data',
		label: 'Data'
	},
	{
		codigo: 7,
		value: 'Hora',
		label: 'Hora'
	}
];

const PERFIS = [
	{
		codigo: 1,
		value: 'Fornecedor',
		label: 'Fornecedor'
	},
	{
		codigo: 2,
		value: 'Analista',
		label: 'Analista'
	}
];

const ARQUIVO_PROCESSADO = 1;

const ROWSPERPAGE = [
	5,
	10,
	15,
	20,
	25
];

const SELECT_TIPO = [
	{ value: 'MATERIAL', label: 'Material' },
	{ value: 'SERVICO', label: 'Serviço' }
];
const SELECT_TIPO_CATEGORIA = [
	{ codigo: 0, value: 'MATERIAL', label: 'Material' },
	{ codigo: 1, value: 'SERVICO', label: 'Serviço' },
	{ codigo: 2, value: 'AMBOS', label: 'Ambos' }
];

const FABRICANTE = { id: 0, codigo: 'FABRICANTE', nome: 'Fabricante' };
const DISTRIBUIDOR = { id: 1, codigo: 'DISTRIBUIDOR', nome: 'Distribuidor' };

const SELECT_TIPO_FORNECEDOR = [
	{ value: 'FABRICANTE', label: 'Fabricante', valorEnum: 0 },
	{ value: 'DISTRIBUIDOR', label: 'Distribuidor', valorEnum: 1 }
];

const GRUPO_CATEGORIA = { codigo: 'grupoCategoria', nome: 'Grupo de Categoria' };
const CATEGORIA = { codigo: 'categoria', nome: 'Categoria' };
const SKU = { codigo: 'sku', nome: 'Sku' };

const SELECT_GRUPOS = [
	{ value: 'grupoCategoria', label: 'Grupo de Categoria' },
	{ value: 'categoria', label: 'Categoria' },
	{ value: 'sku', label: 'Sku' }
];

const CATEGORIAS = [
	{
		value: 'M001',
		label: 'M001 - BATERIA, INVERSOR, RETIFICADOR E CARREGADOR DE ENERGIA E ACESSÓRIO'
	},
	{ value: 'M002', label: 'M002 - CONDUTOR ELÉTRICO PARA REDES DE DISTRIBUIÇÃO' },
	{ value: 'M003', label: 'M003 - CONDUTOR ELÉTRICO PARA SISTEMAS DE TRANSMISSÃO' },
	{
		value: 'M004',
		label: 'M004 - COMBUSTÍVEL, LUBRIFICANTE E ÓLEO ISOLANTE DE DEMANDA REPETITIVA'
	},
	{
		value: 'M005',
		label: 'M005 - COMBUSTÍVEL, LUBRIFICANTE E ÓLEO ISOLANTE PARA APLICAÇÃO DIRETA'
	},
	{ value: 'M006', label: 'M006 - CONEXÃO, LUVA, EMENDA E ACESSÓRIO PARA REDES DE DISTRIBUIÇÃO' },
	{
		value: 'M007',
		label: 'M007 - CONEXÃO, LUVA, EMENDA E ACESSÓRIO PARA SISTEMAS DE TRANSMISSÃO'
	},
	{
		value: 'M008',
		label: 'M008 - DISPOSITIVO DE MANOBRA, PROTEÇÃO E COMANDO PARA REDES DE DISTRIBUIÇÃO'
	},
	{
		value: 'M009',
		label: 'M009 - DISPOSITIVO DE MANOBRA, PROTEÇÃO E COMANDO PARA SISTEMAS DE TRANSMISSÃO'
	},
	{ value: 'M010', label: 'M010 - EQUIPAMENTO DE SEGURANÇA E UNIFORME DE DEMANDA REPETITIVA' },
	{ value: 'M011', label: 'M011 - EQUIPAMENTO DE SEGURANÇA E UNIFORME PARA APLICAÇÃO DIRETA' },
	{
		value: 'M012',
		label: 'M012 - EQUIPAMENTO E ACESSÓRIO PARA CONSTRUÇÃO CIVIL E MANUTENÇÃO PREDIAL'
	},
	{
		value: 'M013',
		label: 'M013 - EQUIPAMENTO E ACESSÓRIO PARA MEDIÇÃO DE ENERGIA DA DISTRIBUIÇÃO'
	},
	{
		value: 'M014',
		label: 'M014 - EQUIPAMENTO E ACESSÓRIO PARA MEDIÇÃO, COMANDO, CONTROLE DE ENERGIA'
	},
	{ value: 'M015', label: 'M015 - EQUIPAMENTO E ACESSÓRIO PARA TELECOMUNICAÇÃO' },
	{ value: 'M016', label: 'M016 - EQUIPAMENTO DE SINALIZAÇÃO E ALARME DE DEMANDA REPETITIVA' },
	{ value: 'M017', label: 'M017 - EQUIPAMENTO DE SINALIZAÇÃO E ALARME PARA APLICAÇÃO DIRETA' },
	{ value: 'M018', label: 'M018 - EQUIPAMENTO E FERRAMENTA PARA LINHA VIVA' },
	{ value: 'M019', label: 'M019 - EQUIPAMENTO, ACESSÓRIO E UTENSÍLIO PARA ESCRITÓRIO' },
	{
		value: 'M020',
		label: 'M020 - EQUIPAMENTO, PERIFÉRICO E ACESSÓRIO PARA INFORMÁTICA E COMUNICAÇÃO'
	},
	{ value: 'M021', label: 'M021 - ESTRUTURA METÁLICA DE DEMANDA REPETITIVA' },
	{ value: 'M022', label: 'M022 - ESTRUTURA METÁLICA PARA APLICAÇÃO DIRETA' },
	{
		value: 'M023',
		label: 'M023 - FERRAGEM ELETROTÉCNICA, PARAFUSO E FIXAÇÃO DE DEMANDA REPETITIVA'
	},
	{
		value: 'M024',
		label: 'M024 - FERRAGEM ELETROTÉCNICA, PARAFUSO E FIXAÇÃO PARA APLICAÇÃO DIRETA'
	},
	{ value: 'M025', label: 'M025 - GERADOR, TURBINA, PAINEL E COMPONENTE PARA GERAÇÃO DE ENERGIA' },
	{ value: 'M026', label: 'M026 - INSTRUMENTO DE MEDIÇÃO DE GRANDEZAS DE DEMANDA REPETITIVA' },
	{ value: 'M027', label: 'M027 - INSTRUMENTO DE MEDIÇÃO DE GRANDEZAS PARA APLICAÇÃO DIRETA' },
	{ value: 'M028', label: 'M028 - LÂMPADA E EQUIPAMENTO PARA ILUMINAÇÃO DE DEMANDA REPETITIVA' },
	{ value: 'M029', label: 'M029 - LÂMPADA E EQUIPAMENTO PARA ILUMINAÇÃO PARA APLICAÇÃO DIRETA' },
	{
		value: 'M030',
		label: 'M030 - MÁQUINA, EQUIPAMENTO E FERRAMENTA DE TRABALHO DE DEMANDA REPETITIVA'
	},
	{
		value: 'M031',
		label: 'M031 - MÁQUINA, EQUIPAMENTO E FERRAMENTA DE TRABALHO PARA APLICAÇÃO DIRETA'
	},
	{ value: 'M032', label: 'M032 - EMBALAGEM DE MATERIAL DE DEMANDA REPETITIVA' },
	{ value: 'M033', label: 'M033 - MATERIAIS DIVERSOS' },
	{ value: 'M034', label: 'M034 - MATERIAL E ACESSÓRIO GRÁFICO' },
	{ value: 'M035', label: 'M035 - MATERIAL PARA INSTALAÇÃO ELÉTRICA BT DE DEMANDA REPETITIVA' },
	{ value: 'M036', label: 'M036 - MATERIAL PARA INSTALAÇÃO ELÉTRICA BT PARA APLICAÇÃO DIRETA' },
	{ value: 'M037', label: 'M037 - MOTOR, ROLAMENTO, COMPRESSOR E BOMBA' },
	{ value: 'M038', label: 'M038 - MÓVEL PARA ESCRITÓRIO' },
	{ value: 'M039', label: 'M039 - PÁRA-RAIOS E ISOLADOR PARA REDES DE DISTRIBUIÇÃO' },
	{ value: 'M040', label: 'M040 - PÁRA-RAIOS E ISOLADOR PARA SISTEMAS DE TRANSMISSÃO' },
	{
		value: 'M041',
		label: 'M041 - POSTE, ESTRUTURA, CRUZETA E ACESSÓRIO DE CONCRETO PARA REDES DE DISTRIBUIÇÃO'
	},
	{
		value: 'M042',
		label: 'M042 - POSTE, ESTRUTURA, CRUZETA E ACESSÓRIO DE CONCRETO PARA SUBESTAÇÃO'
	},
	{
		value: 'M043',
		label: 'M043 - POSTE, ESTRUTURA, CRUZETA E ACESSÓRIO DE FIBRA, POLÍMERO E METAL'
	},
	{ value: 'M044', label: 'M044 - POSTE, ESTRUTURA, CRUZETA E ACESSÓRIO DE MADEIRA' },
	{ value: 'M045', label: 'M045 - PRODUTO E COMPOSTO QUÍMICO' },
	{ value: 'M046', label: 'M046 - REATOR DE POTÊNCIA E AMORTECIMENTO' },
	{ value: 'M047', label: 'M047 - RELÉ, SENSOR, TRANSDUTOR E ACESSÓRIO' },
	{ value: 'M048', label: 'M048 - SUBESTAÇÃO MÓVEL, UNITÁRIA E GIS' },
	{ value: 'M049', label: 'M049 - TRANSFORMADOR E REGULADOR DE POTÊNCIA' },
	{ value: 'M050', label: 'M050 - TRANSFORMADOR E REGULADOR PARA DISTRIBUIÇÃO' },
	{ value: 'M051', label: 'M051 - TRANSFORMADOR PARA INSTRUMENTOS DE DEMANDA REPETITIVA' },
	{ value: 'M052', label: 'M052 - TRANSFORMADOR PARA INSTRUMENTOS DE APLICAÇÃO DIRETA' },
	{ value: 'M053', label: 'M053 - TUBO, ELETRODUTO E MANGUEIRA DE DEMANDA REPETITIVA' },
	{ value: 'M054', label: 'M054 - TUBO, ELETRODUTO E MANGUEIRA PARA APLICAÇÃO DIRETA' },
	{ value: 'M055', label: 'M055 - VÁLVULA, COMPONENTE E ACESSÓRIO' },
	{ value: 'M056', label: 'M056 - VEÍCULO, EQUIPAMENTO E ACESSÓRIO PARA TRANSPORTE' },
	{ value: 'S001', label: 'S001 - FISCALIZAÇÃO DE LINHAS DE TRANSMISSÃO' },
	{ value: 'S002', label: 'S002 - INSPEÇÃO EM SERVIÇOS DE DISTRIBUIÇÃO' },
	{ value: 'S003', label: 'S003 - LIMPEZA DE FAIXA' },
	{ value: 'S004', label: 'S004 - OBRAS E SERVIÇOS EM RD' },
	{ value: 'S005', label: 'S005 - PROJETOS BÁSICOS E EXECUTIVOS PARA MT/BT' },
	{
		value: 'S006',
		label: 'S006 - CONSTRUÇÃO/INSTALAÇÃO, MONTAGEM E TESTES SE/LT/USINAS/EDIFICAÇÕES'
	}
];

const SUBCATEGORIAS = [
	{
		value: '6110',
		IdCategoria: 'M001',
		label: '6110 - ALTERNADOR CONVERSOR E INVERSOR INDUSTRIAL'
	},
	{
		value: '6115',
		IdCategoria: 'M001',
		label: '6115 - RETIFICADOR E CARREGADOR BATERIA INDUSTRIAL'
	},
	{ value: '6117', IdCategoria: 'M001', label: '6117 - SISTEMA NO-BREAK' },
	{
		value: '6132',
		IdCategoria: 'M001',
		label: '6132 - BATERIA PORTÁTIL EQUIPAMENTO E INSTRUMENTO'
	},
	{ value: '6133', IdCategoria: 'M001', label: '6133 - BATERIA ESTACIONÁRIA' },
	{ value: '6134', IdCategoria: 'M001', label: '6134 - BATERIA RECARREGÁVEL' },
	{ value: '6135', IdCategoria: 'M001', label: '6135 - PILHA' },
	{ value: '6136', IdCategoria: 'M001', label: '6136 - CARREGADOR BATERIA PORTÁTIL' },
	{
		value: '6137',
		IdCategoria: 'M001',
		label: '6137 - COMPONENTE E ACESSÓRIO P/BATERIA AUTOMOTIVA E ESTACIONÁRIA'
	},
	{ value: '6150', IdCategoria: 'M001', label: '6150 - FONTE ALIMENTAÇÃO' },
	{
		value: '6151',
		IdCategoria: 'M001',
		label: '6151 - FONTE-CARREGADOR BATERIA P/TRANSCEPTOR UHF/VHF'
	},
	{ value: '2101', IdCategoria: 'M002', label: '2101 - CONDUTOR NU ALUMÍNIO' },
	{ value: '2102', IdCategoria: 'M002', label: '2102 - CONDUTOR NU COBRE' },
	{
		value: '2104',
		IdCategoria: 'M002',
		label: '2104 - CONDUTOR COBRE P/BY-PASS E ATERRAMENTO TEMPORÁRIO'
	},
	{ value: '2105', IdCategoria: 'M002', label: '2105 - CONDUTOR AÇO-COBREADO E AÇO-ALUMINIZADO' },
	{ value: '2106', IdCategoria: 'M002', label: '2106 - JUMPER CORDOALHA CONDUTORA COBRE FLEXÍVEL' },
	{ value: '2110', IdCategoria: 'M002', label: '2110 - CONDUTOR PROTEGIDO DE ALUMÍNIO P/RDA' },
	{ value: '2112', IdCategoria: 'M002', label: '2112 - CONDUTOR NU ALUMÍNIO ESPECIAL' },
	{
		value: '2116',
		IdCategoria: 'M002',
		label: '2116 - CONDUTOR DE ALUMÍNIO C/ISOLAÇÃO TERMOFIXA ATÉ 1kV'
	},
	{
		value: '2117',
		IdCategoria: 'M002',
		label: '2117 - CONDUTOR DE ALUMÍNIO C/ISOLAÇÃO TERMOFIXA DE 1kV ATÉ 25kV'
	},
	{
		value: '2120',
		IdCategoria: 'M002',
		label: '2120 - CONDUTOR DE COBRE C/ISOLAÇÃO TERMOPLÁSTICA ATÉ 1kV'
	}
];

const BALANCO_PATRIMONIAL = [
	{ codigo: 'AtivoTotal', label: 'ATIVO TOTAL', negrito: true, paddingLeft: theme.spacing(0) },
	{ codigo: 'CirculanteAtivo', label: 'Circulante', negrito: true, paddingLeft: theme.spacing(3) },
	{ codigo: 'Disponibilidades', label: 'Disponibilidades', paddingLeft: theme.spacing(3) },
	{ codigo: 'Estoques', label: 'Estoques', paddingLeft: theme.spacing(3) },
	{
		codigo: 'OutrosAtivosCirculante',
		label: 'Outros Ativos Circulante',
		paddingLeft: theme.spacing(3)
	},
	{
		codigo: 'AtivoNaoCirculante',
		label: 'Não Circulante',
		negrito: true,
		paddingLeft: theme.spacing(3)
	},
	{ codigo: 'PassivoTotal', label: 'PASSIVO TOTAL', negrito: true, paddingLeft: theme.spacing(0) },
	{
		codigo: 'CirculantePassivo',
		label: 'Circulante',
		negrito: true,
		paddingLeft: theme.spacing(3)
	},
	{
		codigo: 'EmprestimosFinanciamentoCirculante',
		label: 'Empréstimos e Financiamentos',
		paddingLeft: theme.spacing(3)
	},
	{
		codigo: 'OutrosPassivosCirculantes',
		label: 'Outros Passivos Circulantes',
		paddingLeft: theme.spacing(3)
	},
	{
		codigo: 'NaoCirculantePassivo',
		label: 'Não Circulante',
		negrito: true,
		paddingLeft: theme.spacing(3)
	},
	{
		codigo: 'EmprestimosFinanciamentoNaoCirculante',
		label: 'Empréstimos e financiamento',
		paddingLeft: theme.spacing(3)
	},
	{
		codigo: 'OutrosPassivosNaoCirculantes',
		label: 'Outros Passivos Não Circulantes',
		paddingLeft: theme.spacing(3)
	},
	{
		codigo: 'PatrimonioLiquido',
		label: 'Patrimônio Líquido',
		negrito: true,
		paddingLeft: theme.spacing(3)
	}
];

const DRE = [
	{
		codigo: 'ReceitaOperacionalLiquida',
		label: 'Receita Operacional Líquida',
		negrito: true,
		paddingLeft: theme.spacing(0)
	},
	{
		codigo: 'CustoProdutosVendidosMercadoriasVendidasServicosPrestados',
		label: '(-) Custo dos produtos vendidos / mercadorias vendidas / serviços prestados',
		paddingLeft: theme.spacing(3)
	},
	{
		codigo: 'ResultadoOperacionalBruto',
		label: 'Resultado Operacional Bruto',
		negrito: true,
		paddingLeft: theme.spacing(0)
	},
	{
		codigo: 'DespesasVendasAdministrativasGeraisOutras',
		label: '(-) Despesas de vendas, administrativas, gerais e outras',
		paddingLeft: theme.spacing(3)
	},
	{
		codigo: 'DespesasFinanceiras',
		label: '(-) Despesas financeiras',
		paddingLeft: theme.spacing(3)
	},
	{
		codigo: 'ReceitasFinanceiras',
		label: '(+) Receitas financeiras',
		paddingLeft: theme.spacing(3)
	},
	{
		codigo: 'ResultadoOperacionalAntesIrCssl',
		label: 'Resultado Operacional Antes do IR e CSSL',
		negrito: true,
		paddingLeft: theme.spacing(0)
	},
	{
		codigo: 'ResultadoLiquidoPeriodo',
		label: 'Resultado Líquido do Período',
		negrito: true,
		paddingLeft: theme.spacing(0)
	}
];

const SELECT_NOTAS = [
	{ value: 1, label: '1' },
	{ value: 2, label: '2' },
	{ value: 3, label: '3' },
	{ value: 4, label: '4' },
	{ value: 5, label: '5' },
	{ value: 6, label: '6' },
	{ value: 7, label: '7' },
	{ value: 8, label: '8' },
	{ value: 9, label: '9' },
	{ value: 10, label: '10' }
];

const SELECT_TIPO_CONTATO = [
	{ value: 1, label: 'Comercial' },
	{ value: 2, label: 'Financeiro' },
	{ value: 3, label: 'Marketing' }
];

const TIPO_EMPRESA = [
	{
		value: '1',
		label: 'Pessoa Juridica Nacional',
		align: 'center',
		internalName: 'pessoaJuridicaNacional'
	},
	{
		value: '2',
		label: 'Micro Empreendedor Individual(MEI)',
		align: 'center',
		internalName: 'microEmpreendedorIndividual'
	}
];

const TAB_DADOS_BASICOS = 0;
const TAB_DADOS_COMPLEMENTAR = 1;
const TAB_DADOS_FINANCEIROS = 2;
const TAB_DADOS_SOCIOS = 4;
const TAB_QUALIFICACAO = 3;

const CADASTRO_CRIADO = 0;
const CADASTRO_PENDENTE_ANALISE = 1;

const TIPO_CADASTRO = [
	{
		value: 'cadastroCentralizado',
		label: 'Centralizado'
	},
	{
		value: 'cadastroDescentralizado',
		label: 'Descentralizado'
	}
];

const PESSOAJURIDICA = { id: '1', codigo: 'PESSOAJURIDICA' };

const MEI = { id: '2', codigo: 'MEI' };

const YES_OR_NO = [
	{
		value: true,
		label: 'Sim',
		align: 'center'
	},
	{
		value: false,
		label: 'Não',
		align: 'center'
	}
];

const TIPO_LOGRADOURO = [];
const ESTADO = [
	{
		value: '1',
		label: 'Minas Gerais',
		align: 'center'
	}
];

const SEXO = [
	{
		value: 'M',
		label: 'Masculino',
		align: 'center'
	},
	{
		value: 'F',
		label: 'Feminino',
		align: 'center'
	}
];
const ENUM_TIPO_ARQUIVO = [
	{
		value: 1,
		label: 'pdf'
	},
	{
		value: 2,
		label: 'xml'
	},
	{
		value: 4,
		label: 'csv'
	},
	{
		value: 8,
		label: 'jpg'
	}
];
const ENUM_TIPO_SOCIO = [
	{
		value: 0,
		label: translate('pessoaFisica'),
		internalName: 'Pessoa_Fisica'
	},
	{
		value: 1,
		label: translate('pessoaJuridica'),
		internalName: 'Pessoa_Juridica'
	}
];

const SELECT_TIPO_ASSINATURAS = [
	{ value: 0, label: translate('individualmente'), internalName: 'Individual' },
	{ value: 1, label: translate('emConjunto'), internalName: 'Conjunto' }
];

const ENUM_STATUS_ANALISE = [
	{
		value: 0,
		label: translate('pendenteEnvio'),
		internalName: 'Criado',
		color: 'rgb(0, 51, 204, 0.7)',
		acao: 'criarCadastro'
	},
	{
		value: 1,
		label: translate('pendenteAnalise'),
		internalName: 'Pendente_Analise',
		color: 'rgb(255, 102, 0, 1)',
		acao: 'enviarCadastro'
	},
	{
		value: 2,
		label: translate('emAnalise'),
		internalName: 'Em_Analise',
		color: 'rgb(0, 102, 102, 0.7)',
		acao: 'analisarCadastro'
	},
	{
		value: 3,
		label: translate('aprovado'),
		internalName: 'Aprovado',
		color: 'rgb(0, 128, 0, 0.7)',
		acao: 'aprovarCadastro'
	},
	{
		value: 4,
		label: translate('reprovado'),
		internalName: 'Reprovado',
		color: 'rgb(255, 0, 0, 0.7)',
		acao: 'reprovarCadastro'
	},
	{
		value: 5,
		label: translate('aprovadoRessalvas'),
		internalName: 'Aprovado_Ressalvas',
		color: 'rgb(33, 195, 80, 0.7)',
		acao: 'aprovarRessalvasCadastro'
	},
	{
		value: 6,
		label: translate('suspenso'),
		internalName: 'Suspenso',
		color: 'rgb(128, 0, 0, 0.7)',
		acao: 'suspenderCadastro'
	},
	{
		value: 7,
		label: translate('reaberto'),
		internalName: 'Reaberto',
		color: 'rgb(0, 102, 204, 0.7)',
		acao: 'reabrirCadastro'
	},
	{
		value: null,
		label: translate('pendenteEnvio'),
		internalName: 'Criado',
		color: 'rgb(0, 102, 204, 0.7)',
		acao: 'criarCadastro'
	}
];

const ENUM_ITEMS_ANALISE = [
	{
		value: 0,
		internalName: 'Dados_Gerais'
	},
	{
		value: 1,
		internalName: 'Dados_Endereco'
	},
	{
		value: 2,
		internalName: 'Acesso_Sistema'
	},
	{
		value: 3,
		internalName: 'Dados_Contatos_Adicionais'
	},
	{
		value: 4,
		internalName: 'Dados_Contrato_Social'
	},
	{
		value: 5,
		internalName: 'Cadastro_Socios'
	},
	{
		value: 6,
		internalName: 'Cadastro_Procuradores'
	},
	{
		value: 7,
		internalName: 'Cadastro_Signatario'
	},
	{
		value: 8,
		internalName: 'Grupos_Fornecimento'
	},
	{
		value: 9,
		internalName: 'Dados_Bancarios'
	},
	{
		value: 10,
		internalName: 'Dados_Balanco_Patrimonial'
	},
	{
		value: 11,
		internalName: 'Dados_DRE'
	},
	{
		value: 12,
		internalName: 'Qualificacao_Risco_Financeiro'
	},
	{
		value: 13,
		internalName: 'Arquivo'
	},
	{
		value: 14,
		internalName: 'Dados_Pessoa_Fisica'
	},
	{
		value: 15,
		internalName: 'Contato_Cliente'
	},
	{
		value: 16,
		internalName: 'Termos_Aceite'
	}
];

const COMANDO_CADASTRO_FORNECEDOR = {
	criarCadastro: 'criarCadastro',
	salvar: 'Salvar',
	enviarCadastro: 'enviarCadastro',
	aprovarCadastro: 'aprovarCadastro',
	aprovarRessalvasCadastro: 'aprovarRessalvasCadastro',
	reprovarCadastro: 'reprovarCadastro',
	susenderCadastro: 'susenderCadastro',
	reabrirCadastro: 'reabrirCadastro',
	analisarCadastro: 'analisarCadastro',
	limparFormulario: 'limparFormulario',
	descartarAlteracoes: 'descartarAlteracoes',
	validar: 'validar'
};

const SITUACAO_FORNECEDOR = [
	{
		value: 1,
		label: translate('ativo'),
		internalName: 'Ativo'
	},
	{
		value: 2,
		label: translate('inativo'),
		internalName: 'Inativo'
	},
	{
		value: 3,
		label: translate('suspenso'),
		internalName: 'Suspenso'
	}
];

const FEMININO = { id: '2', codigo: 'Feminino' };

const MASCULINO = { id: '1', codigo: 'Masculino' };

const CADASTRO_DESCENTRALIZADO = { codigo: 'cadastroDescentralizado' };
const CADASTRO_CENTRALIZADO = { codigo: 'cadastroCentralizado' };

const EDITAR = 'u';
const CRIAR = 'c';
const VISUALIZAR = 'r';
const DELETAR = 'd';

const MEC_VERSAO = 'MEC_VERSAO';
const MEC_PERGUNTAS = 'MEC_PERGUNTAS';
const MEC_GRAFICO = 'MEC_GRAFICO';
const MEC_RESPOSTA = 'MEC_RESPOSTA';
const MEC_RESULTADO = 'MEC_RESULTADO';
const MEC_ACOMPANHAMENTO = 'MEC_ACOMPANHAMENTO';
const MEC_IMPORTACAO = 'MEC_IMPORTACAO';
const MEC_GRUPO_USUARIO = 'MEC_GRUPO_USUARIO';

// SUBMODULO FORNECEDOR
const FORNECEDOR_CADASTRO = 'FORNECEDOR_CADASTRO';
const FORNECEDOR_ANALISE_CADASTRO = 'FORNECEDOR_ANALISE_CADASTRO';
const FORNECEDOR_TIPO_CONTATO = 'FORNECEDOR_TIPO_CONTATO';
const FORNECEDOR_TIPO_DOCUMENTO = 'FORNECEDOR_TIPO_DOCUMENTO';
const FORNECEDOR_TERMOS_ACEITE = 'FORNECEDOR_TERMOS_ACEITE';
const FORNECEDOR_TIPO_GRUPO = 'FORNECEDOR_TIPO_GRUPO';
const FORNECEDOR_TIPO_EXIGENCIA = 'FORNECEDOR_TIPO_EXIGENCIA';
const FORNECEDOR_EXIGENCIA = 'FORNECEDOR_EXIGENCIA';

const AUTENTICADOR = 'AUTENTICADOR';
const AUTENTICADOR_ACESSO = 'AUTENTICADOR_ACESSO';

const SUBDIRETORIO_LINK = window.globalConfig.SubDiretorio;

const COLUMNS_QUALIFICACAO_REFERENCIA = [
	{ id: 'Referencia', label: 'Referencia', width: '220%' },
	{ id: '2018', label: '2018', width: '100%' },
	{ id: '2017', label: '2017', width: '100%' },
	{ id: '2016', label: '2016', width: '100%' }
];

const HORIZONTAL_COLUMNS_QUALIFICACAO = [
	{ codigo: 'Referencia', label: 'LIQUIDEZ', negrito: true },
	{ codigo: 'LC', label: 'CORRENTE: (AC / PC) =	>  1,00', paddingLeft: theme.spacing(2) },
	{ codigo: 'LS', label: 'SECA: (AC - ESTOQUES) / PC =	>  1,00', paddingLeft: theme.spacing(2) },
	{ codigo: 'Referencia', label: 'ENDIVIDAMENTO', negrito: true },
	{
		codigo: 'EG',
		label: 'GERAL: (EXIGÍVEL TOTAL / ATIVO TOTAL) =	<  0,70',
		paddingLeft: theme.spacing(2)
	},
	{
		codigo: 'CE',
		label: 'COMPOSIÇÃO DO ENDIVIDAMENTO: (PC / PASSIVO EXIGÍVEL) =	<  0,50',
		paddingLeft: theme.spacing(2)
	},
	{
		codigo: 'ALDB',
		label: 'ALAVANCAGEM: (DÍVIDA BRUTA / PL) =	<  1,00',
		paddingLeft: theme.spacing(2)
	},
	{
		codigo: 'ALDL',
		label: 'ALAVANCAGEM: (DÍVIDA LÍQUIDA / PL ) =	<  0,50',
		paddingLeft: theme.spacing(2)
	},
	{
		codigo: 'AL',
		label: 'ALAVANCAGEM: (DÍVIDA BRUTA / EBIT) =	<  2,00',
		paddingLeft: theme.spacing(2)
	},
	{
		codigo: 'ICJ',
		label: 'ÍNDICE DE COBERTURA DE JUROS: (EBIT / DESPESAS FINANCEIRAS) =	>  1,00',
		paddingLeft: theme.spacing(2)
	},
	{ codigo: 'Referencia', label: 'RENTABILIDADE', negrito: true },
	{ codigo: 'ROE', label: 'ROE = LUCRO LÍQUIDO / PL =	>  0,05', paddingLeft: theme.spacing(2) },
	{
		codigo: 'ME',
		label: 'MARGEM EBIT = EBIT / RECEITA LÍQUIDA =	>  0,00',
		paddingLeft: theme.spacing(2)
	},
	{
		codigo: 'ML',
		label: 'MARGEM LÍQUIDA = LUCRO LÍQUIDO / RECEITA LÍQUIDA =	>  0,00',
		paddingLeft: theme.spacing(2)
	},
	{ codigo: 'Referencia', label: 'ATIVIDADE', negrito: true },
	{
		codigo: 'GA',
		label: 'GIRO DO ATIVO: (REC. LÍQUIDA / ATIVO TOTAL) =	>  0,10',
		paddingLeft: theme.spacing(2)
	}
];

const SEM_AVALIACAO = 'Sem avaliação';

const ESTAVEL = 'Estável';

const INSTAVEL = 'Instável';

const APROVAR = 'APROVAR';

const APROVAR_COM_RESSALVAS = 'APROVAR COM RESSALVAS';

const REPROVAR = 'REPROVAR';

export {
	URL_MATRIZ,
	CRIAR,
	PERFIS,
	EDITAR,
	VISUALIZAR,
	DELETAR,
	MEC_VERSAO,
	MEC_PERGUNTAS,
	MEC_GRAFICO,
	MEC_RESPOSTA,
	MEC_RESULTADO,
	MEC_ACOMPANHAMENTO,
	MEC_IMPORTACAO,
	MEC_GRUPO_USUARIO,
	MATERIAL,
	TIPO_DOCUMENTO,
	SERVICO,
	SELECT_TIPO,
	SELECT_NOTAS,
	SISTEMA,
	FORMULARIO,
	SELECT_TIPO_CONTATO,
	YES_OR_NO,
	ESTADO,
	SEXO,
	FUNCIONALIDADE,
	TIPO_EMPRESA,
	TIPO_RESPOSTA,
	TIPO_LOGRADOURO,
	CATEGORIAS,
	SUBCATEGORIAS,
	PRINCIPAL,
	FORNECEDOR_CADASTRO,
	FORNECEDOR_ANALISE_CADASTRO,
	FORNECEDOR_TIPO_CONTATO,
	FORNECEDOR_TIPO_DOCUMENTO,
	FORNECEDOR_TERMOS_ACEITE,
	FORNECEDOR_TIPO_GRUPO,
	FORNECEDOR_TIPO_EXIGENCIA,
	FORNECEDOR_EXIGENCIA,
	PESSOAJURIDICA,
	MEI,
	BALANCO_PATRIMONIAL,
	DRE,
	FEMININO,
	MASCULINO,
	SELECT_GRUPOS,
	GRUPO_CATEGORIA,
	CATEGORIA,
	SKU,
	SELECT_TIPO_FORNECEDOR,
	DISTRIBUIDOR,
	FABRICANTE,
	ENUM_TIPO_SOCIO,
	ENUM_TIPO_ARQUIVO,
	SELECT_TIPO_ASSINATURAS,
	ENUM_STATUS_ANALISE,
	ENUM_ITEMS_ANALISE,
	TIPO_CADASTRO,
	SUBDIRETORIO_LINK,
	ARQUIVO_PROCESSADO,
	COMANDO_CADASTRO_FORNECEDOR,
	ROWSPERPAGE,
	COLUMNS_QUALIFICACAO_REFERENCIA,
	HORIZONTAL_COLUMNS_QUALIFICACAO,
	SEM_AVALIACAO,
	INSTAVEL,
	ESTAVEL,
	APROVAR,
	APROVAR_COM_RESSALVAS,
	REPROVAR,
	SITUACAO_FORNECEDOR,
	CADASTRO_DESCENTRALIZADO,
	CADASTRO_CENTRALIZADO,
	SELECT_TIPO_CATEGORIA,
	AUTENTICADOR,
	AUTENTICADOR_ACESSO,
	TAB_DADOS_BASICOS,
	TAB_DADOS_COMPLEMENTAR,
	TAB_DADOS_FINANCEIROS,
	TAB_DADOS_SOCIOS,
	TAB_QUALIFICACAO,
	CADASTRO_PENDENTE_ANALISE,
	CADASTRO_CRIADO
};
