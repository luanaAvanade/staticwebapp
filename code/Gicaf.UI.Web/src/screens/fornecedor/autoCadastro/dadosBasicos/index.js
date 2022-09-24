import React, { useState, Fragment, useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { useFormik } from 'formik';
import * as Yup from 'yup';
import _ from 'lodash';
import useReactRouter from 'use-react-router';
import {
	Box,
	FormControlLabel,
	Radio,
	RadioGroup,
	CardContent,
	Checkbox,
	FormHelperText,
	Typography
} from '@material-ui/core';
import { Button, FormSelect, Card, Confirm, Modal } from 'react-axxiom';
import { Form } from 'formik';
import { useSnackbar } from 'notistack';
import { Creators as LoaderCreators } from '@/store/ducks/loader';
import EmpresaService from '@/services/empresa';
import theme from '@/theme';
import { translate, translateWithHtml } from '@/locales';
import { snackSuccess, snackError, snackWarning } from '@/utils/snack';
import { cnpjIsValid, temQuatorze } from '@/utils/cnpj';
import {
	TIPO_EMPRESA,
	PRINCIPAL,
	MEI,
	PESSOAJURIDICA,
	MASCULINO,
	SUBDIRETORIO_LINK,
	COMANDO_CADASTRO_FORNECEDOR,
	CADASTRO_DESCENTRALIZADO,
	CADASTRO_CENTRALIZADO,
	TAB_DADOS_BASICOS,
	CADASTRO_CRIADO
} from '@/utils/constants';
import { checkError, emailIsValid, dateIsValid } from '@/utils/validation';
import ContatoCliente from './contatoCliente';
import DadosGerais from './dadosGerais';
import DadosPessoaFisica from './dadosPessoaFisica';
import DadosEndereco from './dadosEndereco';
import DadosAcessoUsuario from './dadosAcessoUsuario';
import DadosContatosAdicionais from './dadosContatosAdicionais';
import { temOnze, cpfIsValid } from '@/utils/cpf';
import EstadoService from '@/services/estado';
import UsuarioService from '@/services/usuario';
import paths from '@/utils/paths';
import { Mensagem } from './style';
import ObjectHelper from '@/utils/objectHelper';
import { getDisableEdit, getStatusItem } from '../aprovacao/util';

export default function DadosBasicos({
	empresa,
	empresaFindById,
	itensAnalise,
	setItensAnalise,
	comentarios,
	setComentarios,
	getComentarios,
	setDadosBasicosIsValid,
	getAnaliseCadastro,
	preCadastro,
	listTermosAceiteEmpresa,
	setListTermosAceiteEmpresa,
	erroTermoAceite,
	setErroTermoAceite,
	getTermoNaoAceitos,
	user,
	acao,
	tab,
	setAcao,
	setChanged
}) {
	const dispatch = useDispatch();
	const { history } = useReactRouter();
	const { enqueueSnackbar, closeSnackbar } = useSnackbar();

	// Estado Local

	const [
		messageValidateEmail,
		setMessageValidateEmail
	] = useState(translate('campoObrigatorio'));

	const [
		messageValidateCNPJ,
		setMessageValidateCNPJ
	] = useState(translate('campoObrigatorio'));

	const [
		messageValidateData,
		setMessageValidateData
	] = useState(translate('campoObrigatorio'));

	const [
		messageValidateCPF,
		setMessageValidateCPF
	] = useState(translate('campoObrigatorio'));

	const [
		messageValidateTermoAceite,
		setMessageValidaTermoAceite
	] = useState(translate('termoAceiteObrigatorio'));

	const [
		key,
		setKey
	] = useState(0);

	const [
		countLembrete,
		setCountLembrete
	] = useState(0);

	const [
		estadoList,
		setEstadoList
	] = useState([]);

	// Efeito Inicial

	useEffect(() => {
		if (preCadastro) {
			estadoFindAll();
		}

		if (empresa) {
			setDadosEmpresa(empresa);
		}
		return () => {
			setEstadoList([]);
		};
	}, []);

	//Efeitos
	useEffect(
		() => {
			if (tab === TAB_DADOS_BASICOS) {
				estadoFindAll();
			}
		},
		[
			tab
		]
	);
	useEffect(
		() => {
			if (acao && acao !== null && (tab === TAB_DADOS_BASICOS || preCadastro)) {
				switch (acao) {
					case COMANDO_CADASTRO_FORNECEDOR.criarCadastro:
					case COMANDO_CADASTRO_FORNECEDOR.enviarCadastro:
					case COMANDO_CADASTRO_FORNECEDOR.reprovarCadastro:
					case COMANDO_CADASTRO_FORNECEDOR.aprovarCadastro:
					case COMANDO_CADASTRO_FORNECEDOR.aprovarRessalvasCadastro:
						handleSubmit();
						break;
					case COMANDO_CADASTRO_FORNECEDOR.salvar:
						update();
						break;
					case COMANDO_CADASTRO_FORNECEDOR.limparFormulario:
						limparFormulario();
						break;
				}
				setAcao(null);
			}
			if (acao && acao === COMANDO_CADASTRO_FORNECEDOR.validar && !isValid) {
				handleSubmit();
			}
			if (acao === COMANDO_CADASTRO_FORNECEDOR.descartarAlteracoes) {
				setDadosEmpresa(empresa);
				setDadosBasicosIsValid(isValid);
			}
		},
		[
			acao
		]
	);

	// Buscar Dados
	const estadoFindAll = async () => {
		dispatch(LoaderCreators.setLoading());
		const response = await EstadoService.findAll();
		if (response.data) {
			setEstadoList(response.data.Estado_list);
			dispatch(LoaderCreators.disableLoading());
		} else {
			dispatch(LoaderCreators.disableLoading());
		}
	};

	// Ações da Tela

	const setDadosEmpresa = dados => {
		setValues({
			tipoCadastro: dados.TipoCadastro,
			tipoEmpresa: dados.TipoEmpresa,
			nomeContatoCemig: dados.ContatoSolicitante ? dados.ContatoSolicitante.NomeContato : '',
			emailContatoCemig: dados.ContatoSolicitante ? dados.ContatoSolicitante.Email : '',
			cnpj: dados.CNPJ,
			nomeEmpresa: dados.NomeEmpresa,
			inscricaoEstadual: dados.InscricaoEstadual,
			isentoIE: dados.IsentoIE,
			inscricaoMunicipal: dados.InscricaoMunicipal,
			optanteSimplesNacional: dados.OptanteSimplesNacional,
			dataAbertura: dados.DataAbertura ? dados.DataAbertura.split('T')[0] : dados.DataAbertura,
			atividadeEconomicaPrincipal: dados.AtividadeEconomicaPrincipalId,
			ocupacaoPrincipal: dados.OcupacaoPrincipalId,
			cep: dados.Enderecos.length > 0 ? dados.Enderecos[0].CEP : '',
			logradouro: dados.Enderecos.length > 0 ? dados.Enderecos[0].Logradouro : '',
			numero: dados.Enderecos.length > 0 ? dados.Enderecos[0].Numero : '',
			complemento: dados.Enderecos.length > 0 ? dados.Enderecos[0].Complemento : '',
			bairro: dados.Enderecos.length > 0 ? dados.Enderecos[0].Bairro : '',
			municipio: dados.Enderecos.length > 0 ? dados.Enderecos[0].Municipio.Id : '',
			estado: dados.Enderecos.length > 0 ? dados.Enderecos[0].Municipio.EstadoId : '',
			//contatosAdicionais: dados.ContatosAdicionais,
			setEndereco: dados.Enderecos.length > 0 ? dados.Enderecos[0] : '',
			cpfMei: dados.DadosPessoaFisica.CPF ? dados.DadosPessoaFisica.CPF : '',
			dataNascimento: dados.DadosPessoaFisica.DataNascimento
				? dados.DadosPessoaFisica.DataNascimento.split('T')[0]
				: dados.DadosPessoaFisica.DataNascimento,
			pisPasepNit: dados.DadosPessoaFisica.PisPasepNit ? dados.DadosPessoaFisica.PisPasepNit : '',
			sexo: dados.DadosPessoaFisica.Sexo
				? dados.DadosPessoaFisica.Sexo === MASCULINO.id ? 'M' : 'F'
				: '',
			estadoNascimento: dados.DadosPessoaFisica.Municipio
				? dados.DadosPessoaFisica.Municipio.EstadoId
				: 0,
			cidadeNascimento: dados.DadosPessoaFisica.Municipio
				? dados.DadosPessoaFisica.Municipio.Id
				: 0,
			cpf: dados.Usuarios[0].CPF,
			nomeUsuario: dados.Usuarios[0].Nome,
			telefone: dados.Usuarios[0].Telefone,
			celular: dados.Usuarios[0].Celular,
			cargoEmpresa: dados.Usuarios[0].CargoEmpresa,
			email: dados.Usuarios[0].Email
		});
	};

	// Formulário

	const initialValues = {
		tipoCadastro: '',
		tipoEmpresa: 0,
		emailContatoCemig: '',
		nomeContatoCemig: '',
		cnpj: '',
		nomeEmpresa: '',
		inscricaoEstadual: '',
		isentoIE: false,
		inscricaoMunicipal: '',
		optanteSimplesNacional: 0,
		dataAbertura: '',
		atividadeEconomicaPrincipal: '',
		ocupacaoPrincipal: '',
		cep: '',
		logradouro: '',
		numero: '',
		complemento: '',
		bairro: '',
		municipio: 0,
		estado: 0,
		nomeUsuario: '',
		cpf: '',
		telefone: '',
		celular: '',
		cargoEmpresa: '',
		email: '',
		confirmarEmail: '',
		senha: '',
		confirmarSenha: '',
		//contatosAdicionais: [],
		dadosPessoaFisica: {},
		aceitoCondicoes: false,
		dataNascimento: '',
		cpfMei: '',
		pisPasepNit: '',
		estadoNascimento: 0,
		cidadeNascimento: 0,
		sexo: 0,
		contatoSolicitante: {}
	};

	const testeCNPJ = value => {
		if (!value) {
			setMessageValidateCNPJ(translate('campoObrigatorio'));
			return false;
		}
		if (!temQuatorze(value)) {
			setMessageValidateCNPJ(translate('cnpjInvalido'));
			return false;
		}
		if (!cnpjIsValid(value)) {
			setMessageValidateCNPJ(translate('cnpjInvalido'));
			return false;
		}
		return true;
	};

	const testeCPF = value => {
		if (!value) {
			setMessageValidateCPF(translate('campoObrigatorio'));
			return false;
		}
		if (!temOnze(value)) {
			setMessageValidateCPF(translate('cpfInvalido'));
			return false;
		}
		if (!cpfIsValid(value)) {
			setMessageValidateCPF(translate('cpfInvalido'));
			return false;
		}
		return true;
	};

	const testEmailContatoCemig = value => {
		if (tipoCadastro.value === CADASTRO_DESCENTRALIZADO.codigo) {
			if (!value) {
				setMessageValidateEmail(translate('campoObrigatorio'));
				return false;
			}
			if (!emailIsValid(value)) {
				setMessageValidateEmail(translate('emailInvalido'));
				return false;
			}
			return true;
		}
		return true;
	};

	const testNomeContatoCemig = value => {
		if (tipoCadastro.value === CADASTRO_DESCENTRALIZADO.codigo) {
			if (!value) {
				setMessageValidateEmail(translate('campoObrigatorio'));

				return false;
			}
			return true;
		}
		return true;
	};

	const testSelectRequired = value => {
		return value !== '0';
	};

	const testCheckedRequired = value => {
		return value !== 'false';
	};

	const testSelectRequiredMei = value => {
		if (tipoEmpresa.value === MEI.id) {
			if (value === '0') {
				return false;
			}
			return true;
		}
		return true;
	};

	const testIncricaoEstadual = value => {
		if (!isentoIE) {
			if (!value) {
				setMessageValidateEmail(translate('campoObrigatorio'));
				return false;
			}
			return true;
		}
		return true;
	};

	const testData = value => {
		if (!value) {
			setMessageValidateData(translate('campoObrigatorio'));
			return false;
		}
		if (!dateIsValid(value)) {
			setMessageValidateData(translate('dataInvalida'));
			return false;
		}
		return true;
	};

	const testTipoEmpresaMei = value => {
		if (tipoEmpresa.value === MEI.id) {
			if (!value) {
				return false;
			}
			return true;
		}
		return true;
	};

	const testDataNascimento = value => {
		if (tipoEmpresa.value === MEI.id) {
			return testData(value);
		}
		return true;
	};

	const testCpfMei = value => {
		if (tipoEmpresa.value === MEI.id) {
			return testeCPF(value);
		}
		return true;
	};

	const testCpfUsuario = value => {
		if (empresa) {
			return true;
		}
		return testeCPF(value);
	};

	const testRequiredPreCadastro = value => {
		if (empresa) {
			return true;
		}
		if (!value) {
			return false;
		}
		return true;
	};

	const testSenha = value => {
		if (empresa) return true;
		if (!value) return false;
		if (value.length < 8) return false;
		if (value.match(/[A-Z]/).length === 0) return false;
		if (value.match(/[^0-9a-zA-Z]/).length === 0) return false;

		return true;
	};

	const testAtividadeEconomicaPrincipal = value => {
		if (tipoEmpresa.value === PESSOAJURIDICA.id) {
			return value;
		}
		return true;
	};

	const testOcupacaoPrincipal = value => {
		if (tipoEmpresa.value === MEI.id) {
			return value;
		}
		return true;
	};

	const validationSchema = Yup.object().shape({
		tipoEmpresa: Yup.string().test('tipoEmpresa', translate('campoObrigatorio'), value =>
			testSelectRequired(value)
		),
		tipoCadastro: Yup.string().required(translate('campoObrigatorio')),
		// emailContatoCemig: Yup.string()
		// 	.nullable()
		// 	.test('emailContatoCemigRequired', messageValidateEmail, value =>
		// 		testEmailContatoCemig(value)
		// 	),
		// nomeContatoCemig: Yup.string()
		// 	.nullable()
		// 	.test('nomeContatoCemig', translate('campoObrigatorio'), value =>
		// 		testNomeContatoCemig(value)
		// 	),

		cnpj: Yup.string().test('CNPJ', messageValidateCNPJ, value => testeCNPJ(value)),
		nomeEmpresa: Yup.string().required(translate('campoObrigatorio')),
		inscricaoEstadual: Yup.string().test(
			'inscricaoEstadual',
			translate('campoObrigatorio'),
			value => testIncricaoEstadual(value)
		),
		// optanteSimplesNacional: Yup.string().test(
		// 	'optanteSimplesNacional',
		// 	translate('campoObrigatorio'),
		// 	value => testSelectRequired(value)
		// ),
		dataAbertura: Yup.string().test('dataAbertura', messageValidateData, value => testData(value)),
		// atividadeEconomicaPrincipal: Yup.string()
		// 	.nullable()
		// 	.test('atividadeEconomicaPrincipal', translate('campoObrigatorio'), value =>
		// 		testAtividadeEconomicaPrincipal(value)
		// 	),
		// ocupacaoPrincipal: Yup.string().test(
		// 	'ocupacaoPrincipal',
		// 	translate('campoObrigatorio'),
		// 	value => testOcupacaoPrincipal(value)
		// ),
		cep: Yup.string().required(translate('campoObrigatorio')),
		logradouro: Yup.string().required(translate('campoObrigatorio')),
		numero: Yup.string().required(translate('campoObrigatorio')),
		bairro: Yup.string().required(translate('campoObrigatorio')),
		estado: Yup.string().test('estado', translate('campoObrigatorio'), value =>
			testSelectRequired(value)
		),
		municipio: Yup.string().test('municipio', translate('campoObrigatorio'), value =>
			testSelectRequired(value)
		),
		// nomeUsuario: Yup.string().test('nomeUsuario', translate('campoObrigatorio'), value =>
		// 	testRequiredPreCadastro(value)
		// ),
		cpf: Yup.string().test('cpf', messageValidateCPF, value => testCpfUsuario(value)),
		telefone: Yup.string().test('telefone', translate('campoObrigatorio'), value =>
			testRequiredPreCadastro(value)
		),
		// cargoEmpresa: Yup.string().test('cargoEmpresa', translate('campoObrigatorio'), value =>
		// 	testRequiredPreCadastro(value)
		// ),
		email: Yup.string()
			.email('Email invalído.')
			.test('email', translate('campoObrigatorio'), value => testRequiredPreCadastro(value)),
		confirmarEmail: Yup.string()
			.oneOf(
				[
					Yup.ref('email')
				],
				translate('confirmacaoDeveSerIgualAoEmail')
			)
			.test('email', translate('campoObrigatorio'), value => testRequiredPreCadastro(value)),

		senha: Yup.string().test('senha', translate('validacaoSenha'), value => testSenha(value)),
		confirmarSenha: Yup.string()
			.oneOf(
				[
					Yup.ref('senha')
				],
				translate('confirmacaoDeveSerIgualASenha')
			)
			.test('confirmacaosenha', translate('campoObrigatorio'), value =>
				testRequiredPreCadastro(value)
			),
		// contatosAdicionais: Yup.mixed().test(
		// 	'contatosAdicionais',
		// 	translate('adicionePeloMenosUmContato'),
		// 	value => value.length > 0
		// ),
		// aceitoCondicoes: Yup.string().test('aceitoCondicoes', translate('campoObrigatorio'), value =>
		// 	testCheckedRequired(value)
		// ),
		// dataNascimento: Yup.string()
		// 	.nullable()
		// 	.test('dataNascimento', messageValidateData, value => testDataNascimento(value)),
		// cpfMei: Yup.string().test('cpf', messageValidateCPF, value => testCpfMei(value)),
		// pisPasepNit: Yup.string().test('pisPasepNit', translate('campoObrigatorio'), value =>
		// 	testTipoEmpresaMei(value)
		// ),
		// estadoNascimento: Yup.string().test('estadoNascimento', translate('campoObrigatorio'), value =>
		// 	testSelectRequiredMei(value)
		// ),
		// cidadeNascimento: Yup.string().test('cidadeNascimento', translate('campoObrigatorio'), value =>
		// 	testSelectRequiredMei(value)
		// ),
		// sexo: Yup.string().test('optanteSimplesNacional', translate('campoObrigatorio'), value =>
		// 	testSelectRequiredMei(value)
		// )
	});

	const {
		getFieldProps,
		isValid,
		values,
		handleSubmit,
		handleChange,
		submitCount,
		setFieldValue,
		setFieldTouched,
		resetForm,
		setValues,
		touched
	} = useFormik({
		initialValues,
		validationSchema,
		onSubmit: () => save()
	});

	useEffect(
		() => {
			setDadosBasicosIsValid(isValid);
		},
		[
			isValid
		]
	);

	useEffect(
		() => {
			if (!preCadastro && !_.isEmpty(touched)) {
				setChanged(true);
			}
		},
		[
			touched
		]
	);

	const {
		emailContatoCemig,
		nomeContatoCemig,
		cnpj,
		nomeEmpresa,
		isentoIE,
		inscricaoEstadual,
		inscricaoMunicipal,
		optanteSimplesNacional,
		dataAbertura,
		atividadeEconomicaPrincipal,
		ocupacaoPrincipal,
		cep,
		logradouro,
		numero,
		complemento,
		bairro,
		municipio,
		idUsuario,
		nomeUsuario,
		cpf,
		telefone,
		celular,
		cargoEmpresa,
		email,
		senha,
		confirmarSenha,
		//contatosAdicionais,
		termosAceite,
		dadosPessoaFisica,
		dataNascimento,
		cpfMei,
		pisPasepNit,
		cidadeNascimento,
		sexo
	} = values;

	const [
		tipoCadastro
	] = getFieldProps('tipoCadastro', 'text');

	const [
		tipoEmpresa,
		metadataTipoEmpresa
	] = getFieldProps('tipoEmpresa', 'text');

	const [
		aceitoCondicoes,
		metadataAceitoCondicoes
	] = getFieldProps('aceitoCondicoes', 'text');

	const setTipoEmpresa = value => {
		setFieldValue('tipoEmpresa', value);
		setFieldValue('dataNascimento', '');
		setFieldValue('cpfMei', '');
		setFieldValue('pisPasepNit', '');
		setFieldValue('cidadeNascimento', 0);
		setFieldValue('sexo', 0);
	};

	const setTipoCadastro = value => {
		setFieldValue('nomeContatoCemig', '');
		setFieldValue('emailContatoCemig', '');
		setFieldValue('tipoCadastro', value);
	};

	// Ações da Tela
	const save = () => {
		if (preCadastro) {
			create();
		} else {
			if (empresa) {
				update();
			} else {
				create();
			}
		}
	};

	const callbackMensagemSucesso = async () => {
		if (acao == COMANDO_CADASTRO_FORNECEDOR.enviarCadastro) {
			callback(translateWithHtml('fornecedorAtualizadoSucesso'));
		} else {
			if (preCadastro) {
				callback(translateWithHtml('fornecedorCadatradoSucesso'));
			} else {
				callback(translateWithHtml('cadastroSalvoComSucesso'));
			}
		}
	};

	const create = async () => {
		dispatch(LoaderCreators.setLoading());
		const response = await EmpresaService.create(getFornecedor());
		if (response.data.Empresa_insert) {
			callbackMensagemSucesso();
		} else {
			callbackError(translate('erroCadastrarfomulario'));
		}
		dispatch(LoaderCreators.disableLoading());
	};

	const update = async () => {
		dispatch(LoaderCreators.setLoading());
		try {
			const response = await EmpresaService.update(empresa.Id, getFornecedor());
			if (response.data && response.data.Empresa_update) {
				empresaFindById();
				callbackMensagemSucesso();
			} else {
				callbackError(translateWithHtml('erroInesperado'));
			}
			dispatch(LoaderCreators.disableLoading());
		} catch (error) {
			dispatch(LoaderCreators.disableLoading());
			callbackError(translateWithHtml('erroInesperado'));
		}
	};

	const getFornecedor = () => {
		const newFornecedor = {
			NomeEmpresa: nomeEmpresa,
			CNPJ: cnpj,
			InscricaoEstadual: inscricaoEstadual,
			IsentoIE: isentoIE,
			InscricaoMunicipal: inscricaoMunicipal,
			OptanteSimplesNacional: optanteSimplesNacional,
			DataAbertura: dataAbertura,
			AtividadeEconomicaPrincipalId: atividadeEconomicaPrincipal
				? atividadeEconomicaPrincipal
				: null,
			OcupacaoPrincipalId: ocupacaoPrincipal ? ocupacaoPrincipal : null,
			TipoEmpresa: tipoEmpresa.value,
			TipoCadastro: tipoCadastro.value,
			Enderecos: getEndereco(),
			//ContatosAdicionais: contatosAdicionais
		};

		if (preCadastro) {
			newFornecedor.Usuarios = getDadosAcesso();
		}

		if (!preCadastro) {
			newFornecedor.AnaliseCadastro = getAnaliseCadastro();
			newFornecedor.TermoAceiteEmpresa = getTermosAceite();
			newFornecedor.Comentarios = getComentarios();
			const url = `${document.location.origin}${SUBDIRETORIO_LINK}`;
			newFornecedor.LinkCadastro = `${url}/cadastro-complementar/${empresa.Id}`;
		}

		if (tipoEmpresa.value === MEI.id) {
			const dadosPessoaFisica = {};
			dadosPessoaFisica.DataNascimento = dataNascimento;
			dadosPessoaFisica.CPF = cpfMei;
			dadosPessoaFisica.PisPasepNit = pisPasepNit;
			dadosPessoaFisica.MunicipioId = cidadeNascimento;
			dadosPessoaFisica.Sexo = sexo;

			newFornecedor.DadosPessoaFisica = dadosPessoaFisica;
		}

		if (tipoCadastro.value === CADASTRO_DESCENTRALIZADO.codigo) {
			const contatoSolicitante = {};
			contatoSolicitante.NomeContato = nomeContatoCemig;
			contatoSolicitante.Email = emailContatoCemig;

			newFornecedor.ContatoSolicitante = contatoSolicitante;
		}

		return newFornecedor;
	};

	const getTermosAceite = () => {
		const t = ObjectHelper.clone(listTermosAceiteEmpresa);
		t.forEach(item => {
			delete item.EmpresaId;
			delete item.TermosAceite;
			delete item.TermosAceiteId;
		});
		return t;
	};

	const getEndereco = () => {
		let endereco = {
			CEP: cep,
			TipoEndereco: PRINCIPAL.codigo,
			Logradouro: logradouro,
			Numero: numero,
			Complemento: complemento,
			Bairro: bairro,
			MunicipioId: municipio
		};

		//if (empresa.Enderecos[0].Id) endereco.Id = empresa.Enderecos[0].Id;

		return [
			endereco
		];
	};
	const getDadosAcesso = () => {
		const url = `${document.location.origin}${SUBDIRETORIO_LINK}`;
		return [
			idUsuario
				? { Id: idUsuario }
				: {
						Nome: nomeUsuario,
						CPF: cpf,
						Telefone: telefone,
						Celular: celular,
						CargoEmpresa: cargoEmpresa,
						Email: email,
						PassWord: senha,
						ConfirmPassWord: confirmarSenha,
						UserName: email,
						LinkConfirmacao: `${url}/confirmacao-cadastro-fornecedor?userName={userName}&token={token}`
					}
		];
	};

	const getMensagemDadosAcesso = () => {
		if (tipoEmpresa.value === PESSOAJURIDICA.id) {
			if (tipoCadastro.value === CADASTRO_CENTRALIZADO.codigo) {
				return translate('msgPjCadastroCentralizado');
			}
			if (tipoCadastro.value === CADASTRO_DESCENTRALIZADO.codigo) {
				return translate('msgPjCadastroDescentralizado');
			}
		}
		if (tipoEmpresa.value === MEI.id) {
			if (tipoCadastro.value === CADASTRO_CENTRALIZADO.codigo) {
				return translate('msgMeiCadastroCentralizado');
			}
			if (tipoCadastro.value === CADASTRO_DESCENTRALIZADO.codigo) {
				return translate('msgMeiCadastroDescentralizado');
			}
		}
		return '';
	};

	const getMensagem = () => {
		if (tipoCadastro.value === CADASTRO_CENTRALIZADO.codigo) {
			if (tipoEmpresa.value === PESSOAJURIDICA.id) {
				return translate('msgCadastroCentralizadoPJ');
			}

			if (tipoEmpresa.value === MEI.id) {
				return translate('msgCadastroCentralizadoMEI');
			}
		}

		if (tipoCadastro.value === CADASTRO_DESCENTRALIZADO.codigo) {
			if (tipoEmpresa.value === PESSOAJURIDICA.id) {
				return translate('msgCadastroDescentralizadoMEIePJ');
			}

			if (tipoEmpresa.value === MEI.id) {
				return translate('msgCadastroDescentralizadoMEIePJ');
			}
		}

		return '';
	};
	const limparFormulario = () => {
		resetForm(initialValues);
	};

	// Ações de retorno

	const callback = mensagem => {
		enqueueSnackbar('', snackSuccess(mensagem, closeSnackbar));
		if (!empresa) {
			irParaLogin();
		}
	};

	const callbackError = mensagem => {
		enqueueSnackbar('', snackError(mensagem, closeSnackbar));
	};

	const callbackWarning = mensagem => {
		enqueueSnackbar('', snackWarning(mensagem, closeSnackbar));
	};

	const irParaLogin = () => {
		history.push(paths.getPathByCodigo('login'));
	};

	return (
		<Form id='dadosBasico' onSubmit={handleSubmit}>
			<Fragment>
				{!empresa && (
					<Box paddingTop={`${theme.spacing(1)}px`}>
						<Card>
							<CardContent>
								<Box display='flex' flexDirection='row'>
									<FormSelect
										label={`${translate('labelSelectFornecedor')}`}
										labelInitialItem={translate('selecioneOpcao')}
										items={TIPO_EMPRESA}
										value={tipoEmpresa.value}
										onChange={event => {
											resetForm();
											setTipoEmpresa(event.target.value);
										}}
										error={checkError(submitCount, metadataTipoEmpresa)}
										width='300px'
									/>
								</Box>
								<Box display='flex' flexDirection='row'>
									<RadioGroup
										value={tipoCadastro.value}
										onChange={event => {
											setTipoCadastro(event.target.value);
										}}
										row
									>
										<FormControlLabel
											value={CADASTRO_CENTRALIZADO.codigo}
											control={<Radio color='primary' />}
											label={`${translate('cadastroCentralizadoApenasLicitacoes')}`}
											labelPlacement='end'
										/>
										<FormControlLabel
											value={CADASTRO_DESCENTRALIZADO.codigo}
											control={<Radio color='primary' />}
											label={`${translate('cadastroDescentralizadoApenasUsuariosCemig')}`}
											labelPlacement='end'
										/>
									</RadioGroup>
								</Box>
								<Mensagem>{getMensagem()}</Mensagem>
							</CardContent>
						</Card>
					</Box>
				)}
				{tipoEmpresa.value !== 0 && (
					<Fragment>
						{tipoCadastro.value === CADASTRO_DESCENTRALIZADO.codigo && (
							<ContatoCliente
								formulario={{ submitCount, getFieldProps }}
								itensAnalise={itensAnalise}
								setItensAnalise={setItensAnalise}
								comentarios={comentarios}
								setComentarios={setComentarios}
								preCadastro={preCadastro}
								user={preCadastro ? null : user}
								historicoEmpresa={preCadastro ? false : empresa.Historico}
								disableEdit={
									preCadastro ? null : (
										getDisableEdit(
											user,
											empresa.AnaliseCadastro,
											getStatusItem(itensAnalise, 'Contato_Cliente')
										)
									)
								}
								statusEmpresa={
									empresa && empresa.AnaliseCadastro ? empresa.AnaliseCadastro.StatusAnalise : null
								}
							/>
						)}

						<DadosGerais
							formulario={{ submitCount, getFieldProps, setFieldValue, setFieldTouched }}
							itensAnalise={itensAnalise}
							setItensAnalise={setItensAnalise}
							comentarios={comentarios}
							setComentarios={setComentarios}
							preCadastro={preCadastro}
							user={preCadastro ? null : user}
							historicoEmpresa={preCadastro ? false : empresa.Historico}
							disableEdit={
								preCadastro ? null : (
									getDisableEdit(
										user,
										empresa.AnaliseCadastro,
										getStatusItem(itensAnalise, 'Dados_Gerais')
									)
								)
							}
							statusEmpresa={
								empresa && empresa.AnaliseCadastro ? empresa.AnaliseCadastro.StatusAnalise : null
							}
						/>

						{tipoEmpresa.value === MEI.id && (
							<DadosPessoaFisica
								estadoList={estadoList}
								formulario={{ submitCount, getFieldProps, setFieldValue, setFieldTouched }}
								itensAnalise={itensAnalise}
								setItensAnalise={setItensAnalise}
								comentarios={comentarios}
								setComentarios={setComentarios}
								preCadastro={preCadastro}
								user={preCadastro ? null : user}
								historicoEmpresa={preCadastro ? null : empresa.Historico}
								disableEdit={
									preCadastro ? (
										false
									) : (
										getDisableEdit(
											user,
											empresa.AnaliseCadastro,
											getStatusItem(itensAnalise, 'Dados_Pessoa_Fisica')
										)
									)
								}
								statusEmpresa={
									empresa && empresa.AnaliseCadastro ? empresa.AnaliseCadastro.StatusAnalise : null
								}
							/>
						)}
						<DadosEndereco
							estadoList={estadoList}
							formulario={{ submitCount, getFieldProps, setFieldValue, setFieldTouched }}
							itensAnalise={itensAnalise}
							setItensAnalise={setItensAnalise}
							comentarios={comentarios}
							setComentarios={setComentarios}
							preCadastro={preCadastro}
							user={preCadastro ? null : user}
							historicoEmpresa={preCadastro ? null : empresa.Historico}
							disableEdit={
								preCadastro ? (
									false
								) : (
									getDisableEdit(
										user,
										empresa.AnaliseCadastro,
										getStatusItem(itensAnalise, 'Dados_Endereco')
									)
								)
							}
							statusEmpresa={
								empresa && empresa.AnaliseCadastro ? empresa.AnaliseCadastro.StatusAnalise : null
							}
						/>
						<DadosAcessoUsuario
							formulario={{
								submitCount,
								getFieldProps,
								setFieldValue,
								setFieldTouched
							}}
							mensagem={getMensagemDadosAcesso()}
							itensAnalise={itensAnalise}
							setItensAnalise={setItensAnalise}
							comentarios={comentarios}
							setComentarios={setComentarios}
							preCadastro={preCadastro}
							user={preCadastro ? null : user}
							historicoEmpresa={preCadastro ? null : empresa.Historico}
							disableEdit={
								preCadastro ? (
									false
								) : (
									getDisableEdit(
										user,
										empresa.AnaliseCadastro,
										getStatusItem(itensAnalise, 'Acesso_Sistema')
									)
								)
							}
							statusEmpresa={
								empresa && empresa.AnaliseCadastro ? empresa.AnaliseCadastro.StatusAnalise : null
							}
						/>
						<DadosContatosAdicionais
							formulario={{ submitCount, getFieldProps, setFieldValue, setFieldTouched }}
							itensAnalise={itensAnalise}
							setItensAnalise={setItensAnalise}
							comentarios={comentarios}
							setComentarios={setComentarios}
							preCadastro={preCadastro}
							user={preCadastro ? null : user}
							historicoEmpresa={preCadastro ? null : empresa.Historico}
							disableEdit={
								preCadastro ? (
									false
								) : (
									getDisableEdit(
										user,
										empresa.AnaliseCadastro,
										getStatusItem(itensAnalise, 'Dados_Contatos_Adicionais')
									)
								)
							}
						/>
											<Box paddingTop={`${theme.spacing(1)}px`}>
								<Card>
									<CardContent>
										<Fragment>
											<p>
												Declaro que todas as informações aqui prestadas são verdadeiras e a
												falsificação configura crime previsto no Código Penal Brasileiro (Art.
												297-299) passível de apuração na forma da lei, bem como pode ser enquadrada
												como litigância de má fé.
											</p>
											<Box
												style={{
													marginTop: 16,
													marginBottom: 16,
													borderRadius: 5,
													padding: 8,
													width: '275px',
													border: `${checkError(submitCount, metadataAceitoCondicoes)
														? 1
														: 0}px solid red`
												}}
											>
												<FormControlLabel
													control={
														<Checkbox
															key={key}
															checked={aceitoCondicoes.value}
															value={aceitoCondicoes.value}
															onChange={event => {
																setKey(key + 1);
																setFieldValue('aceitoCondicoes', event.target.checked);
																setFieldTouched('aceitoCondicoes', true);
															}}
															preCadastro={preCadastro}
														/>
													}
													label='Li e aceito os termos e condições.'
												/>
												{checkError(submitCount, metadataAceitoCondicoes) && (
													<FormHelperText style={{ color: 'red' }}>
														{metadataAceitoCondicoes.error}
													</FormHelperText>
												)}
											</Box>
										</Fragment>
									</CardContent>
								</Card>
							</Box>
						
					</Fragment>
				)}
			</Fragment>
		</Form>
	);
}
