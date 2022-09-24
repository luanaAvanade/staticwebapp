/* eslint-disable import/no-unresolved */
import React, { useState, useEffect, Fragment } from 'react';
import { Route, Redirect } from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { Box, IconButton, Typography } from '@material-ui/core';
import useReactRouter from 'use-react-router';
import { HowToReg, PersonAdd, History, MonetizationOn } from '@material-ui/icons';
import { useSnackbar } from 'notistack';
import { Button, Modal, FormSelect, FormSelectWithSearch } from 'react-axxiom';
import { LayoutContent } from '@/layout';
import { Creators as LoaderCreators } from '@/store/ducks/loader';
import { translate } from '@/locales';
import { snackSuccess, snackError } from '@/utils/snack';
import theme from '@/theme';
import EmpresaService from '@/services/empresa';
import LoginService from '@/services/login';
import { getUser } from '@/utils/auth';
import ObjectHelper from '@/utils/objectHelper';
import { StatusEmpresa } from '@/screens/fornecedor/autoCadastro/componentesLayout/statusEmpresa';
import { FiltroAnalise } from './filtro';
import {
	SUBDIRETORIO_LINK,
	ROWSPERPAGE,
	TIPO_EMPRESA,
	ENUM_STATUS_ANALISE
} from '@/utils/constants';
import MaterialTable from 'material-table';
import paths from '@/utils/paths';
import { date } from 'yup';

