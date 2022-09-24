import React, { useEffect, useState } from 'react';
import { Card, FormInput, FormSelect } from 'react-axxiom';
import { CardHeader, CardContent, Box } from '@material-ui/core';
import { useDispatch } from 'react-redux';
import _ from 'lodash';
import { translate, translateWithHtml } from '@/locales';
import { Creators as LoaderCreators } from '@/store/ducks/loader';
import { cepMask, soNumero } from '@/utils/mascaras';
import { checkError } from '@/utils/validation';
import theme from '@/theme';
import MunicipioService from '@/services/municipio';
import EnderecoService from '@/services/endereco';
import { ENUM_ITEMS_ANALISE } from '@/utils/constants';
import Aprovacao from '@/screens/fornecedor/autoCadastro/aprovacao';
import { snackWarning } from '@/utils/snack';
import { useSnackbar } from 'notistack';

export default function DadosEndereco({
	formulario,
	estadoList,
	preCadastro,
	itensAnalise,
	setItensAnalise,
	comentarios,
	setComentarios,
	historicoEmpresa,
	user,
	disableEdit,
	statusEmpresa
}) {
	const dispatch = useDispatch();
	const { enqueueSnackbar, closeSnackbar } = useSnackbar();

	const [
		municipioList,
		setMunicipioList
	] = useState([]);

	const [
		ibge,
		setIbge
	] = useState(null);

	const [
		CEPOriginal,
		setCEPOriginal
	] = useState(null);

	const { submitCount, getFieldProps, setFieldValue, setFieldTouched } = formulario;

	const [
		cep,
		metadataCep
	] = getFieldProps('cep', 'text');

	const [
		logradouro,
		metadataLogradouro
	] = getFieldProps('logradouro', 'text');

	const [
		numero,
		metadataNumero
	] = getFieldProps('numero', 'text');

	const [
		complemento,
		metadataComplemento
	] = getFieldProps('complemento', 'text');

	const [
		bairro,
		metadataBairro
	] = getFieldProps('bairro', 'text');

	const [
		municipio,
		metadataMunicipio
	] = getFieldProps('municipio', 'text');

	const [
		estado,
		metadataEstado
	] = getFieldProps('estado', 'text');

	const getEstado = sigla => {
		return _.find(estadoList, e => e.Sigla === sigla);
	};

	const getMunicipio = CodIBGE => {
		return _.find(municipioList, m => m.CodIBGE.toString() === CodIBGE);
	};

	const limpaCamposEndereco = () => {
		setFieldValue('logradouro', '');
		setFieldValue('bairro', '');
		setFieldValue('estado', 0);
		setFieldValue('municipio', 0);
		setFieldValue('numero', '');
		setFieldValue('complemento', '');
	};

	const buscarEndereco = async () => {
		dispatch(LoaderCreators.setLoading(translate('buscandoDadosCorreio')));
		try {
			const response = await EnderecoService.buscarPorCep(soNumero(cep.value));
			if (response.erro == true) {
				enqueueSnackbar('', snackWarning(translateWithHtml('cepInvalido'), closeSnackbar));
				limpaCamposEndereco();
				dispatch(LoaderCreators.disableLoading());
			} else {
				limpaCamposEndereco();
				setFieldValue('logradouro', response.logradouro);
				setFieldValue('bairro', response.bairro);
				const estadoRetorno = getEstado(response.uf);
				setFieldValue('estado', estadoRetorno.value);
				setIbge(response.ibge);
				dispatch(LoaderCreators.disableLoading());
			}
		} catch (error) {
			enqueueSnackbar('', snackWarning(translateWithHtml('servicoCepIndisponivel'), closeSnackbar));
			limpaCamposEndereco();
			dispatch(LoaderCreators.disableLoading());
		}
	};

	// Efeito Inicial
	useEffect(
		() => {
			if (preCadastro && cep.value && soNumero(cep.value).length === 8) {
				//buscarEndereco();
			}

			if (!preCadastro && CEPOriginal === null) {
				//setCEPOriginal(cep.value);
			}

			if (CEPOriginal && CEPOriginal !== cep.value && soNumero(cep.value).length === 8) {
				//buscarEndereco();
			}
		},
		[
			cep.value
		]
	);

	useEffect(
		() => {
			if (estado.value > 0) {
				municipioFindByEstado(estado.value);
			}
		},
		[
			estado.value
		]
	);

	useEffect(
		() => {
			if (preCadastro && ibge && municipioList.length > 0) {
				setFieldValue('municipio', getMunicipio(ibge).value);
			}
			if (!preCadastro && ibge && municipioList.length > 0 && municipio.value === 0) {
				setFieldValue('municipio', getMunicipio(ibge).value);
			}
		},
		[
			municipioList
		]
	);

	// Busca de Dados

	const setEstado = value => {
		setFieldValue('estado', value);
	};

	const municipioFindByEstado = async estadoId => {
		dispatch(LoaderCreators.setLoading());

		const response = await MunicipioService.findByEstado(estadoId);
		if (response.data) {
			setMunicipioList(response.data.Municipio_list);
			dispatch(LoaderCreators.disableLoading());
		} else {
			dispatch(LoaderCreators.disableLoading());
		}
	};

	return (
		<Box paddingTop={`${theme.spacing(1)}px`}>
			<Card>
				<CardHeader
					title={translate('endereco')}
					action={
						!preCadastro && (
							<Aprovacao
								itensAnalise={itensAnalise}
								setItensAnalise={setItensAnalise}
								comentarios={comentarios}
								setComentarios={setComentarios}
								tipoItem={ENUM_ITEMS_ANALISE.find(x => x.internalName === 'Dados_Endereco').value}
								historicoEmpresa={historicoEmpresa}
								user={user}
								disableEdit={disableEdit}
								statusEmpresa={statusEmpresa}
							/>
						)
					}
				/>
				<CardContent>
					<Box display='flex' flexDirection='row'>
						<Box width='20%' paddingRight={`${theme.spacing(1)}px`}>
							<FormInput
								label={`${translate('cep')}:`}
								value={cepMask(cep.value)}
								onChange={event => setFieldValue('cep', event.target.value)}
								onFocus={() => setFieldTouched('cep', true)}
								error={checkError(submitCount, metadataCep)}
								required
								disabled={disableEdit}
							/>
						</Box>
						<Box width='65%' paddingRight={`${theme.spacing(1)}px`}>
							<FormInput
								label={`${translate('logradouro')}:`}
								name={logradouro}
								value={logradouro.value}
								onChange={event => setFieldValue('logradouro', event.target.value)}
								onFocus={() => setFieldTouched('logradouro', true)}
								error={checkError(submitCount, metadataLogradouro)}
								required
								disabled={disableEdit}
							/>
						</Box>
						<Box width='15%'>
							<FormInput
								label={`${translate('numero')}:`}
								value={soNumero(numero.value)}
								onChange={event => setFieldValue('numero', event.target.value)}
								onFocus={() => setFieldTouched('numero', true)}
								error={checkError(submitCount, metadataNumero)}
								required
								disabled={disableEdit}
							/>
						</Box>
					</Box>

					<Box display='flex' flexDirection='row'>
						<Box width='10%' paddingRight={`${theme.spacing(1)}px`}>
							<FormInput
								label={`${translate('complemento')}:`}
								name={complemento}
								value={complemento.value}
								onChange={event => setFieldValue('complemento', event.target.value)}
								onFocus={() => setFieldTouched('complemento', true)}
								error={checkError(submitCount, metadataComplemento)}
								disabled={disableEdit}
							/>
						</Box>
						<Box width='30%' paddingRight={`${theme.spacing(1)}px`}>
							<FormInput
								label={`${translate('bairro')}:`}
								name={bairro}
								value={bairro.value}
								onChange={event => setFieldValue('bairro', event.target.value)}
								onFocus={() => setFieldTouched('bairro', true)}
								error={checkError(submitCount, metadataBairro)}
								required
								disabled={disableEdit}
							/>
						</Box>
						<Box width='30%' paddingRight={`${theme.spacing(1)}px`}>
							<FormSelect
								disabled={estadoList.length === 0 || disableEdit}
								label={`${translate('estado')}`}
								labelInitialItem={translate('selecioneOpcao')}
								items={estadoList}
								value={estado.value}
								onChange={event => setEstado(event.target.value)}
								onFocus={() => setFieldTouched('estado', true)}
								error={checkError(submitCount, metadataEstado)}
								required
							/>
						</Box>
						<Box width='30%'>
							<FormSelect
								disabled={estado.value === 0 || disableEdit}
								label={`${translate('municipio')}:`}
								labelInitialItem={translate('selecioneOpcao')}
								items={municipioList}
								value={municipio.value}
								onChange={event => setFieldValue('municipio', event.target.value)}
								onFocus={() => setFieldTouched('municipio', true)}
								error={checkError(submitCount, metadataMunicipio)}
								required
							/>
						</Box>
					</Box>
				</CardContent>
			</Card>
		</Box>
	);
}
