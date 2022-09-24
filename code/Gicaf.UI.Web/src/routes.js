import React, { Fragment, Suspense, lazy } from 'react';
import { Switch, Route, Redirect } from 'react-router-dom';
import { Loader } from 'react-axxiom';
import { useSelector } from 'react-redux';
import paths from '@/utils/paths';
import { translate } from '@/locales';
import { PrivateRoute } from '@/layout';
import {
	Login,
	RedefinicaoSenha,
	SolicitacaoRedefinicaoSenha,
	ConfirmacaoCadastroFornecedor,
	EsqueciMeuEmailVerifica,
	EsqueciMeuEmailEnvia
} from './screens/autenticacao';
import {
	CadastroComplementar,
	ListagemTipoContato,
	CadastrarTipoContato,
	ListagemTipoDocumento,
	CadastrarTipoDocumento,
	ListagemAnalise
} from './screens/fornecedor';
import {
	EDITAR,
	CRIAR,
	VISUALIZAR,
	MEC_VERSAO,
	MEC_RESULTADO,
	MEC_GRAFICO,
	MEC_GRUPO_USUARIO,
	MEC_PERGUNTAS,
	MEC_IMPORTACAO,
	MEC_ACOMPANHAMENTO,
	MEC_RESPOSTA,
	FORNECEDOR_CADASTRO,
	FORNECEDOR_ANALISE_CADASTRO,
	FORNECEDOR_TIPO_CONTATO,
	FORNECEDOR_TIPO_DOCUMENTO,
	FORNECEDOR_TERMOS_ACEITE,
	FORNECEDOR_TIPO_GRUPO,
	FORNECEDOR_TIPO_EXIGENCIA,
	FORNECEDOR_EXIGENCIA
} from './utils/constants';

const Home = lazy(() => import('./screens/home'));
const SemPermissao = lazy(() => import('./screens/error'));


export default function Routes() {
	const { open, message } = useSelector(stateRedux => stateRedux.loader);

	return (
		<Fragment>
			<Loader open={open} message={message} />
			<Suspense fallback={<Loader open message={`${translate('carregando')}`} />}>
				<Switch>
				<Redirect exact from='/' to={paths.getPathByCodigo('login')} />
					<Redirect exact from='/semPermissao' to={paths.getPathByCodigo('semPermissao')} />
					<Route path={paths.getPathByCodigo('login')} component={Login} />
					<Route path={paths.getPathByCodigo('redefinicao-senha')} component={RedefinicaoSenha} />
					<Route
						path={paths.getPathByCodigo('confirmacao-cadastro-fornecedor')}
						component={ConfirmacaoCadastroFornecedor}
					/>
					<Route
						path={paths.getPathByCodigo('esqueci-meu-email-verifica')}
						component={EsqueciMeuEmailVerifica}
					/>
					<Route
						path={paths.getPathByCodigo('esqueci-meu-email-envia')}
						component={EsqueciMeuEmailEnvia}
					/>
					<Route
						path={paths.getPathByCodigo('solicitacao-redefinicao-senha')}
						component={SolicitacaoRedefinicaoSenha}
					/>
					<PrivateRoute path={paths.getPathByCodigo('home')} component={Home} />
					<PrivateRoute path={paths.getPathByCodigo('semPermissao')} component={SemPermissao} />

					<Route
						path={paths.getPathByCodigo('esqueci-meu-email-verifica')}
						component={EsqueciMeuEmailVerifica}
					/>
					<PrivateRoute
						funcionalidade={FORNECEDOR_TIPO_CONTATO}
						path={paths.getPathByCodigo('editar-tipo-contato')}
						component={CadastrarTipoContato}
					/>
					<Route
						path={paths.getPathByCodigo('cadastro-complementar')}
						component={CadastroComplementar}
					/>
					<PrivateRoute
						funcionalidade={FORNECEDOR_TIPO_CONTATO}
						path={paths.getPathByCodigo('tipo-contato')}
						component={ListagemTipoContato}
					/>
					<PrivateRoute
						funcionalidade={FORNECEDOR_TIPO_DOCUMENTO}
						path={paths.getPathByCodigo('novo-tipo-documento')}
						component={CadastrarTipoDocumento}
					/>
					<PrivateRoute
						funcionalidade={FORNECEDOR_TIPO_DOCUMENTO}
						path={paths.getPathByCodigo('editar-tipo-documento')}
						component={CadastrarTipoDocumento}
					/>
					<PrivateRoute
						funcionalidade={FORNECEDOR_TIPO_DOCUMENTO}
						path={paths.getPathByCodigo('tipo-documento')}
						component={ListagemTipoDocumento}
					/>
					<PrivateRoute
						path={paths.getPathByCodigo('analise-cadastro')}
						component={ListagemAnalise}
					/>
					{/* <Redirect to={paths.getPathByCodigo('login')} /> */}
				</Switch>
			</Suspense>
		</Fragment>
	);
}
