/* eslint-disable import/no-unresolved */
import React, { useState, useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { Box, IconButton } from '@material-ui/core';
import { FormSelect, FormInput, Modal, Button, FormSelectWithSearch } from 'react-axxiom';
import { Clear } from '@material-ui/icons';
import { Creators as LoaderCreators } from '@/store/ducks/loader';
import { translate } from '@/locales';
import theme from '@/theme';
import EstadoService from '@/services/estado';
import MunicipioService from '@/services/municipio';
import { ROWSPERPAGE, SITUACAO_FORNECEDOR } from '@/utils/constants';
import { useFormik } from 'formik';
import { cnpjMask, soNumero, cpfCnpjMask } from '@/utils/mascaras';

export function FiltroAnalise({ openFiltro, setOpenFiltro, setFiltro }) {
	const dispatch = useDispatch();

	const [
		estadoList,
		setEstadoList
	] = useState([]);

	const [
		grupoFornecimentoList,
		setGrupoFornecimentoList
	] = useState([]);

	const [
		municipioList,
		setMunicipioList
	] = useState([]);

	const [
		key,
		setKey
	] = useState(0);

	// // Efeito Inicial

	useEffect(() => {
		setOpenFiltro(false);
		//carregarDados();
		return () => {
			//resetForm();
			setEstadoList([]);
		};
	}, []);

	const carregarDados = async () => {
		await estadoFindAll();
		await carregaGrupos();
	};

	const carregaGrupos = async () => {
		dispatch(LoaderCreators.setLoading());
		const response = null;
		if (response.data) {
			setGrupoFornecimentoList(response.data.GrupoCategoria_list);
			setKey(key + 1);
			dispatch(LoaderCreators.disableLoading());
		} else {
			dispatch(LoaderCreators.disableLoading());
		}
	};

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

	const municipioFindByEstado = async estadoId => {
		dispatch(LoaderCreators.setLoading());
		const response = await MunicipioService.findByEstado(estadoId);
		if (response.data) {
			setMunicipioList(response.data.Municipio_list);
			setFieldValue('municipio', municipio.value);
			dispatch(LoaderCreators.disableLoading());
		} else {
			dispatch(LoaderCreators.disableLoading());
		}
	};

	const initialValues = {
		nomeEmpresa: '',
		cnpj: '',
		codigoSap: '',
		situacaoFornecedor: 0,
		estado: 0,
		municipio: 0,
		grupoFornecimento: null,
		cpfCnpjSocio: ''
	};

	const {
		getFieldProps,
		isValid,
		values,
		handleSubmit,
		submitCount,
		setFieldValue,
		resetForm,
		setValues
	} = useFormik({
		initialValues,
		onSubmit: () => filtrar()
	});

	const [
		nomeEmpresa,
		metadataNomeEmpresa
	] = getFieldProps('nomeEmpresa', 'text');

	const [
		cnpj,
		metadataCnpj
	] = getFieldProps('cnpj', 'number');

	const [
		codigoSap,
		metadataCodigoSap
	] = getFieldProps('codigoSap', 'text');

	const [
		situacaoFornecedor,
		metadataSituacaoFornecedor
	] = getFieldProps('situacaoFornecedor', 'number');

	const [
		estado,
		metadataEstado
	] = getFieldProps('estado', 'number');

	const [
		municipio,
		metadataMunicipio
	] = getFieldProps('municipio', 'number');

	const [
		grupoFornecimento,
		metadataGrupoFornecimento
	] = getFieldProps('grupoFornecimento', 'number');

	const [
		cpfCnpjSocio,
		metadataCpfCnpjSocio
	] = getFieldProps('cpfCnpjSocio', 'text');

	const filtrar = () => {
		setFiltro(values);
		setOpenFiltro(false);
	};

	useEffect(
		() => {
			if (estado.value !== 0) {
				municipioFindByEstado(estado.value);
			}
			return () => {};
		},
		[
			estado.value
		]
	);

	return (
		<Modal
			open={openFiltro}
			handleClose={() => setOpenFiltro(false)}
			title={translate('filtroPesquisa')}
			textButton={translate('filtrar')}
			onClickButton={() => filtrar()}
			maxWidth='md'
			fullWidth
		>
			<Box
				width='100%'
				display='flex'
				justify='space-between'
				paddingBottom={`${theme.spacing(1)}px`}
			>
				<Box width='33%' padding={`${theme.spacing(1)}px`}>
					<FormInput
						label={`${translate('nomeEmpresa')}:`}
						value={nomeEmpresa.value}
						onChange={event => setFieldValue('nomeEmpresa', event.target.value)}
						placeholder={translate('nomeEmpresa')}
						// error={checkError(submitCount, metadataNomeEmpresa)}
					/>
				</Box>
				<Box width='33%' padding={`${theme.spacing(1)}px`}>
					<FormInput
						label={`${translate('cnpj')}:`}
						value={cnpjMask(cnpj.value)}
						onChange={event => setFieldValue('cnpj', soNumero(event.target.value))}
						placeholder={translate('cnpj')}
						// error={checkError(submitCount, metadataCnpj)}
					/>
				</Box>
				<Box width='33%' padding={`${theme.spacing(1)}px`}>
					<FormInput
						label={`${translate('cpfcnpjSocio')}:`}
						value={cpfCnpjMask(cpfCnpjSocio.value)}
						onChange={event => {
							setFieldValue('cpfCnpjSocio', event.target.value);
						}}
						placeholder={translate('cpfcnpjSocio')}
					/>
				</Box>
			</Box>

			{/* <FormInput
				label={`${translate('codigoSap')}:`}
				placeholder={'Não Implementado'}
				disabled={true}
				value={codigoSap.value}
				placeholder={translate('codigoSap')}
				error={checkError(submitCount, metadataCodigoSap)}
			/> */}

			<Box
				width='100%'
				display='flex'
				justify='space-between'
				paddingBottom={`${theme.spacing(1)}px`}
			>
				<Box width='33%' padding={`${theme.spacing(1)}px`}>
					<FormSelect
						label={`${translate('situacao')}`}
						labelInitialItem={`${translate('selecioneOpcao')}`}
						items={SITUACAO_FORNECEDOR}
						value={situacaoFornecedor.value}
						onChange={event => {
							setFieldValue('situacaoFornecedor', event.target.value);
						}}
					/>
				</Box>
				<Box width='33%' padding={`${theme.spacing(1)}px`}>
					<FormSelect
						label={`${translate('estado')}`}
						labelInitialItem={`${translate('selecioneOpcao')}`}
						items={estadoList}
						value={estado.value}
						onChange={event => {
							setFieldValue('municipio', 0);
							setFieldValue('estado', event.target.value);
						}}
					/>
				</Box>
				<Box width='33%' padding={`${theme.spacing(1)}px`}>
					<FormSelect
						label={`${translate('municipio')}`}
						labelInitialItem={`${translate('selecioneOpcao')}`}
						items={municipioList}
						value={municipio.value}
						onChange={event => {
							setFieldValue('municipio', event.target.value);
						}}
					/>
				</Box>
			</Box>

			<Box
				width='100%'
				// display='flex'
				// justify='space-between'
				paddingBottom={`${theme.spacing(1)}px`}
			>
				{/* <FormInput
						label={`${translate('grupoFornecimento')}:`}
						value={grupoFornecimento.value}
						placeholder={translate('informeParteDoCodigoOuDescricao')}
						onChange={event => {
							setFieldValue('grupoFornecimento', event.target.value);
						}}
					/> */}

				<FormSelectWithSearch
					placeholder={translate('informeParteDoCodigoOuDescricao')}
					label={`${translate('grupoFornecimento')}:`}
					options={grupoFornecimentoList}
					value={grupoFornecimento.value}
					onChange={(event, grupoFornecimentoSelected) => {
						if (grupoFornecimentoSelected) {
							setFieldValue('grupoFornecimento', grupoFornecimentoSelected);
						}
					}}
					// key={key}
					// error={checkError(submitCount, metadataGrupoFornecimento)}
				/>
			</Box>
			<Box display='flex' justifyContent='flex-end' padding='5px' style={{ float: 'left' }}>
				<Button
					text={translate('limpar')}
					backgroundColor={theme.palette.secondary.main}
					onClick={() => resetForm()}
				/>
			</Box>
		</Modal>
	);
}
