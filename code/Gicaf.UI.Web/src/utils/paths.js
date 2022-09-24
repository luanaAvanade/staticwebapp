import _ from 'lodash';
import { SUBDIRETORIO_LINK } from '@/utils/constants';

const routes = [
	{
		codigo: 'home',
		currentPath: 'home',
		path: `${SUBDIRETORIO_LINK}/home`
	},
	{
		codigo: 'semPermissao',
		currentPath: 'semPermissao',
		path: `${SUBDIRETORIO_LINK}/semPermissao`
	},
	{
		codigo: 'login',
		currentPath: 'login',
		path: `${SUBDIRETORIO_LINK}/login`
	},
	{
		codigo: 'confirmacao-cadastro-fornecedor',
		currentPath: 'confirmacao-cadastro-fornecedor',
		path: `${SUBDIRETORIO_LINK}/confirmacao-cadastro-fornecedor`
	},
	{
		codigo: 'redefinicao-senha',
		currentPath: 'redefinicao-senha',
		path: `${SUBDIRETORIO_LINK}/redefinicao-senha`
	},
	{
		codigo: 'solicitacao-redefinicao-senha',
		currentPath: 'solicitacao-redefinicao-senha',
		path: `${SUBDIRETORIO_LINK}/solicitacao-redefinicao-senha`
	},
	{
		codigo: 'esqueci-meu-email-verifica',
		currentPath: 'esqueci-meu-email',
		path: `${SUBDIRETORIO_LINK}/esqueci-meu-email-verifica`
	},
	{
		codigo: 'esqueci-meu-email-envia',
		currentPath: 'esqueci-meu-email-envia',
		path: `${SUBDIRETORIO_LINK}/esqueci-meu-email-envia`
	},
	// Usuarios
	{
		codigo: 'novo-usuario',
		name: 'Novo Usuário',
		title: 'Novo Usuário',
		currentPath: 'usuario',
		path: `${SUBDIRETORIO_LINK}/usuario`
	},
	// Versões
	{
		codigo: 'versoes',
		name: 'Versões',
		title: 'Listagem de Versões',
		currentPath: 'versoes',
		path: `${SUBDIRETORIO_LINK}/versoes`
	},
	{
		codigo: 'nova-versao',
		name: 'Nova Versão',
		title: 'Nova Versão',
		currentPath: 'nova-versao',
		path: `${SUBDIRETORIO_LINK}/versoes/nova-versao`
	},
	{
		codigo: 'editar-versao',
		name: 'Editar Versão',
		title: 'Editar Versão',
		currentPath: 'editar-versao',
		path: `${SUBDIRETORIO_LINK}/versoes/editar-versao/:id`
	},
	// Pergunta
	{
		codigo: 'pergunta',
		name: 'Perguntas',
		title: 'Listagem de Perguntas',
		currentPath: 'pergunta',
		path: `${SUBDIRETORIO_LINK}/pergunta`
	},
	{
		codigo: 'nova-pergunta',
		name: 'Nova Pergunta',
		title: 'Nova Pergunta',
		currentPath: 'nova-pergunta',
		path: `${SUBDIRETORIO_LINK}/pergunta/nova-pergunta`
	},
	{
		codigo: 'editar-pergunta',
		name: 'Editar Pergunta',
		title: 'Editar Pergunta',
		currentPath: 'editar-pergunta',
		path: `${SUBDIRETORIO_LINK}/pergunta/editar-pergunta/:id`
	},
	// Resultado
	{
		codigo: 'resultado',
		name: 'Resultados',
		title: 'Resultados',
		currentPath: 'resultado',
		path: `${SUBDIRETORIO_LINK}/resultado`
	},
	// Importação
	{
		codigo: 'importacao',
		name: 'Importação de Arquivos',
		title: 'Importação de Arquivos de Resposta',
		currentPath: 'importacao',
		path: `${SUBDIRETORIO_LINK}/importacao`
	},
	// Matriz
	{
		codigo: 'matriz',
		name: 'Matriz',
		title: 'Matriz Estratégica de Categorias',
		currentPath: 'matriz',
		path: `${SUBDIRETORIO_LINK}/matriz`
	},
	// Acompanhamento
	{
		codigo: 'acompanhamento',
		name: 'Acompanhamento',
		title: 'Acompanhamento das Respostas',
		currentPath: 'acompanhamento',
		path: `${SUBDIRETORIO_LINK}/acompanhamento`
	},
	// Resposta
	{
		codigo: 'resposta',
		name: 'Formulário de Resposta',
		title: 'Formulário de Resposta',
		currentPath: 'resposta',
		path: `${SUBDIRETORIO_LINK}/resposta`
	},
	// Grupo de Usuário
	{
		codigo: 'grupo-respondente',
		name: 'Grupos de Respondentes',
		title: 'Listagem de Grupo de Respondentes',
		currentPath: 'grupo-respondente',
		path: `${SUBDIRETORIO_LINK}/grupo-respondente`
	},
	{
		codigo: 'novo-grupo-respondente',
		name: 'Novo Grupo de Respondentes',
		title: 'Novo Grupo de Respondentes',
		currentPath: 'novo-grupo-respondente',
		path: `${SUBDIRETORIO_LINK}/grupo-respondente/novo-grupo-respondente`
	},
	{
		codigo: 'editar-grupo-respondente',
		name: 'Editar Grupo de Respondentes',
		title: 'Editar Grupo de Respondentes',
		currentPath: 'editar-grupo-respondente',
		path: `${SUBDIRETORIO_LINK}/grupo-respondente/editar-grupo-respondente/:id`
	},
	{
		codigo: 'fornecedor',
		name: 'Formulário Online de Cadastro de Fornecedores',
		title: 'Formulário Online de Cadastro de Fornecedores',
		currentPath: 'fornecedor',
		path: `${SUBDIRETORIO_LINK}/fornecedor`
	},
	{
		codigo: 'cadastroComplementar',
		name: 'Cadastro do Fornecedor',
		title: 'Cadastro do Fornecedor',
		currentPath: 'fornecedor-complementar',
		path: `${SUBDIRETORIO_LINK}/fornecedor-complementar`
	},
	{
		codigo: 'novo-fornecedor',
		name: 'Novo Fornecedor',
		title: 'Novo Fornecedor',
		currentPath: 'novo',
		path: `${SUBDIRETORIO_LINK}/fornecedor/novo`
	},
	// Tipo de Documento
	{
		codigo: 'tipo-documento',
		name: 'Listagem de Tipos de Documento',
		title: 'Listagem de Tipos de Documento',
		currentPath: 'tipo-documento',
		path: `${SUBDIRETORIO_LINK}/tipo-documento`
	},
	{
		codigo: 'novo-tipo-documento',
		name: 'Novo Tipo de Documento',
		title: 'Novo Tipo de Documento',
		currentPath: 'novo-tipo-documento',
		path: `${SUBDIRETORIO_LINK}/tipo-documento/novo-tipo-documento`
	},
	{
		codigo: 'editar-tipo-documento',
		name: 'Editar Tipo de Documento',
		title: 'Editar Tipo de Documento',
		currentPath: 'editar-tipo-documento',
		path: `${SUBDIRETORIO_LINK}/tipo-documento/editar-tipo-documento/:id`
	},
	// Fornecedor
	//Termos de Aceite
	{
		codigo: 'termos-aceite',
		name: 'Termos de Aceite',
		title: 'Termos de Aceite',
		currentPath: 'termos-aceite',
		path: `${SUBDIRETORIO_LINK}/termos-aceite`
	},
	{
		codigo: 'termos-aceite-cadastro',
		name: 'Termos de Aceite Cadastro',
		title: 'Termos de Aceite Cadastro',
		currentPath: 'termos-aceite-cadastro',
		path: `${SUBDIRETORIO_LINK}/termos-aceite/termos-aceite-cadastro`
	},
	{
		codigo: 'editar-termos-aceite',
		name: 'Editar Termos de Aceite',
		title: 'Editar Termos de Aceite',
		currentPath: 'editar-termos-aceite',
		path: `${SUBDIRETORIO_LINK}/termos-aceite/editar-termos-aceite/:id`
	},
	//Tipo de Grupo

	{
		codigo: 'grupo-pergunta-qualificacao',
		name: 'Grupo de Perguntas de Qualificação',
		title: 'Grupo de Perguntas de Qualificação',
		currentPath: 'grupo-pergunta-qualificacao',
		path: `${SUBDIRETORIO_LINK}/grupo-pergunta-qualificacao`
	},

	{
		codigo: 'novo-grupo-pergunta-qualificacao',
		name: 'Novo Grupo de Perguntas de Qualificação',
		title: 'Novo Grupo de Perguntas de Qualificação',
		currentPath: 'novo-grupo-pergunta-qualificacao',
		path: `${SUBDIRETORIO_LINK}/grupo-pergunta-qualificacao/novo-grupo-pergunta-qualificacao`
	},
	{
		codigo: 'editar-grupo-pergunta-qualificacao',
		name: 'Editar Grupo de Perguntas de Qualificação',
		title: 'Editar Grupo de Perguntas de Qualificação',
		currentPath: 'editar-grupo-pergunta-qualificacao',
		path: `${SUBDIRETORIO_LINK}/grupo-pergunta-qualificacao/editar-grupo-pergunta-qualificacao/:id`
	},

	{
		codigo: 'pergunta-qualificacao',
		name: 'Perguntas de Qualificação',
		title: 'Perguntas de Qualificação',
		currentPath: 'pergunta-qualificacao',
		path: `${SUBDIRETORIO_LINK}/pergunta-qualificacao`
	},
	{
		codigo: 'nova-pergunta-qualificacao',
		name: 'Nova Pergunta de Qualificação',
		title: 'Nova Pergunta de Qualificação',
		currentPath: 'nova-pergunta-qualificacao',
		path: `${SUBDIRETORIO_LINK}/pergunta-qualificacao/nova-pergunta-qualificacao`
	},
	{
		codigo: 'editar-pergunta-qualificacao',
		name: 'Editar Pergunta de Qualificação',
		title: 'Editar Pergunta de Qualificação',
		currentPath: 'editar-pergunta-qualificacao',
		path: `${SUBDIRETORIO_LINK}/pergunta-qualificacao/editar-pergunta-qualificacao/:id`
	},

	// Tipo de Contato
	{
		codigo: 'tipo-contato',
		name: 'Listagem de Tipos de Contato',
		title: 'Listagem de Tipos de Contato',
		currentPath: 'tipo-contato',
		path: `${SUBDIRETORIO_LINK}/tipo-contato`
	},
	{
		codigo: 'novo-tipo-contato',
		name: 'Novo Tipo de Contato',
		title: 'Novo Tipo de Contato',
		currentPath: 'novo-tipo-contato',
		path: `${SUBDIRETORIO_LINK}/tipo-contato/novo-tipo-contato`
	},
	{
		codigo: 'editar-tipo-contato',
		name: 'Editar Tipo de Contato',
		title: 'Editar Tipo de Contato',
		currentPath: 'editar-tipo-contato',
		path: `${SUBDIRETORIO_LINK}/tipo-contato/editar-tipo-contato/:id`
	},
	// Tipo de Exigencia

	// Tipo de Exigencia

	{
		codigo: 'exigencia-grupo-cadastro',
		name: 'Cadastro de Associação de Grupos às Exigências',
		title: 'Cadastro de Associação de Grupos às Exigências',
		currentPath: 'exigencia-grupo-cadastro',
		path: `${SUBDIRETORIO_LINK}/exigencia-grupo-cadastro`
	},

	{
		codigo: 'tipo-exigencia',
		name: 'Listagem de Tipos de Exigencia',
		title: 'Listagem de Tipos de Exigencia',
		currentPath: 'tipo-exigencia',
		path: `${SUBDIRETORIO_LINK}/tipo-exigencia`
	},
	{
		codigo: 'novo-tipo-exigencia',
		name: 'Novo Tipo de Exigencia',
		title: 'Novo Tipo de Exigencia',
		currentPath: 'novo-tipo-exigencia',
		path: `${SUBDIRETORIO_LINK}/tipo-exigencia/novo-tipo-exigencia`
	},
	{
		codigo: 'editar-tipo-exigencia',
		name: 'Editar Tipo de Exigencia',
		title: 'Editar Tipo de Exigencia',
		currentPath: 'editar-tipo-exigencia',
		path: `${SUBDIRETORIO_LINK}/tipo-exigencia/editar-tipo-exigencia/:id`
	},

	// Auto Cadastro
	{
		codigo: 'pre-cadastro',
		name: 'Formulário Online de Cadastro de Fornecedores',
		title: 'Formulário Online de Cadastro de Fornecedores',
		currentPath: 'pre-cadastro',
		path: `${SUBDIRETORIO_LINK}/pre-cadastro`
	},
	{
		codigo: 'cadastro-complementar',
		name: 'Cadastro do Fornecedor',
		title: 'Cadastro do Fornecedor',
		currentPath: 'cadastro-complementar',
		path: `${SUBDIRETORIO_LINK}/cadastro-complementar`
	},
	{
		codigo: 'analise-cadastro',
		name: 'Listagem de Análise de Cadastro',
		title: 'Listagem de Análise de Cadastro',
		currentPath: 'analise-cadastro',
		path: `${SUBDIRETORIO_LINK}/analise-cadastro`
	}
];

export default class Paths {
	static getPartsOfPath(history) {
		return history.location.pathname.split('/');
	}

	static getCurrentPath(history) {
		const allParts = this.getPartsOfPath(history);
		const otherParts = allParts.slice(1, allParts.length - 1);
		let lastPart = _.last(allParts);

		// eslint-disable-next-line no-restricted-globals
		if (!isNaN(lastPart)) {
			lastPart = _.last(otherParts);
		}

		return lastPart;
	}

	static getRoute(currentPath) {
		return _.find(routes, { currentPath });
	}

	static getRouteByCodigo(codigo) {
		return _.find(routes, { codigo });
	}

	static getPathByCodigo(codigo) {
		const route = this.getRouteByCodigo(codigo);
		return route ? route.path : '';
	}

	static getPath(currentPath) {
		const route = this.getRoute(currentPath);
		return route ? route.path : '';
	}

	static getName(currentPath) {
		const route = this.getRoute(currentPath);
		return route ? route.name : currentPath;
	}

	static getTitle(currentPath) {
		const route = this.getRoute(currentPath);
		return route ? route.title : '';
	}
}
