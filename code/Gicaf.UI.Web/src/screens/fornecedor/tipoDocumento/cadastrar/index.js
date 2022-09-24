import React, { useState, useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { useFormik, Form } from 'formik';
import * as Yup from 'yup';
import { useSnackbar } from 'notistack';
import {
	Box,
	Checkbox,
	Typography,
	TableBody,
	IconButton,
	Select,
	ListItemText,
	MenuItem,
	OutlinedInput,
	Tab,
	Tabs
} from '@material-ui/core';
import { Add, Edit, Delete } from '@material-ui/icons';
import useReactRouter from 'use-react-router';
import _ from 'lodash';
import {
	Button,
	Confirm,
	FormInput,
	FormSelect,
	Modal,
	Table,
	TableHead,
	TableRow,
	TableCell
} from 'react-axxiom';
import { LayoutContent } from '@/layout';
import { translate, translateWithHtml } from '@/locales';
import { Creators as LoaderCreators } from '@/store/ducks/loader';
import { snackSuccess, snackError, snackWarning } from '@/utils/snack';
import theme from '@/theme';
import { checkError } from '@/utils/validation';
import ObjectHelper from '@/utils/objectHelper';
import { BoxContentTab } from './style';
import { Switch } from '../../style';

export default function CadastrarTipoDocumento() {
	const { history, match } = useReactRouter();
	const id = isNaN(parseInt(match.params.id, 10)) ? null : parseInt(match.params.id, 10);
	const { enqueueSnackbar, closeSnackbar } = useSnackbar();
	const dispatch = useDispatch();

	// Estado Local

	const [
		tiposArquivosPermitidosList,
		setTiposArquivosPermitidosList
	] = useState([]);

	const [
		openConfirmCancelar,
		setOpenConfirmBotaoCancelar
	] = useState(false);

	const [
		relacionamentoListField,
		setRelacionamentoListField
	] = useState([]);

	const [
		validadeDocumentoEstadoList,
		setValidadeDocumentoEstadoList
	] = useState([]);

	const [
		funcionalidadeIdsListInitial,
		setFuncionalidadeIdsListInitial
	] = useState([]);

	const [
		openConfirmAlterar,
		setOpenConfirmAlterar
	] = useState(false);

	const [
		estadoList,
		setEstadoList
	] = useState([]);

	const [
		flagSelectTodosTipoArq,
		setFlagSelectTodosTipoArq
	] = useState(false);

	const [
		flagValidadeDocumentoEstadoEditId,
		setFlagValidadeDocumentoEstadoEditId
	] = useState(null);

	const [
		flagEditValidadeEstadoListFuncionalidadeId,
		setFlagEditValidadeEstadoListFuncionalidadeId
	] = useState(null);

	const [
		funcionalidadesTipoDoc,
		setFuncionalidadesTipoDoc
	] = useState([]);

	const [
		tab,
		setTab
	] = useState(0);
	const [
		status,
		setStatus
	] = useState(false);

	// Efeito Inicial

	useEffect(() => {
		setEstados()
			.then(() => {
				if (id) {
					tipoDocumentoFindById();
				}
			})
			.catch(() => {
				if (id) {
					tipoDocumentoFindById();
				}
			});

		return () => {
			setValidadeDocumentoEstadoList([]);
			setEstadoList([]);
		};
	}, []);

	// Ações de Retorno
	const botaoCancelar = () => {
		setOpenConfirmBotaoCancelar(true);
	};

	const callback = mensagem => {
		voltar();
		enqueueSnackbar('', snackSuccess(mensagem, closeSnackbar));
	};

	const callbackError = (mensagem, response) => {
		enqueueSnackbar('', snackError(mensagem, closeSnackbar));
	};
	const callbackWarning = mensagem => {
		enqueueSnackbar('', snackWarning(mensagem, closeSnackbar));
	};

	const voltar = () => {
		history.goBack();
	};

	// Busca de Dados

	const tipoDocumentoFindById = async () => {
		dispatch(LoaderCreators.setLoading());
		const response = null;
		if (response.data) {
			const r = response.data.TipoDocumento_list[0];
			setFieldValue('nome', r.Nome, false);
			setFieldValue('helpDoc', r.Help, false);
			setFieldValue('obrigatorio', r.Obrigatorio, false);
			setFieldValue('qtdMaxima', r.QuantidadeMaxima === '' ? null : r.QuantidadeMaxima, false);
			setFieldValue('tamanhoMaximo', r.TamanhoMaximo === '' ? null : r.TamanhoMaximo, false);
			setFieldValue('validadeMeses', r.ValidadeMeses === '' ? null : r.ValidadeMeses, false);
			setFuncionalidadesTipoDoc(
				r.TipoDocumentoFuncionalidade.map(item => {
					item.Funcionalidade = selectRelac.find(x => x.value === item.Funcionalidade).internalName;
					return item;
				})
			);
			setFuncionalidadeIdsListInitial(r.TipoDocumentoFuncionalidade.map(item => item.Id));
			r.TiposArquivos = enumFuncReverse(r.TiposArquivos);
			if (r.TiposArquivos.length === 4) {
				r.TiposArquivos = [
					translate('Todos')
				].concat(r.TiposArquivos);
				setFlagSelectTodosTipoArq(true);
			}
			setTiposArquivosPermitidosList(r.TiposArquivos);
			dispatch(LoaderCreators.disableLoading());
		} else {
			dispatch(LoaderCreators.disableLoading());
		}
	};

	// Monta Estrutura de Dados - Visão Geral
	function getVisaoGeral() {
		const visaoGeral = [];
		let Index = 0;
		visaoGeral.push({
			index: Index,
			Padrao: nome.value,
			Class: 0,
			TipoDocumento: nome.value,
			Funcionalidade: '',
			Estado: '',
			ValidadeMeses: validadeMeses.value,
			Obrigatorio: obrigatorio.value,
			Status: status
		});

		funcionalidadesTipoDoc.map(itemFuncionalidade => {
			visaoGeral.push({
				index: (Index += 1),
				TipoDocumento: '',
				Padrao: selectRelac.find(func => func.internalName === itemFuncionalidade.Funcionalidade)
					.label,
				Class: 50,
				Funcionalidade: selectRelac.find(
					func => func.internalName === itemFuncionalidade.Funcionalidade
				).label,
				Estado: '',
				ValidadeMeses: itemFuncionalidade.ValidadeMeses,
				Obrigatorio: itemFuncionalidade.Obrigatorio,
				Status: status
			});

			itemFuncionalidade.ValidadeDocumentoEstado.map(itemValidadeEstado => {
				visaoGeral.push({
					index: (Index += 1),
					Padrao: estadoList.find(e => e.value === itemValidadeEstado.EstadoId).label,
					Class: 100,
					TipoDocumento: '',
					Funcionalidade: '',
					Estado: estadoList.find(e => e.value === itemValidadeEstado.EstadoId).label,
					ValidadeMeses: itemValidadeEstado.ValidadeMeses,
					Obrigatorio: itemValidadeEstado.Obrigatorio,
					Status: status
				});
				return null;
			});

			return null;
		});

		return visaoGeral;
	}

	// Ações da Tela
	const save = () => {
		if (isValid) {
			if (id) {
				setOpenConfirmAlterar(true);
			} else {
				create();
			}
		}
	};

	const addValidadeDocumentoEstado = () => {
		const list = ObjectHelper.clone(validadeDocumentoEstadoList);
		if (estado.value === 0) return;
		if (flagValidadeDocumentoEstadoEditId == null) {
			if (!list.find(x => x.EstadoId === estado.value)) {
				list.push({
					EstadoId: estado.value,
					ValidadeMeses: validadeMesesEstado.value === '' ? null : validadeMesesEstado.value,
					Obrigatorio: obrigatorioEstado.value
				});
			} else {
				callbackError(translate('estadoJaCadastrado'), null);
			}
		} else {
			list[list.findIndex(x => x.EstadoId === flagValidadeDocumentoEstadoEditId)].ValidadeMeses =
				validadeMesesEstado.value === '' ? null : validadeMesesEstado.value;

			list[list.findIndex(x => x.EstadoId === flagValidadeDocumentoEstadoEditId)].Obrigatorio =
				obrigatorioEstado.value;
			setFlagValidadeDocumentoEstadoEditId(false);
		}

		setValidadeDocumentoEstadoList(list);
		setFieldValue('estado', 0);
		setFieldValue('validadeMesesEstado', '');
		setFieldValue('obrigatorioEstado', false);
		setFieldValue(flagAddRelacionamento, false);
		setFlagValidadeDocumentoEstadoEditId(null);
		setFieldValue(flagAddRelacionamento, false);
	};

	const removeValidadeDocumentoEstado = estadoId => {
		const list = ObjectHelper.clone(validadeDocumentoEstadoList);

		list.splice(list.findIndex(x => x.EstadoId === estadoId), 1);
		setValidadeDocumentoEstadoList(list);
	};

	const create = async () => {
		const list = null;

		if (list.data && list.data.TipoDocumento_list && list.data.TipoDocumento_list.length === 0) {
			const tiposArq = enumFunc(selectTiposArquivos, tiposArquivosPermitidosList);
			if (funcionalidadesTipoDoc.length === 0) {
				callbackWarning(translate('associacaoFuncionalidadeObrigatoria'), null);
				return;
			}
					} else {
			enqueueSnackbar(
				'',
				snackWarning(translateWithHtml('jaExisteUmTipoDocumentoComEsteNome'), closeSnackbar)
			);
		}
	};

	const update = async () => {
		const list = null;

		if (
			list.data &&
			list.data.TipoDocumento_list &&
			(list.data.TipoDocumento_list.length === 0 || list.data.TipoDocumento_list[0].Id === id)
		) {
			setOpenConfirmAlterar(false);
			if (funcionalidadesTipoDoc.length === 0) {
				callbackError(translate('associacaoFuncionalidadeObrigatoria'), null);
				return;
			}
			const tiposArq = enumFunc(selectTiposArquivos, tiposArquivosPermitidosList);
			
		} else {
			setOpenConfirmAlterar(false);
			enqueueSnackbar(
				'',
				snackWarning(translateWithHtml('jaExisteUmTipoDocumentoComEsteNome'), closeSnackbar)
			);
		}
	};

	const handleChangeCheck = field => {
		if (field.value) {
			setFieldValue(field.name, false);
		} else {
			setFieldValue(field.name, true);
		}
	};

	const addRelac = event => {
		const list = ObjectHelper.clone(relacionamentoListField);
		const itemLabel = event.nativeEvent.path.filter(i => i.localName === 'li')[0].innerText;
		const item = selectRelac.find(relac => relac.label === itemLabel).internalName;
		if (list.indexOf(item) > -1) {
			list.splice(list.indexOf(item), 1);
		} else {
			list.push(item);
		}
		setRelacionamentoListField(list);
	};

	const addTipoArq = event => {
		if (event.target.value[event.target.value.length - 1] === translate('Todos')) {
			setFlagSelectTodosTipoArq(true);
			setTiposArquivosPermitidosList(selectTiposArquivos.map(item => item.label));
		} else if (flagSelectTodosTipoArq && !event.target.value.includes(translate('Todos'))) {
			setFlagSelectTodosTipoArq(false);
			setTiposArquivosPermitidosList([]);
		} else if (
			flagSelectTodosTipoArq &&
			event.target.value[event.target.value.length - 1] !== translate('Todos')
		) {
			event.target.value.splice(event.target.value.indexOf(translate('Todos')), 1);
			setFlagSelectTodosTipoArq(false);
			setTiposArquivosPermitidosList(event.target.value);
		} else setTiposArquivosPermitidosList(event.target.value);
	};

	const setEstados = async () => {
		const response = null;
		if (response.data) {
			setEstadoList(response.data.Estado_list);
		} else {
			callbackError(translate('erroCarregamentoEstados'), response);
		}
	};

	const editarValidadeDocumentoEstado = validadeDocumento => {
		setFieldValue('estado', validadeDocumento.EstadoId);
		setFieldValue('validadeMesesEstado', validadeDocumento.ValidadeMeses);
		setFieldValue('obrigatorioEstado', validadeDocumento.Obrigatorio);
		setFlagValidadeDocumentoEstadoEditId(validadeDocumento.EstadoId);
		setFieldValue('AddValidadeEstado', true);
	};

	const addFuncDoc = () => {
		if (obrigatorioRelac != null && validadeMesesRelac != null && relacionamentoListField != null) {
			const list = ObjectHelper.clone(funcionalidadesTipoDoc);
			relacionamentoListField.forEach(element => {
				const indexFunc = list.findIndex(x => x.Funcionalidade === element);
				if (indexFunc < 0) {
					list.push({
						Funcionalidade: element,
						Obrigatorio: obrigatorioRelac.value,
						ValidadeMeses: validadeMesesRelac.value === '' ? null : validadeMesesRelac.value,
						ValidadeDocumentoEstado: []
					});
				} else {
					list[indexFunc] = {
						Funcionalidade: element,
						Obrigatorio: obrigatorioRelac.value,
						ValidadeMeses: validadeMesesRelac.value === '' ? null : validadeMesesRelac.value,
						ValidadeDocumentoEstado: list[indexFunc].ValidadeDocumentoEstado
					};
				}
			});

			setFuncionalidadesTipoDoc(list);
			setFieldValue('validadeMesesRelac', null);
			setFieldValue('flagAddRelacionamento', false);
			setRelacionamentoListField([]);
		} else {
			callbackError(translate('PreenchaCamposObrigatorios'));
		}
	};

	const removeFuncDoc = internalName => {
		const list = ObjectHelper.clone(funcionalidadesTipoDoc);
		const indexFunc = list.findIndex(x => x.Funcionalidade === internalName);

		if (indexFunc >= 0) {
			list.splice(indexFunc, 1);
		}

		setFuncionalidadesTipoDoc(list);
	};

	const editValidadeEstadoList = FuncionalidadeInternalName => {
		const funcionalidade = funcionalidadesTipoDoc.find(
			x => x.Funcionalidade === FuncionalidadeInternalName
		);
		setValidadeDocumentoEstadoList(funcionalidade.ValidadeDocumentoEstado);
		setFieldValue('flagAddValidadeEstado', true);
		setFlagEditValidadeEstadoListFuncionalidadeId(FuncionalidadeInternalName);
	};

	const updateValidadeDocumentoListFuncionalidade = () => {
		const list = ObjectHelper.clone(funcionalidadesTipoDoc);
		const indexFunc = list.findIndex(
			x => x.Funcionalidade === flagEditValidadeEstadoListFuncionalidadeId
		);

		if (indexFunc >= 0) {
			list[indexFunc].ValidadeDocumentoEstado = validadeDocumentoEstadoList;
		}

		setFuncionalidadesTipoDoc(list);
		setFieldValue('flagAddValidadeEstado', false);
	};

	function enumFunc(listRef, listValues) {
		let total = 0;
		for (let r = 0; r < listValues.length; r += 1) {
			total += listRef.find(x => x.label === listValues[r]).value;
		}
		return total;
	}
//todo[iuri] mudar para lista de tipos de arquivo
	function enumFuncReverse(int) {
		switch (int) {
			case 0:
				return [];
			case 1:
				return [
					'PDF'
				];
			case 2:
				return [
					'XML'
				];
			case 4:
				return [
					'CSV'
				];
			case 8:
				return [
					'JPG'
				];

			case 3:
				return [
					'PDF',
					'XML'
				];
			case 5:
				return [
					'PDF',
					'CSV'
				];
			case 9:
				return [
					'PDF',
					'JPG'
				];
			case 6:
				return [
					'XML',
					'CSV'
				];
			case 10:
				return [
					'XML',
					'JPG'
				];
			case 12:
				return [
					'CSV',
					'JPG'
				];
			case 7:
				return [
					'PDF',
					'XML',
					'CSV'
				];
			case 11:
				return [
					'PDF',
					'XML',
					'JPG'
				];
			case 13:
				return [
					'PDF',
					'CSV',
					'JPG'
				];
			case 14:
				return [
					'XML',
					'CSV',
					'JPG'
				];
			case 15:
				return [
					'PDF',
					'XML',
					'CSV',
					'JPG'
				];
			default:
				return [];
		}
	}

	const alterTab = optTab => {
		setTab(optTab);
	};

	// Formulário

	const initialValues = {
		nome: '',
		helpDoc: '',
		obrigatorio: false,
		validadeMeses: null,
		validadeDocumentoList: [],
		validadeMesesEstado: null,
		validadeMesesRelac: null,
		estado: 0,
		tamanhoMaximo: null,
		qtdMaxima: null,
		validadeDocumento: null,
		addValidadeEstado: false,
		obrigatorioEstado: false,
		addRelacionamento: false
	};

	const validationSchema = Yup.object().shape({
		nome: Yup.string().required(translate('campoObrigatorio')),
		helpDoc: Yup.string().required(translate('campoObrigatorio'))
	});

	const { getFieldProps, handleSubmit, submitCount, isValid, setFieldValue } = useFormik({
		initialValues,
		validationSchema,
		onSubmit: save
	});

	const [
		nome,
		metadataNome
	] = getFieldProps('nome', 'text');

	const [
		helpDoc,
		metadataHelpDoc
	] = getFieldProps('helpDoc', 'text');

	const [
		tamanhoMaximo
	] = getFieldProps('tamanhoMaximo', 'int');

	const [
		qtdMaxima
	] = getFieldProps('qtdMaxima', 'int');

	const [
		obrigatorio
	] = getFieldProps('obrigatorio', 'boolean');

	const [
		obrigatorioRelac
	] = getFieldProps('obrigatorioRelac', 'boolean');

	const [
		obrigatorioEstado
	] = getFieldProps('obrigatorioEstado', 'boolean');

	const [
		validadeMeses
	] = getFieldProps('validadeMeses', 'int');

	const [
		validadeMesesRelac
	] = getFieldProps('validadeMesesRelac', 'int');

	const [
		validadeMesesEstado
	] = getFieldProps('validadeMesesEstado', 'int');

	const [
		estado
	] = getFieldProps('estado', 'text');

	const [
		flagAddRelacionamento
	] = getFieldProps('flagAddRelacionamento', 'boolean');

	const [
		FlagAddValidadeEstado
	] = getFieldProps('flagAddValidadeEstado', 'boolean');

	const selectRelac = [
		{
			value: 1,
			label: translate('fornecedorMEI'),
			internalName: 'Cadastro_MEI',
			parent: translate('telaCadastroFornecedor')
		},
		{
			value: 2,
			label: translate('fornecedorPessoaJuridica'),
			internalName: 'Cadastro_Pessoa_Juridica',
			parent: translate('telaCadastroFornecedor')
		},
		{
			value: 3,
			label: translate('fornecedorEmpresaEstrangeira'),
			internalName: 'Cadastro_Empresa_Estrangeira',
			parent: translate('telaCadastroFornecedor')
		},

		{
			value: 5,
			label: translate('socioPessoaJuridicaCentralizado'),
			internalName: 'Cadastro_Socio_Pessoa_Juridica',
			parent: translate('telaCadastroSocios')
		},
		{
			value: 6,
			label: translate('socioEmpresaEstrangeiraCentralizado'),
			internalName: 'Cadastro_Socio_Empresa_Estrangeira',
			parent: translate('telaCadastroSocios')
		},
		{
			value: 7,
			label: translate('financeiroMEIfinanceiroMEICentralizado'),
			internalName: 'Cadastro_Financeiro_MEI',
			parent: translate('telaCadastroFinanceiro')
		},
		{
			value: 8,
			label: translate('financeiroPessoaJuridicaCentralizado'),
			internalName: 'Cadastro_Financeiro_Pessoa_Juridica',
			parent: translate('telaCadastroFinanceiro')
		},
		{
			value: 9,
			label: translate('financeiroEmpresaEstrangeiraCentralizado'),
			internalName: 'Cadastro_Financeiro_Empresa_Estrangeira',
			parent: translate('telaCadastroFinanceiro')
		},
		{
			value: 10,
			label: translate('fornecedorMEIDecentralizado'),
			internalName: 'Cadastro_MEI_Descentralizado',
			parent: translate('telaCadastroFornecedor')
		},
		{
			value: 11,
			label: translate('fornecedorPessoaJuridicaDecentralizado'),
			internalName: 'Cadastro_Pessoa_Juridica_Descentralizado',
			parent: translate('telaCadastroFornecedor')
		},
		{
			value: 12,
			label: translate('fornecedorEmpresaEstrangeiraDecentralizado'),
			internalName: 'Cadastro_Empresa_Estrangeira_Descentralizado',
			parent: translate('telaCadastroFornecedor')
		}
	];

	const parents = [
		translate('telaCadastroFornecedor'),
		translate('telaCadastroSocios'),
		translate('telaCadastroFinanceiro')
	];

	const selectTiposArquivos = [
		{ value: 0, label: `@/..{translate('Todos')}`, internalName: 'Todos' },
		{ value: 1, label: 'PDF' },
		{ value: 2, label: 'XML' },
		{ value: 4, label: 'CSV' },
		{ value: 8, label: 'JPG' }
	];

	const columnsEstados = [
		{ id: 'Estado', label: translate('estado'), width: '20%' },
		{ id: 'Validade', label: translate('validadeMeses'), width: '20%' },
		{ id: 'Obrigatorio', label: translate('obrigatorio'), width: '20%' },
		{ id: 'Acoes', label: translate('acoes'), width: '5%', colSpan: 2, align: 'center' }
	];

	const columnsRelac = [
		{ id: 'Funcionalidade', label: translate('ondeExigir'), width: '45%' },
		{ id: 'Validade', label: translate('validadeMeses'), width: '25%' },
		{ id: 'Obrigatorio', label: translate('obrigatorio'), width: '15%' },
		{ id: 'EspecificidadeEstado', label: translate('especificidadeEstado'), width: '10%' },
		{ id: 'Acoes', label: translate('acoes'), width: '5%', colSpan: 2, align: 'center' }
	];

	const columnsVisaoGeral = [
		// { id: 'TipoDocumento', label: translate('tipoDocumento'), width: '25%' },
		// { id: 'Funcionalidade', label: translate('funcionalidade'), width: '40%' },
		// { id: 'Estado', label: translate('estado'), width: '20%' },
		{ id: 'Padrao', label: translate('tipoDocumento'), width: '80%' },
		{ id: 'Validade', label: translate('validadeMeses'), width: '10%' },
		{ id: 'Obrigatorio', label: translate('obrigatorio'), width: '10%' }
	];

	const ITEM_HEIGHT = 48;
	const ITEM_PADDING_TOP = '1px';
	const MenuProps = {
		PaperProps: {
			style: {
				maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
				width: '30%'
			}
		}
	};

	// Interações com a Tabela

	let variantTableRow = theme.palette.table.tableRowSecondary;

	return (
		<LayoutContent>
			<Confirm
				open={openConfirmAlterar}
				handleClose={() => setOpenConfirmAlterar(false)}
				handleSuccess={update}
				title={translate('confirmacao')}
				text={translate('desejaRealmenteAlterarTipoDocumento')}
				textButtonSuccess={translate('sim')}
				textButtonCancel={translate('nao')}
				backgroundColorButtonCancel={theme.palette.secondary.main}
			/>
			<Confirm
				open={openConfirmCancelar}
				handleClose={() => setOpenConfirmBotaoCancelar(false)}
				handleSuccess={voltar}
				title={translate('confirmacao')}
				text={translate('desejaRealmenteCancelar')}
				textButtonSuccess={translate('sim')}
				textButtonCancel={translate('nao')}
				backgroundColorButtonCancel={theme.palette.secondary.main}
			/>
			<Form onSubmit={handleSubmit}>
				<Tabs
					value={tab}
					onChange={(event, newValue) => alterTab(newValue)}
					indicatorColor='primary'
					textcolor='primary'
				>
					<Tab label={translate('cadastro')} />
					<Tab label={translate('visaoGeral')} />
				</Tabs>
				{tab === 0 && (
					<BoxContentTab>
						<Box display='flex' flexDirection='row'>
							<Box width='50%' paddingRight={`@/..{theme.spacing(1)}px`}>
								<FormInput
									label={`@/..{translate('nome')}:`}
									name={nome}
									required
									error={checkError(submitCount, metadataNome)}
								/>
							</Box>
							<Box width='50%'>
								<FormInput
									label={`@/..{translate('helpDoc')}:`}
									name={helpDoc}
									required
									error={checkError(submitCount, metadataHelpDoc)}
									margin={`0px 0px 0px @/..{theme.spacing(1)}px`}
								/>
							</Box>
						</Box>

						<Box display='flex' flexDirection='row'>
							<Box width='25%' paddingRight={`@/..{theme.spacing(1)}px`}>
								<Typography display='block' variant='h6'>
									{`@/..{translate('tiposArquivo')}`}
								</Typography>
								<Select
									style={{ width: '100%' }}
									multiple
									value={tiposArquivosPermitidosList}
									onChange={addTipoArq}
									input={<OutlinedInput id='select-multiple-checkbox' margin='dense' />}
									renderValue={selected => selected.join(', ')}
									MenuProps={MenuProps}
								>
									{selectTiposArquivos.map(item => item.label).map(item => (
										<MenuItem key={item} value={item}>
											<Checkbox checked={tiposArquivosPermitidosList.indexOf(item) > -1} />
											<ListItemText primary={item} />
										</MenuItem>
									))}
								</Select>
							</Box>
							<Box width='20%' paddingRight={`@/..{theme.spacing(1)}px`}>
								<FormInput
									label={`@/..{translate('tamanhoMaximo')}:`}
									name={tamanhoMaximo}
									type='number'
									InputProps={{ inputProps: { min: 1 } }}
								/>
							</Box>
							<Box width='20%' paddingRight={`@/..{theme.spacing(1)}px`}>
								<FormInput
									label={`@/..{translate('quantidadeMaxima')}:`}
									name={qtdMaxima}
									type='number'
									InputProps={{ inputProps: { min: 0 } }}
								/>
							</Box>
							<Box width='20%' display='flex' flexDirection='row'>
								<Box width='80%' paddingRight={`@/..{theme.spacing(1)}px`}>
									<FormInput
										label={`@/..{translate('validadeMeses')}:`}
										name={validadeMeses}
										type='number'
										InputProps={{ inputProps: { min: 0 } }}
									/>
								</Box>

								<Box
									width='20%'
									paddingRight={`@/..{theme.spacing(1)}px`}
									alignItems='center'
									display='flex'
									flexDirection='row'
									textAlign='center'
								/>
							</Box>
							<Box display='flex' flexDirection='row'>
								<Switch
									label='Status:'
									onChange={() => {
										setStatus(!status);
									}}
									checked={status}
									checkedName={status ? 'Ativo' : 'Inativo'}
								/>
							</Box>
						</Box>

						<hr />
						<Box>
							<Box display='flex' justifyContent='flex-start' align-items='flex-end'>
								<Button
									text={translate('associarTipoDocumento')}
									onClick={() => {
										setFieldValue('flagAddRelacionamento', true);
										setFieldValue('obrigatorioRelac', obrigatorio.value);
										setRelacionamentoListField([]);
										setFieldValue('validadeMesesRelac', null);
									}}
								/>
							</Box>
							<Table small>
								<TableHead columns={columnsRelac} rowCount={columnsRelac.length} />
								<TableBody>
									{funcionalidadesTipoDoc.map((item, index) => {
										variantTableRow =
											variantTableRow === theme.palette.table.tableRowPrimary
												? theme.palette.table.tableRowSecondary
												: theme.palette.table.tableRowPrimary;

										return (
											<TableRow key={index} backgroundColor={variantTableRow}>
												<TableCell
													label={
														selectRelac.find(relac => relac.internalName === item.Funcionalidade)
															.label
													}
												/>
												<TableCell label={item.ValidadeMeses} />
												<TableCell label={item.Obrigatorio ? translate('sim') : translate('nao')} />
												<TableCell
													title={translate('editar')}
													label={
														<Box textAlign='center'>
															<IconButton
																size='small'
																onClick={() => editValidadeEstadoList(item.Funcionalidade)}
															>
																<Add />
															</IconButton>
														</Box>
													}
												/>
												<TableCell
													title={translate('excluir')}
													label={
														<IconButton
															size='small'
															onClick={() => removeFuncDoc(item.Funcionalidade)}
														>
															<Delete />
														</IconButton>
													}
												/>
											</TableRow>
										);
									})}
									{funcionalidadesTipoDoc.length === 0 && (
										<TableRow backgroundColor={variantTableRow}>
											<TableCell
												align='center'
												colSpan={6}
												label={translate('semResultadosAExibir')}
											/>
										</TableRow>
									)}
								</TableBody>
							</Table>
						</Box>
						<Box display='flex' width='60%' height='30%' flexDirection='row' alignItems='center'>
							<Modal
								open={flagAddRelacionamento.value}
								handleClose={() => setFieldValue('flagAddRelacionamento', false)}
								title={translate('relacionarFunc')}
								textButton={translate('adicionar')}
								onClickButton={() => addFuncDoc()}
								maxWidth='md'
								fullWidth
							>
								<Box display='flex' flexDirection='row'>
									<Box width='65%' paddingRight={`@/..{theme.spacing(1)}px`}>
										<Typography display='block' variant='h6'>
											{`@/..{translate('ondeExigir')}`}
										</Typography>
										<Select
											style={{ width: '100%' }}
											multiple
											value={relacionamentoListField}
											onChange={addRelac}
											input={<OutlinedInput id='select-multiple-checkbox' margin='dense' />}
											renderValue={selected =>
												selected
													.map(s => selectRelac.find(relac => relac.internalName === s).label)
													.join(',')}
											MenuProps={MenuProps}
										>
											{parents.map(itemParent => (
												<Box>
													<Typography display='block' variant='h6'>
														{itemParent}
													</Typography>
													{selectRelac
														.filter(ip => ip.parent === itemParent)
														.map(il => il.internalName)
														.map(item => (
															<Box>
																<Typography display='block' variant='h6'>
																	{item.parent}
																</Typography>
																<MenuItem key={item.value} value={item.value}>
																	<Checkbox checked={relacionamentoListField.indexOf(item) > -1} />
																	<ListItemText
																		primary={
																			selectRelac.find(relac => relac.internalName === item).label
																		}
																	/>
																</MenuItem>
															</Box>
														))}
												</Box>
											))}
										</Select>
									</Box>
									<Box width='30%' paddingRight={`@/..{theme.spacing(1)}px`}>
										<FormInput
											label={`@/..{translate('validadeMeses')}:`}
											name={validadeMesesRelac}
											type='number'
											InputProps={{ inputProps: { min: 0 } }}
										/>
									</Box>
									<Box display='flex' flexDirection='row'>
										<Typography display='block' variant='h6'>
											{`@/..{translate('obrigatorio')}`}
											<Checkbox
												display='block'
												onChange={() => handleChangeCheck(obrigatorioRelac)}
												checked={obrigatorioRelac.value}
												value={obrigatorioRelac}
											/>
										</Typography>
									</Box>
								</Box>
							</Modal>
						</Box>
						<Modal
							open={FlagAddValidadeEstado.value}
							handleClose={() => setFieldValue('flagAddValidadeEstado', false)}
							title={translate('adicionarEspecificidadeEstado')}
							textButton={translate('atualizar')}
							onClickButton={() => updateValidadeDocumentoListFuncionalidade()}
							maxWidth='sm'
							fullWidth
						>
							<Box display='flex' flexDirection='row' justifyContent='space-between'>
								<Box width='90%' display='flex' flexDirection='row'>
									<Box width='45%' paddingRight={`@/..{theme.spacing(1)}px`}>
										<FormSelect
											label={`@/..{translate('Estado')}`}
											labelInitialItem={`@/..{translate('selecioneOpcao')}`}
											items={estadoList}
											value={estado.value}
											disabled={flagValidadeDocumentoEstadoEditId != null}
											onChange={event => {
												setFieldValue('estado', event.target.value);
											}}
										/>
									</Box>
									<Box width='35%' paddingRight={`@/..{theme.spacing(1)}px`}>
										<FormInput
											label={`@/..{translate('validadeMeses')}:`}
											name={validadeMesesEstado}
											type='number'
											InputProps={{ inputProps: { min: 0 } }}
										/>
									</Box>
									<Box
										width='10%'
										paddingRight={`@/..{theme.spacing(1)}px`}
										alignItems='center'
										display='flex'
										flexDirection='row'
										textAlign='center'
									>
										<Typography display='block' variant='h6'>
											{`@/..{translate('obrigatorio')}`}
											<Checkbox
												display='block'
												onChange={() => handleChangeCheck(obrigatorioEstado)}
												checked={obrigatorioEstado.value}
												value={obrigatorioEstado.value}
											/>
										</Typography>
									</Box>
								</Box>
								<Box width='10%' paddingTop={`@/..{theme.spacing(3)}px`}>
									<IconButton
										style={{ backgroundColor: '#336666', color: 'white' }}
										size='small'
										onClick={() => addValidadeDocumentoEstado()}
									>
										<Add />
									</IconButton>
								</Box>
							</Box>
							<Table small>
								<TableHead columns={columnsEstados} rowCount={columnsEstados.length} />
								<TableBody>
									{validadeDocumentoEstadoList.map((item, index) => {
										variantTableRow =
											variantTableRow === theme.palette.table.tableRowPrimary
												? theme.palette.table.tableRowSecondary
												: theme.palette.table.tableRowPrimary;

										return (
											<TableRow key={index} backgroundColor={variantTableRow}>
												<TableCell
													label={_.find(estadoList, e => e.value === item.EstadoId).label}
												/>
												<TableCell
													label={<p style={{ textAlign: 'center' }}>{item.ValidadeMeses}</p>}
												/>
												<TableCell
													label={
														<p style={{ textAlign: 'center' }}>
															{item.Obrigatorio ? translate('sim') : translate('nao')}
														</p>
													}
													style={{ textAlign: 'center' }}
												/>
												<TableCell
													title={translate('editar')}
													label={
														<IconButton
															size='small'
															onClick={() => editarValidadeDocumentoEstado(item)}
														>
															<Edit />
														</IconButton>
													}
												/>
												<TableCell
													title={translate('excluir')}
													label={
														<IconButton
															size='small'
															onClick={() => removeValidadeDocumentoEstado(item.EstadoId)}
														>
															<Delete />
														</IconButton>
													}
												/>
											</TableRow>
										);
									})}
									{validadeDocumentoEstadoList.length === 0 && (
										<TableRow backgroundColor={variantTableRow}>
											<TableCell
												align='center'
												colSpan={6}
												label={translate('semResultadosAExibir')}
											/>
										</TableRow>
									)}
								</TableBody>
							</Table>
						</Modal>
					</BoxContentTab>
				)}
				{tab === 1 && (
					<BoxContentTab>
						<Table small>
							<TableHead columns={columnsVisaoGeral} rowCount={columnsVisaoGeral.length} />
							<TableBody>
								{getVisaoGeral().map((item, index) => {
									variantTableRow =
										variantTableRow === theme.palette.table.tableRowPrimary
											? '#dfe3e8b8'
											: theme.palette.table.tableRowPrimary;

									return (
										<TableRow key={index} backgroundColor={variantTableRow}>
											<TableCell
												label={
													<p style={{ paddingLeft: item.Class, fontWeight: 500 - item.Class }}>
														{item.Padrao}
													</p>
												}
											/>
											<TableCell
												label={
													<p style={{ textAlign: 'center', fontWeight: 500 - item.Class }}>
														{item.ValidadeMeses}
													</p>
												}
											/>
											<TableCell
												label={
													<p style={{ textAlign: 'center', fontWeight: 500 - item.Class }}>
														{item.Obrigatorio ? translate('sim') : translate('nao')}
													</p>
												}
											/>
										</TableRow>
									);
								})}
								{getVisaoGeral().length === 0 && (
									<TableRow backgroundColor={variantTableRow}>
										<TableCell
											align='center'
											colSpan={6}
											label={translate('semResultadosAExibir')}
										/>
									</TableRow>
								)}
							</TableBody>
						</Table>
					</BoxContentTab>
				)}
				<Box display='flex' justifyContent='flex-end'>
					<Button
						text='Cancelar'
						backgroundColor={theme.palette.secondary.main}
						onClick={botaoCancelar}
					/>
					<Button
						text={id ? translate('atualizar') : translate('salvar')}
						type='submit'
						margin={`0px 0px 0px @/..{theme.spacing(1)}px`}
					/>
				</Box>
			</Form>
		</LayoutContent>
	);
}