export default function ListagemAnalise() {
	const { history } = useReactRouter();
	const { enqueueSnackbar, closeSnackbar } = useSnackbar();
	const dispatch = useDispatch();

	// Estado Local
	const [
		analiseCadastroList,
		setAnaliseCadastroList
	] = useState([]);

	const [
		analiseCadastroListInitial,
		setAnaliseCadastroListInitial
	] = useState([]);

	const [
		usuarioList,
		setUsuarioList
	] = useState([]);

	const [
		empresaId,
		setEmpresaId
	] = useState(null);

	const [
		assignee,
		setAssignee
	] = useState(null);

	const [
		user,
		setUser
	] = useState(null);

	const [
		order,
		setOrder
	] = useState('desc');

	const [
		openFiltro,
		setOpenFiltro
	] = useState(false);

	const [
		filtro,
		setFiltro
	] = useState(null);

	const [
		filtroMinhasAnalises,
		setFiltroMinhasAnalises
	] = useState(false);

	const [
		filtroAnalisesPendentes,
		setFiltroAnalisesPendentes
	] = useState(false);

	const [
		key,
		setKey
	] = useState(0);

	// Efeito Inicial

	useEffect(() => {
		analiseFindAll();
		return () => {
			setAnaliseCadastroList([]);
		};
	}, []);

	useEffect(
		() => {
			 if (filtro !== null) {
			filtrar();
			 }
			return () => {};
		},
		[
			filtro,
			filtroMinhasAnalises,
			filtroAnalisesPendentes
		]
	);

	const filtrar = () => {
		let temp = ObjectHelper.clone(analiseCadastroListInitial);

		//Filtro Modal
		if (filtro !== null) {
			temp = temp.filter(
				x =>
					(filtro['nomeEmpresa'] !== ''
						? x.NomeEmpresa.toUpperCase().includes(filtro['nomeEmpresa'].toUpperCase())
						: true) &&
					(filtro['cnpj'] !== '' ? x.CNPJ.includes(filtro['cnpj']) : true) &&
					(filtro['cpfCnpjSocio'] !== ''
						? x.Socios.some(y => y.Codigo.includes(filtro['cpfCnpjSocio']))
						: true) &&
					(filtro['estado'] !== 0
						? x.Enderecos.some(y => y.Municipio.EstadoId === filtro['estado'])
						: true) &&
					(filtro['municipio'] !== 0
						? x.Enderecos.some(y => y.Municipio.Id === filtro['municipio'])
						: true) &&
					(filtro['grupoFornecimento'] !== null
						? x.GruposFornecimento.some(y => y.GrupoCategoria.Id === filtro['grupoFornecimento'].Id)
						: true) &&
					(filtro['situacaoFornecedor'] !== 0 ? x.Situacao === filtro['situacaoFornecedor'] : true)
			);
		}

		if (filtroMinhasAnalises) {
			temp = temp.filter(
				x =>
					x.AnaliseCadastro.TransmitidoId === user.id || x.AnaliseCadastro.AtribuidoId === user.id
			);
		}
		if (filtroAnalisesPendentes) {
			temp = temp.filter(x => x.AnaliseCadastro.StatusAnalise === 1);
		}
		setAnaliseCadastroList(temp);
	};

	// Buscar Dados

	const analiseFindAll = async () => {
		//dispatch(LoaderCreators.setLoading());
		const u = getUser();
		const usuario = {
			perfilAnalista: true,
			id: "123"
		};

		
		setUser(usuario);


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


		var empresa = [
		{
			Id: 1,
			Situacao: 2,
			NomeEmpresa: 'Empresa telecom',
			CalculoRiscoLista: calcRiscoLista,
			CNPJ: '123456789',
			TipoEmpresa: '1',
			TipoCadastro: 'cadastro empresa',
			DataCriacao: '',
			AnaliseCadastro: AnaliseCadastro
			
		}

];

		 	setAnaliseCadastroList(empresa);
		 	setAnaliseCadastroListInitial(empresa);

		// const response = await EmpresaService.findEmpresasWithWhereClause('Id!=null');
		// if (response.data) {
		// 	let t = response.data.Empresa_list.sort(sort);
		// 	setAnaliseCadastroList(t);
		// 	setAnaliseCadastroListInitial(t);
		// 	const responseUsers = await LoginService.getUsersInRole('Analista de Cadastro');
		// 	if (responseUsers) {
		// 		setUsuarioList(responseUsers);
		// 	} else {
		// 		callbackError(translate('erroCarregarDados'));
		// 	}
		// 	dispatch(LoaderCreators.disableLoading());
		// } else {
		// 	callbackError(translate('erroCarregarDados'));
		// 	dispatch(LoaderCreators.disableLoading());
		// }
	};

	function sort(a, b) {
		try {
			if (a.AnaliseCadastro === null || a.AnaliseCadastro.StatusAnalise === null) {
				a.AnaliseCadastro.StatusAnalise = 0;
			}
			if (b.AnaliseCadastro === null || b.AnaliseCadastro.StatusAnalise === null) {
				b.AnaliseCadastro.StatusAnalise = 0;
			}
			if (a.AnaliseCadastro.StatusAnalise - b.AnaliseCadastro.StatusAnalise === 0) {
				return -(a.DataCriacao - b.DataCriacao);
			} else {
				return -(a.AnaliseCadastro.StatusAnalise - b.AnaliseCadastro.StatusAnalise);
			}
		} catch (error) {}
	}
	// // Ações da Tela

	const open = id => {
		history.push(`@/..{SUBDIRETORIO_LINK}/cadastro-complementar/@/..{id}`);
	};

	const selfAssignee = async idEmpresa => {
		dispatch(LoaderCreators.setLoading());
		try {
			let empresaSave = {
				AnaliseCadastro: {
					Id: analiseCadastroList.find(x => x.Id === idEmpresa).AnaliseCadastro.Id,
					StatusAnalise: ENUM_STATUS_ANALISE.find(x => x.internalName === 'Em_Analise')
						.internalName,
					AtribuidoId: user.id
				}
			};
			const response = await EmpresaService.update(idEmpresa, empresaSave);

			if (response.data && response.data.Empresa_update) {
				history.push(`@/..{SUBDIRETORIO_LINK}/cadastro-complementar/@/..{idEmpresa}`);
				//analiseFindAll();
				callback(translate('sucessoAtribuicaoAnalise'));
			} else {
				callbackError(translate('erroAtribuicaoAnalise'));
			}
		} catch (error) {
			callbackError(translate('erroAtribuicaoAnalise'));
		}
	};

	const assigneeTo = async () => {
		if (assignee) {
			dispatch(LoaderCreators.setLoading());
			try {
				let empresaSave = {
					AnaliseCadastro: {
						Id: analiseCadastroList.find(x => x.Id === empresaId).AnaliseCadastro.Id,
						StatusAnalise: ENUM_STATUS_ANALISE.find(
							x =>
								x.value ===
								analiseCadastroList.find(x => x.Id === empresaId).AnaliseCadastro.StatusAnalise
						).internalName,
						AtribuidoId:
							analiseCadastroList.find(x => x.Id === empresaId).AnaliseCadastro.AtribuidoId === ''
								? null
								: analiseCadastroList.find(x => x.Id === empresaId).AnaliseCadastro.AtribuidoId,
						TransmitidoId: assignee
					}
				};
				const response = await EmpresaService.update(empresaId, empresaSave);
				setEmpresaId(null);
				setAssignee(null);

				if (response.data && response.data.Empresa_update) {
					analiseFindAll();
					callback(translate('sucessoAtribuicaoAnalise'));
				} else {
					callbackError(translate('erroAtribuicaoAnalise'));
				}
			} catch (error) {
				callbackError(translate('erroAtribuicaoAnalise'));
			}
		}
	};

	const getResponsable = empresa => {
		if (empresa.AnaliseCadastro.AtribuidoId !== '') {
			return (
				<Box display='flex' justify='space-between'>
					<HowToReg style={{ width: '15%', paddingRight: '8px' }} />
					<Typography variant='caption' color='textSecondary'>
						{`${empresa.AnaliseCadastro.AtribuidoId ? empresa.AnaliseCadastro.Atribuido.Nome : ''}`}
					</Typography>
				</Box>
			);
		} else {
			if (empresa.AnaliseCadastro.TransmitidoId !== '') {
				return (
					<Box display='flex' justify='space-between'>
						<PersonAdd style={{ width: '15%', paddingRight: '8px' }} />
						<Typography variant='caption' color='textSecondary'>
							{`${empresa.AnaliseCadastro.TransmitidoId
								? empresa.AnaliseCadastro.Transmitido.Nome
								: ''}`}
						</Typography>
					</Box>
				);
			}
			return null;
		}
	};

	// Ações de Retorno

	const callback = mensagem => {
		analiseFindAll();
		dispatch(LoaderCreators.disableLoading());
		enqueueSnackbar('', snackSuccess(mensagem, closeSnackbar));
	};

	const callbackError = mensagem => {
		dispatch(LoaderCreators.disableLoading());
		enqueueSnackbar('', snackError(mensagem, closeSnackbar));
	};

	const voltar = () => {
		history.goBack();
	};

	// Interação com a Tabela

	const gruposAtivar = [
		'Pendente_Analise',
		'Em_Analise'
	];

	function desablitarAcoes(empresa, botao) {
		if (empresa === undefined) return true;
		if (empresa.AnaliseCadastro.StatusAnalise === '') {
			return true;
		}
		var t = !gruposAtivar.includes(
			ENUM_STATUS_ANALISE.find(x => x.value === empresa.AnaliseCadastro.StatusAnalise).internalName
		);
		if (botao === 'assumir') {
			return empresa.AnaliseCadastro.StatusAnalise === 2 || t;
		}
		return t;
	}

	function getPropRisco(calcRiscoLista) {
		if (calcRiscoLista.length <= 0) return { label: translate('semDadosAExibir'), color: `gray` };
		switch (calcRiscoLista[0].ClassificacaoFase1) {
			case 1:
				return { label: translate('riscoBaixo'), color: `green` };
			case 2:
				return { label: translate('riscoMedio'), color: 'yellow' };
			case 3:
				return { label: translate('riscoAlto'), color: 'red' };
			default:
				return { label: translate('riscoNaoDefinido'), color: 'blue' };
		}
	}

	const semPermissao = () => {
		dispatch(LoaderCreators.disableLoading());
		history.push(paths.getPathByCodigo('semPermissao'));
	};

	if (user && !user.perfilAnalista) {
		semPermissao();
	}

	const setFiltroAnalises = value => {
		if (value == 'MinhasAnalises') {
			setFiltroMinhasAnalises(true);
			setFiltroAnalisesPendentes(false);
		} else if (value == 'AnalisesPendentes') {
			setFiltroMinhasAnalises(false);
			setFiltroAnalisesPendentes(true);
		}
	};

	return (
		<LayoutContent>
			{user &&
			user.perfilAnalista && (
				<Fragment>
					<FiltroAnalise
						openFiltro={openFiltro}
						setOpenFiltro={setOpenFiltro}
						setFiltro={setFiltro}
					/>
					<Modal
						open={empresaId}
						handleClose={() => setEmpresaId(null)}
						title={
							empresaId ? (
								`${translate('transmitirAnalise')}: ${analiseCadastroList.find(
									x => x.Id === empresaId
								).NomeEmpresa}`
							) : (
								translate('transmitirAnalise')
							)
						}
						textButton={translate('transmitir')}
						onClickButton={() => assigneeTo()}
					>
						<FormSelectWithSearch
							key={key}
							placeholder={`${translate('selecioneOpcao')}`}
							label={`${translate('usuario')}`}
							options={usuarioList}
							getOptionLabel={option => option.nome}
							//value={event.target.value}
							onChange={(event, selected) => setAssignee(selected == null ? null : selected.id)}
						/>
					</Modal>
					<Box display='flex' justify='flex-start' justifyContent='space-between' width='30%'>
						<Button
							text={translate('minhasAnalises')}
							backgroundColor={
								!filtroMinhasAnalises ? theme.palette.primary.main : theme.palette.secondary.main
							}
							//onClick={() => setFiltroAnalises('MinhasAnalises')}
						/>
						<Button
							text={translate('analisesPendentes')}
							backgroundColor={
								!filtroAnalisesPendentes ? theme.palette.primary.main : theme.palette.secondary.main
							}
							onClick={() => setFiltroAnalises('AnalisesPendentes')}
						/>
					</Box>
					<link rel='stylesheet' href='https://fonts.googleapis.com/icon?family=Material+Icons' />
					<MaterialTable
						key={key}
						columns={[
							{
								title: 'Fornecedor',
								field: 'NomeEmpresa',
								// defaultFilter: rowData.NomeEmpresa.includes(filtro['nomeEmpresa']),
								cellStyle: {
									width: '35%'
								},
								render: rowData => (
									<Box onClick={() => open(rowData.Id)} style={{ cursor: 'pointer' }}>
										<Typography
											variant='caption'
											color='textPrimary'
											style={{
												fontWeight: 'bold',
												textDecoration: rowData.Situacao === 2 ? 'line-through' : ''
											}}
										>
											{rowData.NomeEmpresa}
										</Typography>
										<IconButton
											size='small'
											// onClick={() => selfAssignee(rowData.Id)}
											title={getPropRisco(rowData.CalculoRiscoLista).label}
											style={{
												color: getPropRisco(rowData.CalculoRiscoLista).color
											}}
										>
											<MonetizationOn />
										</IconButton>
										<br />
										<Typography variant='caption' color='textSecondary'>
											{`Cnpj: ${rowData.CNPJ}`}
										</Typography>
									</Box>
								),
								customFilterAndSearch: (term, rowData) =>
									rowData.NomeEmpresa.toUpperCase().includes(term.toUpperCase()) ||
									rowData.CNPJ.includes(term)
							},
							{
								title: 'Tipo Empresa',
								field: 'TipoEmpresa',
								render: rowData => (
									<Box width='100%'>
										<Typography variant='caption' color='textSecondary'>
											{`${translate(
												TIPO_EMPRESA.find(x => x.value === rowData.TipoEmpresa)
													? TIPO_EMPRESA.find(x => x.value === rowData.TipoEmpresa).internalName
													: ''
											)}
											|| ${translate(rowData.TipoCadastro)}`}
											<br />
											{`Data criação: ${ObjectHelper.showData(rowData.DataCriacao)}`}
										</Typography>
									</Box>
								),
								customFilterAndSearch: (term, rowData) => {
									var conteudo = `${translate(
										TIPO_EMPRESA.find(x => x.value === rowData.TipoEmpresa)
											? TIPO_EMPRESA.find(x => x.value === rowData.TipoEmpresa).internalName
											: ''
									)}
									|| ${translate(rowData.TipoCadastro)} 
									Data criação: ${ObjectHelper.showData(rowData.DataCriacao)}`;

									return conteudo.toUpperCase().includes(term.toUpperCase());
								},
								cellStyle: {
									width: '30%'
								}
							},
							{
								title: 'Status',
								field: 'AnaliseCadastro.StatusAnalise',
								render: rowData => (
									<Box margin='0% 5% 0% 5%'>
										<StatusEmpresa statusEmpresa={rowData.AnaliseCadastro.StatusAnalise} />
										{getResponsable(rowData)}
									</Box>
								),
								customFilterAndSearch: (term, rowData) =>
									ENUM_STATUS_ANALISE.find(
										x => x.value === rowData.AnaliseCadastro.StatusAnalise
									).label.includes(term),
								cellStyle: {
									width: '20%'
								}
							},

							{
								title: 'Ações',
								render: rowData => (
									<Box display='flex' justify='space-between' margin='auto'>
										<IconButton
											size='small'
											onClick={() => selfAssignee(rowData.Id)}
											title={'Assumir'}
											style={{
												color: `rgb(36,36,156,${desablitarAcoes(rowData, 'assumir') ? 0.3 : 1})`
											}}
											disabled={desablitarAcoes(rowData, 'assumir')}
										>
											<HowToReg />
										</IconButton>
										<IconButton
											size='small'
											onClick={() => setEmpresaId(rowData.Id)}
											title={'Transmitir'}
											style={{
												color: `rgb(51,102,102,${desablitarAcoes(rowData) ? 0.3 : 1})`
											}}
											disabled={desablitarAcoes(rowData)}
										>
											<PersonAdd />
										</IconButton>
										<IconButton
											size='small'
											onClick={() => alert('Funcionalidade ainda não implementada!')}
											title={'Funcionalidade ainda não implementada!'}
											style={{
												color: `rgb(0,0,255,0.3)`
											}}
										>
											<History />
										</IconButton>
									</Box>
								),
								cellStyle: {
									width: '5%'
								},
								filter: false,
								search: false,
								sorting: false
							}
						]}
						data={analiseCadastroList}
						actions={[
							{
								icon: 'swap_vert',
								tooltip: translate('reordenar'),
								onClick: (event, rowData) => {
									let temp = ObjectHelper.clone(analiseCadastroList);
									temp = temp.sort(sort);
									setAnaliseCadastroList(temp);
									setKey(key + 1);
								},
								isFreeAction: true
							},
							{
								icon: 'filter_list',
								tooltip: translate('filtro'),
								onClick: (event, rowData) => setOpenFiltro(true),
								isFreeAction: true
							},
							{
								icon: 'clear',
								tooltip: translate('limparFiltros'),
								onClick: (event, rowData) => {
									setFiltro(null);
									setFiltroAnalisesPendentes(false);
									setFiltroMinhasAnalises(false);
									analiseFindAll();
								},
								isFreeAction: true
							}
						]}
						options={{
							pageSizeOptions: ROWSPERPAGE,
							filtering: true,
							padding: 'dense',
							headerStyle: {
								backgroundColor: '#336666',
								color: '#FFF',
								padding: '0px 8px 0px 8px',
								textAlign: 'center',
								alignContent: 'center'
							},
							filterCellStyle: {
								padding: '0px',
								minHeight: '25px'
							},
							actionsColumnIndex: 3,
							columnsButton: true,
							showTitle: false,
							rowStyle: (rowData, index, level) => {
								return {
									backgroundColor:
										index % 2 === 0
											? '#eee' //theme.palette.table.tableRowPrimary
											: '#ddd' //theme.palette.table.tableRowSecondary
								};
							},
							actionsCellStyle: {
								padding: '0% 3% 0% 3%'
							}
						}}
						localization={{
							pagination: {
								labelRowsPerPage: 'Linhas',
								labelRowsSelect: 'Linhas',
								firstTooltip: 'Primeira Página',
								firstAriaLabel: 'Primeira Página',
								previousTooltip: 'Página Anterior',
								previousAriaLabel: 'Anterior',
								nextTooltip: 'Próxima Página',
								nextAriaLabel: 'Próxima',
								lastTooltip: 'Última Página',
								lastAriaLabel: 'Última'
							},
							toolbar: {
								searchPlaceholder: 'Pesquisar',
								showColumnsTitle: 'Mostrar título da coluna',
								searchTooltip: 'Pesquisar'
							},
							header: {
								actions: 'string;'
							}
						}}
					/>

					<Box display='flex' justifyContent='flex-end' padding='5px'>
						<Button
							text={translate('voltar')}
							backgroundColor={theme.palette.secondary.main}
							onClick={voltar}
						/>
					</Box>
				</Fragment>
			)}
		</LayoutContent>
	);
}
