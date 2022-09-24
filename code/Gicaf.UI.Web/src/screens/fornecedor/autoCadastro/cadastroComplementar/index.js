import React, { useState, useEffect, Fragment } from 'react';
import { useDispatch } from 'react-redux';
import { Tabs, Tab, Box, Typography } from '@material-ui/core';
import useReactRouter from 'use-react-router';
import { useSnackbar } from 'notistack';
import * as Yup from 'yup';
import { snackError, snackWarning } from '@/utils/snack';
import { getUser } from '@/utils/auth';
import theme from '@/theme';
import _ from 'lodash';
import paths from '@/utils/paths';
import { translate } from '@/locales';
import { Title, LayoutContent } from '@/layout';
import { Creators as LoaderCreators } from '@/store/ducks/loader';
import DadosBasicos from '@/screens/fornecedor/autoCadastro/dadosBasicos';
import DadosComplementar from '@/screens/fornecedor/autoCadastro/dadosComplementares';
import DadosFinanceiros from '@/screens/fornecedor/autoCadastro/dadosFinanceiros';
import DadosSocios from '@/screens/fornecedor/autoCadastro/dadosSocios';
import EmpresaService from '@/services/empresa';
import ObjectHelper from '@/utils/objectHelper';
import {
	ENUM_ITEMS_ANALISE,
	ENUM_STATUS_ANALISE,
	COMANDO_CADASTRO_FORNECEDOR,
	CADASTRO_DESCENTRALIZADO,
	TAB_DADOS_BASICOS,
	TAB_DADOS_SOCIOS,
	TAB_DADOS_COMPLEMENTAR,
	TAB_DADOS_FINANCEIROS,
	TAB_QUALIFICACAO,
	CADASTRO_PENDENTE_ANALISE,
	CADASTRO_CRIADO
} from '@/utils/constants';
import { DisplayDiv } from './style';
import { Done, Clear } from '@material-ui/icons';

import { Confirm, Button, Card, Modal } from 'react-axxiom';
import { BotoesCadastro } from '@/screens/fornecedor/autoCadastro/componentesLayout/botoesCadastro';
import { StatusEmpresa } from '@/screens/fornecedor/autoCadastro/componentesLayout/statusEmpresa';
import { getDisableEdit } from '../aprovacao/util';
import { Valid, Invalid } from './style';

