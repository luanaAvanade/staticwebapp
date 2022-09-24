import { lazy } from 'react';
const Login = lazy(() => import('./login'));
const RedefinicaoSenha = lazy(() => import('./redefinicaoSenha'));
const ConfirmacaoCadastroFornecedor = lazy(() => import('./confirmacaoCadastroFornecedor'));
const SolicitacaoRedefinicaoSenha = lazy(() => import('./solicitacaoRedefinicaoSenha'));
const EsqueciMeuEmailVerifica = lazy(() => import('./esqueciMeuEmail/verifica'));
const EsqueciMeuEmailEnvia = lazy(() => import('./esqueciMeuEmail/envia'));

export {
	Login,
	RedefinicaoSenha,
	ConfirmacaoCadastroFornecedor,
	SolicitacaoRedefinicaoSenha,
	EsqueciMeuEmailVerifica,
	EsqueciMeuEmailEnvia
};
