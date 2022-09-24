import React, { useState, useEffect } from 'react';
import { FormInput, FormSelect, Card } from 'react-axxiom';
import { Box, CardContent, CardHeader } from '@material-ui/core';
import { useDispatch } from 'react-redux';
import { translate } from '@/locales';
import { checkError } from '@/utils/validation';
import { soNumero, cpfMask } from '@/utils/mascaras';
import { SEXO, ENUM_ITEMS_ANALISE } from '@/utils/constants';
import { Creators as LoaderCreators } from '@/store/ducks/loader';
import theme from '@/theme';
import MunicipioService from '@/services/municipio';
import Aprovacao from '@/screens/fornecedor/autoCadastro/aprovacao';

export default function DadosPessoaFisica({
	formulario,
	estadoList,
	itensAnalise,
	setItensAnalise,
	comentarios,
	setComentarios,
	preCadastro,
	historicoEmpresa,
	user,
	disableEdit,
	statusEmpresa
}) {
	const dispatch = useDispatch();

	const { submitCount, getFieldProps, setFieldValue, setFieldTouched } = formulario;

	const [
		municipioList,
		setMunicipioList
	] = useState([]);

	const [
		dataNascimento,
		metadataDataNascimento
	] = getFieldProps('dataNascimento', 'text');

	const [
		cpfMei,
		metadataCpfMei
	] = getFieldProps('cpfMei', 'text');

	const [
		pisPasepNit,
		metadataPisPasepNit
	] = getFieldProps('pisPasepNit', 'text');

	const [
		cidadeNascimento,
		metadataCidadeNascimento
	] = getFieldProps('cidadeNascimento', 'text');

	const [
		estadoNascimento,
		metadataEstadoNascimento
	] = getFieldProps('estadoNascimento', 'text');

	const [
		sexo,
		metadataSexo
	] = getFieldProps('sexo', 'text');

	// Efeito Inicial
	useEffect(
		() => {
			if (estadoNascimento.value > 0) {
				municipioFindByEstado(estadoNascimento.value);
			}
		},
		[
			estadoNascimento.value
		]
	);

	// Busca de Dados

	const setEstado = value => {
		setFieldValue('cidadeNascimento', 0);
		setFieldValue('estadoNascimento', value);
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
					title={translate('dadosPessoais')}
					action={
						!preCadastro && (
							<Aprovacao
								itensAnalise={itensAnalise}
								setItensAnalise={setItensAnalise}
								comentarios={comentarios}
								setComentarios={setComentarios}
								tipoItem={
									ENUM_ITEMS_ANALISE.find(x => x.internalName === 'Dados_Pessoa_Fisica').value
								}
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
						<Box width='33%' paddingRight={`${theme.spacing(1)}px`}>
							<FormInput
								label={`${translate('cpfMei')}:`}
								value={cpfMask(cpfMei.value)}
								onChange={event => setFieldValue('cpfMei', soNumero(event.target.value))}
								onFocus={() => setFieldTouched('cpfMei', true)}
								error={checkError(submitCount, metadataCpfMei)}
								required
								disabled={disableEdit}
							/>
						</Box>
						<Box width='33%' paddingRight={`${theme.spacing(1)}px`}>
							<FormInput
								type='date'
								label={`${translate('dataNascimento')}:`}
								value={dataNascimento.value}
								onChange={event => setFieldValue('dataNascimento', event.target.value)}
								onFocus={() => setFieldTouched('dataNascimento', true)}
								error={checkError(submitCount, metadataDataNascimento)}
								required
								disabled={disableEdit}
							/>
						</Box>
						<Box width='34%'>
							<FormInput
								label={`${translate('pisPasepNit')}:`}
								value={soNumero(pisPasepNit.value)}
								onChange={event => setFieldValue('pisPasepNit', event.target.value)}
								onFocus={() => setFieldTouched('pisPasepNit', true)}
								error={checkError(submitCount, metadataPisPasepNit)}
								required
								disabled={disableEdit}
							/>
						</Box>
					</Box>

					<Box display='flex' flexDirection='row'>
						<Box width='33%' paddingRight={`${theme.spacing(1)}px`}>
							<FormSelect
								disabled={estadoList.length === 0 || disableEdit}
								label={`${translate('estado')}:`}
								labelInitialItem={translate('selecioneOpcao')}
								items={estadoList}
								value={estadoNascimento.value}
								onChange={event => setEstado(event.target.value)}
								onFocus={() => setFieldTouched('estadoNascimento', true)}
								error={checkError(submitCount, metadataEstadoNascimento)}
								required
							/>
						</Box>
						<Box width='33%' paddingRight={`${theme.spacing(1)}px`}>
							<FormSelect
								disabled={estadoNascimento.value === 0 || disableEdit}
								label={`${translate('municipio')}:`}
								labelInitialItem={translate('selecioneOpcao')}
								items={municipioList}
								value={cidadeNascimento.value}
								onChange={event => setFieldValue('cidadeNascimento', event.target.value)}
								onFocus={() => setFieldTouched('cidadeNascimento', true)}
								error={checkError(submitCount, metadataCidadeNascimento)}
								required
							/>
						</Box>
						<Box width='34%'>
							<FormSelect
								label={`${translate('sexo')}`}
								labelInitialItem={translate('selecioneOpcao')}
								items={SEXO}
								name={sexo}
								error={checkError(submitCount, metadataSexo)}
								required
								disabled={disableEdit}
							/>
						</Box>
					</Box>
				</CardContent>
			</Card>
		</Box>
	);
}
