import { lazy } from 'react';
const ListagemTipoContato = lazy(() => import('./tipoContato/listar'));
const CadastrarTipoContato = lazy(() => import('./tipoContato/cadastrar'));
const ListagemTipoDocumento = lazy(() => import('./tipoDocumento/listar'));
const CadastrarTipoDocumento = lazy(() => import('./tipoDocumento/cadastrar'));
const CadastroComplementar = lazy(() => import('./autoCadastro/cadastroComplementar'));
const ListagemAnalise = lazy(() => import('./analiseCadastro/listar'));

export {
	ListagemTipoContato,
    ListagemAnalise,
	CadastrarTipoContato,
	CadastroComplementar,
	ListagemTipoDocumento,
	CadastrarTipoDocumento
};