export default function CadastroComplemetar() {
	const { history, match } = useReactRouter();
	const { enqueueSnackbar, closeSnackbar } = useSnackbar();
	const dispatch = useDispatch();
	const idParam = match.isExact
		? null
		: parseInt(window.location.pathname.split(`${match.url}/`)[1], 10);

	//Mocar historico

	const historicoEmpresa = [
		{
			Id: 1,
			Autor: {
				Id: 1,
				Name: 'Mariana Athayde Garcia',
				Perfil: 0
			},
			DataCriacao: '2019-10-20',
			Categoria: 0,
			Valor: 0,
			Item: null,
			Comentario: null,
			EmpresaId: 135,
			ArquivoId: null,
			UsuarioId: null,
			Descricao: `Cadastro Criado`
		},
		{
			Id: 1,
			Autor: {
				Id: 1,
				Name: 'Mariana Athayde Garcia',
				Perfil: 0
			},
			DataCriacao: '2019-10-20',
			Categoria: 0,
			Valor: 2,
			Item: null,
			Comentario: null,
			EmpresaId: 135,
			ArquivoId: null,
			UsuarioId: null,
			Descricao: `Pendente de Análise`
		},
		{
			Id: 1,
			Autor: {
				Id: 1,
				Name: 'Rafael Lima Souza',
				Perfil: 0
			},
			DataCriacao: '2019-10-25',
			Categoria: 1,
			Valor: 9,
			Item: null,
			Comentario: null,
			EmpresaId: 135,
			ArquivoId: null,
			UsuarioId: null,
			Descricao: `Análise Assumida`
		},
		{
			Id: 1,
			Autor: {
				Id: 1,
				Name: 'Rafael Lima Souza',
				Perfil: 0
			},
			DataCriacao: '2019-10-25',
			Categoria: 3,
			Valor: 4,
			Item: 5,
			Comentario: null,
			EmpresaId: 135,
			ArquivoId: null,
			UsuarioId: null,
			Descricao: `Dados Sócios Aprovado`
		},
		{
			Id: 1,
			Autor: {
				Id: 1,
				Name: 'Rafael Lima Souza',
				Perfil: 0
			},
			DataCriacao: '2019-10-25',
			Categoria: 3,
			Valor: 4,
			Item: 5,
			Comentario: null,
			EmpresaId: 135,
			ArquivoId: null,
			UsuarioId: null,
			Descricao: `Dados Sócios Aprovado`
		},
		{
			Id: 1,
			Autor: {
				Id: 1,
				Name: 'Rafael Lima Souza',
				Perfil: 0
			},
			DataCriacao: '2019-10-25',
			Categoria: 3,
			Valor: 4,
			Item: 5,
			Comentario: null,
			EmpresaId: 135,
			ArquivoId: null,
			UsuarioId: null,
			Descricao: `Dados Sócios Aprovado`
		},
		{
			Id: 1,
			Autor: {
				Id: 1,
				Name: 'Rafael Lima Souza',
				Perfil: 0
			},
			DataCriacao: '2019-10-25',
			Categoria: 3,
			Valor: 4,
			Item: 5,
			Comentario: null,
			EmpresaId: 135,
			ArquivoId: null,
			UsuarioId: null,
			Descricao: `Dados Sócios Aprovado`
		}
	];

	// Estados locais

	const [
		tab,
		setTab
	] = useState(0);

	const [
		empresa,
		setEmpresa
	] = useState(null);

	const [
		key,
		setKey
	] = useState(0);

	const [
		itensAnalise,
		setItensAnalise
	] = useState([]);

	const [
		comentarios,
		setComentarios
	] = useState([]);

	const [
		socioIsValid,
		setSocioIsValid
	] = useState(false);

	const [
		qualificacaoRiscoFinanceiroIsValid,
		setQualificacaoRiscoFinanceiroIsValid
	] = useState(false);

	const [
		dadosBasicosIsValid,
		setDadosBasicosIsValid
	] = useState(false);

	const [
		dadosFinanceirosIsValid,
		setDadosFinanceirosIsValid
	] = useState(false);

	const [
		balancoPatrimonialIsValid,
		setBalancoPatrimonialIsValid
	] = useState(false);

	const [
		balancoDREIsValid,
		setBalancoDREIsValid
	] = useState(false);

	const [
		dadosComplementaresIsValid,
		setDadosComplementaresIsValid
	] = useState(false);

	const [
		listTermosAceiteEmpresa,
		setListTermosAceiteEmpresa
	] = useState([]);

	const [
		erroTermoAceite,
		setErroTermoAceite
	] = useState('');

	const [
		statusCadastro,
		setStatusCadastro
	] = useState(ENUM_STATUS_ANALISE.find(x => x.internalName === 'Criado').value);

	const [
		user,
		setUser
	] = useState(null);

	const [
		acao,
		setAcao
	] = useState(null);

	const [
		openSalvarDados,
		setOpenSalvarDados
	] = useState(false);

	const [
		tabChanged,
		setTabChanged
	] = useState(0);

	const [
		changed,
		setChanged
	] = useState(false);

	const [
		modalLembreteAtualizacaoCadastro,
		setModalLembreteAtualizacaoCadastro
	] = useState(false);

	const [
		countLembrete,
		setCountLembrete
	] = useState(0);

	// Efeito Inicial
	useEffect(() => {
		setEmpresa(null);
		setItensAnalise([]);
			setComentarios([]);
			setListTermosAceiteEmpresa([]);
			setDadosBasicosIsValid(false);
			setDadosFinanceirosIsValid(false);
			setDadosComplementaresIsValid(false);
			setSocioIsValid(false);
			setStatusCadastro(null);

		//empresaFindById();
		return () => {
					};
	}, []);

	// Buscar Dados

	const empresaFindById = async () => {
		dispatch(LoaderCreators.setLoading());
		//const u = getUser();
		// if (u === null) {
		// 	return history.push(paths.getPathByCodigo('semPermissao'));
		// }

//		goHome();

		
		var UsuarioEmp = {
			Nome: 'Luana Rodrigues Santos'
		}

		var UsuarioEmp2 = {
			Nome: 'Luana Rodrigues Santos'
		}

		var AnaliseCadastro = {
			StatusAnalise: 0,
			AtribuidoId: 1,
			TransmitidoId: '',
			Atribuido: UsuarioEmp,
			Transmitido: UsuarioEmp2,
			

		};

		var calcRiscoLista = [
		{
			ClassificacaoFase1: 1
		}

		];



		var empresa = 
			{
				Id: 1,
				Situacao: 2,
				NomeEmpresa: 'Empresa telecom',
				CalculoRiscoLista: calcRiscoLista,
				CNPJ: '123456789',
				TipoEmpresa: '1',
				TipoCadastro: 'cadastro empresa',
				DataCriacao: '',
				AnaliseCadastro: AnaliseCadastro,
	IsentoIE: true,
	InscricaoMunicipal: 'sjdffjldfj',
	OptanteSimplesNacional: true,
	DataAbertura: '',
	DadosPessoaFisicaId: null,
	DadosPessoaFisica: null,
	ContatosAdicionais: null,
	Usuarios: null,
	ContatoSolicitante: null,
	ContatoSolicitanteId: null,
	Enderecos: null,
	GruposFornecimento: null,
	Documentos: null,
	DadosBalancosPatrimoniais: null,
	DadosDREs: null,
	DadosBancariosId: null,
	DadosBancarios: null,
	Socios: null,
	GruposDeAssinatura: null,
	CapitalSocialTotalSociedade: null,
	DataRegistroSociedade: '',
	AtividadeEconomicaPrincipalId: null,
	AtividadeEconomicaPrincipal: null,
	OcupacaoPrincipalId: null,
	OcupacaoPrincipal: null,
	TermoAceiteEmpresa: null,
	CodigoSap: '',
	Situacao: null,
	Comentarios: null,
	LinkCadastro: '', 
	Historico: historicoEmpresa
				
			}
	
			setEmpresa(empresa);



			setKey(key + 1);




			            //empresa = montarAnalise(empresa);
						// const t = response.data.Empresa_list[0].AnaliseCadastro.ItensAnalise;
						// t.forEach(x => (x.ArquivoId = x.ArquivoId === '' ? null : x.ArquivoId));
						//setItensAnalise(response.data.Empresa_list[0].AnaliseCadastro.ItensAnalise);
						setStatusCadastro(
							empresa.AnaliseCadastro.StatusAnalise === ''
								? null
								: empresa.AnaliseCadastro.StatusAnalise
						);
						setListTermosAceiteEmpresa(empresa.TermoAceiteEmpresa);
						
	
				
						dispatch(LoaderCreators.disableLoading());




		// const usuario = {
		// 	empresaId: u.empresaId,
		// 	perfilAnalista: u.permissions.find(x => x.name === 'FORNECEDOR_ANALISE_CADASTRO').u,
		// 	id: u.id,
		// 	nome: u.nome
		// };

		// if (usuario.perfilAnalista && idParam) {
		// 	usuario.empresaId = idParam;
		// }
		// setUser(usuario);

		// if (usuario.perfilAnalista && !usuario.empresaId) {
		// 	goAnalise();
		// } else if (!usuario.perfilAnalista && !usuario.empresaId) {
		// 	goHome();
		// } else {
		// 	try {
		// 		const response = await EmpresaService.findById(usuario.empresaId);
		// 		if (response.data && response.data.Empresa_list.length > 0) {
		// 			response.data.Empresa_list[0].Historico = historicoEmpresa;
		// 			setEmpresa(response.data.Empresa_list[0]);

		// 			response.data.Empresa_list[0] = montarAnalise(response.data.Empresa_list[0]);
		// 			// const t = response.data.Empresa_list[0].AnaliseCadastro.ItensAnalise;
		// 			// t.forEach(x => (x.ArquivoId = x.ArquivoId === '' ? null : x.ArquivoId));
		// 			setItensAnalise(response.data.Empresa_list[0].AnaliseCadastro.ItensAnalise);
		// 			setStatusCadastro(
		// 				response.data.Empresa_list[0].AnaliseCadastro.StatusAnalise === ''
		// 					? null
		// 					: response.data.Empresa_list[0].AnaliseCadastro.StatusAnalise
		// 			);
		// 			setListTermosAceiteEmpresa(response.data.Empresa_list[0].TermoAceiteEmpresa);
		// 			setKey(key + 1);

		// 			if (
		// 				!usuario.perfilAnalista &&
		// 				countLembrete === 0 &&
		// 				response.data.Empresa_list[0].AnaliseCadastro.StatusAnalise === CADASTRO_CRIADO
		// 			) {
		// 				setModalLembreteAtualizacaoCadastro(true);
		// 				setCountLembrete(1);
		// 			}

		// 			dispatch(LoaderCreators.disableLoading());
		// 		} else {
		// 			dispatch(LoaderCreators.disableLoading());
		// 			callbackError(translate('erroCarregamentoDadosEmpresa'));
		// 		}
		// 	} catch (error) {
		// 		dispatch(LoaderCreators.disableLoading());
		// 	}
		// }
	};

	const goHome = () => {
		dispatch(LoaderCreators.disableLoading());
		history.push(paths.getPathByCodigo('home'));
	};

	const goAnalise = () => {
		dispatch(LoaderCreators.disableLoading());
		history.push(paths.getPathByCodigo('analise-cadastro'));
	};

	function getItensAnalise() {
		const itensAnaliseTemp = ObjectHelper.clone(itensAnalise);
		itensAnaliseTemp.forEach(x => {
			x.TipoItem = ENUM_ITEMS_ANALISE.find(t => t.value === x.TipoItem).internalName;
			x.Status = ENUM_STATUS_ANALISE.find(t => t.value === x.Status).internalName;
			x.AutorId = x.AutorId === '' ? null : x.AutorId;
		});
		return itensAnaliseTemp;
	}

	function getAnaliseCadastro() {
		const obj = {
			ItensAnalise: getItensAnalise()
		};

		if (empresa.AnaliseCadastro.hasOwnProperty('Id') && empresa.AnaliseCadastro.Id !== null) {
			obj.Id = empresa.AnaliseCadastro.Id;
		}
		if (
			empresa.AnaliseCadastro.hasOwnProperty('AtribuidoId') &&
			empresa.AnaliseCadastro.AtribuidoId !== null
		) {
			obj.AtribuidoId =
				empresa.AnaliseCadastro.AtribuidoId === '' ? null : empresa.AnaliseCadastro.AtribuidoId;
		}
		if (acao) {
			if (acao !== COMANDO_CADASTRO_FORNECEDOR.salvar) {
				obj.StatusAnalise = ENUM_STATUS_ANALISE.find(x => x.acao === acao).internalName;
			} else {
				obj.StatusAnalise = ENUM_STATUS_ANALISE.find(
					x => x.value === empresa.AnaliseCadastro.StatusAnalise
				).internalName;
			}
		}
		return obj;
	}

	function getComentarios() {
		let comentariosTemp = ObjectHelper.clone(comentarios);
		comentariosTemp.forEach(x => {
			x.Local = ENUM_ITEMS_ANALISE.find(y => y.value === x.Local).internalName;
			delete x.DataCriacao;
			//x.Usuario ? delete x.Usuario : null;
		});
		return comentariosTemp;
	}

	const getTermoNaoAceitos = () => {
		return _.filter(listTermosAceiteEmpresa, termoAceite => termoAceite.Aceite === false);
	};

	function enviarCadastro() {
		if (getTermoNaoAceitos().length > 0) {
			enqueueSnackbar(
				'',
				snackWarning(translate('possuemCamposPreenchidosIncorretamente'), closeSnackbar)
			);
			setErroTermoAceite(translate('termoAceiteObrigatorio'));
		} else {
			if (
				!(
					dadosBasicosIsValid &&
					dadosComplementaresIsValid &&
					((dadosFinanceirosIsValid && socioIsValid) ||
						empresa.TipoCadastro === CADASTRO_DESCENTRALIZADO.codigo)
				)
			) {
				setAcao(COMANDO_CADASTRO_FORNECEDOR.validar);
				callbackWarning(translate('possuemCamposPreenchidosIncorretamente'));
			} else {
				setAcao(COMANDO_CADASTRO_FORNECEDOR.enviarCadastro);
			}
		}
	}

	function finalizarAnalise() {
		if (
			!(
				validarAnalise() &&
				dadosBasicosIsValid &&
				dadosComplementaresIsValid &&
				((dadosFinanceirosIsValid && socioIsValid) ||
					empresa.TipoCadastro === CADASTRO_DESCENTRALIZADO.codigo)
			)
		) {
			setAcao(COMANDO_CADASTRO_FORNECEDOR.validar);
			callbackWarning(translate('possuemCamposPreenchidosIncorretamenteNaoAprovados'));
		} else {
			setAcao(COMANDO_CADASTRO_FORNECEDOR[getResultadoAnalise()]);
		}
	}

	function validarAnalise() {
		if (
			itensAnalise.find(
				x =>
					x.Status === ENUM_STATUS_ANALISE.find(y => y.internalName === 'Criado').value ||
					x.Status === null
			)
		) {
			return false;
		}
		return true;
	}

	function salvarAba(aba) {
		setAcao(COMANDO_CADASTRO_FORNECEDOR.salvar);
	}

	//Aprovacao
	const montarAnalise = dado => {
		// if (dado.AnaliseCadastro.ItensAnalise === null) {
		// 	dado.AnaliseCadastro.ItensAnalise = [];
		// } else {
		// 	dado.AnaliseCadastro.ItensAnalise.forEach(
		// 		x => (x.ArquivoId = x.ArquivoId === '' ? null : x.ArquivoId)
		// 	);
		// }
		//dado.AnaliseCadastro.AtribuidoId === '' ? null : dado.AnaliseCadastro.AtribuidoId;
		//return dado;
	};

	// Ações de retorno
	const callbackError = mensagem => {
		enqueueSnackbar('', snackError(mensagem, closeSnackbar));
	};
	const callbackWarning = mensagem => {
		enqueueSnackbar('', snackWarning(mensagem, closeSnackbar));
	};
	const alterTab = () => {
		salvarAba(tab);
		setTimeout(() => closeModal(), 50);
	};

	const closeModal = () => {
		setChanged(false);
		setOpenSalvarDados(false);
		setTab(tabChanged);
		setAcao(COMANDO_CADASTRO_FORNECEDOR.descartarAlteracoes);
	};

	const getIconIsValid = isValid => {
		return isValid ? <Valid /> : <Invalid />;
	};

	function getResultadoAnalise() {
		if (
			itensAnalise.find(
				x => x.Status === ENUM_STATUS_ANALISE.find(y => y.internalName === 'Reprovado').value
			)
		)
			return ENUM_STATUS_ANALISE.find(y => y.internalName === 'Reprovado').acao;

		if (
			itensAnalise.find(
				x =>
					x.Status === ENUM_STATUS_ANALISE.find(y => y.internalName === 'Aprovado_Ressalvas').value
			)
		)
			return ENUM_STATUS_ANALISE.find(y => y.internalName === 'Aprovado_Ressalvas').acao;

		return ENUM_STATUS_ANALISE.find(y => y.internalName === 'Aprovado').acao;
	}

	const setTabWithChangesCheck = newTab => {
		if (changed) {
			setTabChanged(newTab);
			setOpenSalvarDados(true);
		} else {
			setTab(newTab);
		}
	};

	return (
		<LayoutContent noCard>
			<Title text={paths.getTitle(paths.getCurrentPath(history))} />

			{comentarios.count > 0 && <p>{comentarios[0].Coment}</p>}
			<Modal
				open={modalLembreteAtualizacaoCadastro}
				handleClose={() => setModalLembreteAtualizacaoCadastro(false)}
				onClickButton={() => setModalLembreteAtualizacaoCadastro(false)}
				title={
					<h3 align='center'>
						{translate('Olá ')} {user !== null ? user.nome : ''}
					</h3>
				}
				textButton={translate('ok')}
				backgroundColorButtonCancel={theme.palette.secondary.main}
			>
				<Typography variant='h5' align='center'>
					{translate('msgAlertaAtualizacaoCadastro')}
				</Typography>
			</Modal>

			<Confirm
				open={openSalvarDados}
				handleClose={() => closeModal()}
				handleSuccess={() => alterTab()}
				title={translate('salvarDados')}
				text={translate('desejaSalvarDados')}
				textButtonSuccess={translate('salvar')}
				textButtonCancel={translate('descartarAlteracoes')}
				backgroundColorButtonCancel={theme.palette.secondary.main}
			/>
			
				<Box justifyContent='space-between' display='flex'>
					<Box>
						<Tabs
							value={tab}
							onChange={(event, newValue) => setTabWithChangesCheck(newValue)}
							indicatorColor='primary'
							textcolor='primary'
						>
							
								<Tab
									value={TAB_DADOS_BASICOS}
									label={translate('dadosBasicos')}
									icon={getIconIsValid(dadosBasicosIsValid)}
								/>
							

								<Tab
									value={TAB_DADOS_COMPLEMENTAR}
									label={translate('dadosComplementares')}
									icon={getIconIsValid(dadosComplementaresIsValid)}
								/>
							
								<Tab
									value={TAB_DADOS_FINANCEIROS}
									label={translate('dadosFinanceiros')}
									icon={getIconIsValid(dadosFinanceirosIsValid)}
								/>
							
								<Tab
									value={TAB_DADOS_SOCIOS}
									label={translate('dadosSocios')}
									icon={getIconIsValid(socioIsValid)}
								/>
							</Tabs>
					</Box>
				</Box>
				<Fragment>
					<DisplayDiv visible={tab === TAB_DADOS_BASICOS}>
						<DadosBasicos
							preCadastro
					acao={acao}
					setAcao={setAcao}
					setDadosBasicosIsValid={setDadosBasicosIsValid}
						/>
						<BotoesCadastro
							setAcao={setAcao}
							enviarCadastro={enviarCadastro}
							empresaFindById={empresaFindById}
							//disableEdit={getDisableEdit(user, empresa.AnaliseCadastro, null)}
							finalizarAnalise={finalizarAnalise}
							statusEmpresa={"Pendente Envio"}
						/>
					</DisplayDiv>

					<DisplayDiv visible={tab === TAB_DADOS_COMPLEMENTAR}>
						<DadosComplementar/>
					</DisplayDiv>
					<DisplayDiv visible={tab === TAB_DADOS_FINANCEIROS}>
						<DadosFinanceiros/>
					</DisplayDiv>
					<DisplayDiv visible={tab === TAB_DADOS_SOCIOS}>
						<DadosSocios/>
					</DisplayDiv>
				 </Fragment> 
			 
		</LayoutContent>
	);
}
