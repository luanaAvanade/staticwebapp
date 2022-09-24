import React, { useState, useEffect } from 'react';
import { FormSelect, FormInput, Card, Confirm, FormSelectWithSearch } from 'react-axxiom';
import {
	Box,
	CardHeader,
	CardContent,
	FormControlLabel,
	Checkbox,
	MenuItem
} from '@material-ui/core';
import _ from 'lodash';
import { useDispatch } from 'react-redux';
import useReactRouter from 'use-react-router';
import { translate } from '@/locales';
import { checkError } from '@/utils/validation';
import { cnpjMask, soNumero } from '@/utils/mascaras';
import { YES_OR_NO, MEI, ENUM_ITEMS_ANALISE, PESSOAJURIDICA } from '@/utils/constants';
import { Creators as LoaderCreators } from '@/store/ducks/loader';
import EmpresaService from '@/services/empresa';
import theme from '@/theme';
import { temQuatorze, cnpjIsValid } from '@/utils/cnpj';
import { stableSort, getSorting } from '@/utils/list';
import Aprovacao from '@/screens/fornecedor/autoCadastro/aprovacao';
import moment from 'moment';

export default function DadosGerais({
	formulario,
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
	const { history } = useReactRouter();
	const dispatch = useDispatch();

	const [
		atividadeEconomicaPrincipalList,
		setAtividadeEconomicaPrincipalList
	] = useState([]);

	const [
		ocupacaoPrincipalList,
		setOcupacaoPrincipalList
	] = useState([]);

	const [
		modalCnpjJaCadastrado,
		setModalCnpjJaCadastrado
	] = useState(false);

	const [
		CNPJOriginal,
		setCNPJOriginal
	] = useState(null);

	const { submitCount, getFieldProps, setFieldValue, setFieldTouched } = formulario;

	const [
		tipoEmpresa
	] = getFieldProps('tipoEmpresa', 'text');

	const [
		cnpj,
		metadataCnpj
	] = getFieldProps('cnpj', 'text');

	const [
		nomeEmpresa,
		metadataNomeEmpresa
	] = getFieldProps('nomeEmpresa', 'text');

	const [
		isentoIE
	] = getFieldProps('isentoIE', 'text');

	const [
		inscricaoEstadual,
		metadataInscricaoEstadual
	] = getFieldProps('inscricaoEstadual', 'text');

	const [
		inscricaoMunicipal,
		metadataInscricaoMunicipal
	] = getFieldProps('inscricaoMunicipal', 'text');

	const [
		optanteSimplesNacional,
		metadataOptanteSimplesNacional
	] = getFieldProps('optanteSimplesNacional', 'text');

	const [
		dataAbertura,
		metadataDataAbertura
	] = getFieldProps('dataAbertura', 'text');

	const [
		atividadeEconomicaPrincipal,
		metadataAtividadeEconomicaPrincipal
	] = getFieldProps('atividadeEconomicaPrincipal', 'text');

	const [
		ocupacaoPrincipal,
		metadataOcupacaoPrincipal
	] = getFieldProps('ocupacaoPrincipal', 'text');

	const [
		key,
		setKey
	] = useState(0);

	//	Efeito Inicial

	useEffect(() => {
		if (!preCadastro && cnpj.value) {
			setCNPJOriginal(cnpj.value);
		}
		return () => {};
	}, []);

	useEffect(
		() => {
			setAtividadeEconomicaPrincipalList([]);
			setOcupacaoPrincipalList([]);

			if (tipoEmpresa.value === PESSOAJURIDICA.id) {
				//CNAEFindAll();
			}
			if (tipoEmpresa.value === MEI.id) {
				//CNAEOcupacaoFindAll();
			}
		},
		[
			tipoEmpresa.value
		]
	);

	useEffect(
		() => {
			if (preCadastro) {
				if (temQuatorze(cnpj.value) && cnpjIsValid(cnpj.value)) {
					//buscarEmpresa();
				}
			} else {
				if (CNPJOriginal) {
					if (CNPJOriginal !== cnpj.value && temQuatorze(cnpj.value) && cnpjIsValid(cnpj.value)) {
						//buscarEmpresa();
					}
				}
			}
		},
		[
			cnpj.value
		]
	);

	const montaList = list => {
		list.forEach(item => {
			item.label = `${item.Codigo} - ${item.Descricao}`;
		});
		return tipoEmpresa.value === MEI.id
			? stableSort(_.filter(list, value => value.Descricao), getSorting('asc', 'Descricao'))
			: stableSort(list, getSorting('asc', 'Descricao'));
	};

	const CNAEFindAll = async () => {
		dispatch(LoaderCreators.setLoading());
		const response = null;
		if (response.data) {
			setAtividadeEconomicaPrincipalList(
				montaList(response.data.AtividadeEconomicaPrincipalCNAE_list)
			);
			dispatch(LoaderCreators.disableLoading());
		} else {
			dispatch(LoaderCreators.disableLoading());
		}
	};

	const CNAEOcupacaoFindAll = async () => {
		dispatch(LoaderCreators.setLoading());
		const response = null;
		if (response.data) {
			setOcupacaoPrincipalList(montaList(response.data.OcupacaoPrincipalCNAE_list));
			dispatch(LoaderCreators.disableLoading());
		} else {
			dispatch(LoaderCreators.disableLoading());
		}
	};

	const setDadosGerais = empresaExistente => {
		setFieldValue('nomeEmpresa', empresaExistente.NomeEmpresa);
		setFieldValue(
			'dataAbertura',
			moment(new Date(empresaExistente.DataAbertura)).format('YYYY-MM-DD')
		);
		setFieldValue('cep', empresaExistente.Enderecos[0].CEP);
		setFieldValue(
			'atividadeEconomicaPrincipal',
			tipoEmpresa.value === PESSOAJURIDICA.id
				? empresaExistente.AtividadeEconomicaPrincipalId
				: empresaExistente.OcupacaoPrincipalId
		);
		setKey(key + 1);
	};

	const buscarEmpresa = () => {
		dispatch(LoaderCreators.setLoading(translate('buscandoDadosFornecedor')));
		EmpresaService.findByCnpj(cnpj.value)
			.then(response => {
				if (response.data.Empresa_list.length > 0) {
					if (preCadastro) {
						setDadosGerais(response.data.Empresa_list[0]);
					}
					if (!preCadastro && CNPJOriginal && CNPJOriginal !== response.data.Empresa_list[0].CNPJ) {
						setModalCnpjJaCadastrado(true);
					}
					if (!preCadastro && CNPJOriginal && CNPJOriginal === response.data.Empresa_list[0].CNPJ) {
						setDadosGerais(response.data.Empresa_list[0]);
					}
					dispatch(LoaderCreators.disableLoading());
				} else {
					dispatch(LoaderCreators.disableLoading());
					dispatch(LoaderCreators.setLoading(translate('buscandoDadosReceitaFederal')));
					EmpresaService.findByCnpjReceita(cnpj.value)
						.then(responseEmpresa => {
							const dia = responseEmpresa.abertura.substring(0, 2);
							const mes = responseEmpresa.abertura.substring(3, 5);
							const ano = responseEmpresa.abertura.substring(6, 10);
							setFieldValue('nomeEmpresa', responseEmpresa.nome);
							setFieldValue('dataAbertura', `${ano}-${mes}-${dia}`);
							setFieldValue('cep', responseEmpresa.cep);

							let idAtividadePrincipal = null;

							if (
								responseEmpresa.atividade_principal &&
								responseEmpresa.atividade_principal.length > 0
							) {
								let code = soNumero(responseEmpresa.atividade_principal[0].code).toString();

								if (tipoEmpresa.value === PESSOAJURIDICA.id) {
									setFieldValue(
										'atividadeEconomicaPrincipal',
										_.find(
											atividadeEconomicaPrincipalList,
											atividadePrincipal => atividadePrincipal.Codigo.toString() === code
										).Id
									);
								} else {
									setFieldValue(
										'ocupacaoPrincipal',
										_.find(ocupacaoPrincipalList, ocupacao => ocupacao.Codigo.toString() === code)
											.Id
									);
								}
								setKey(key + 1);
							}

							dispatch(LoaderCreators.disableLoading());
						})
						.catch(() => dispatch(LoaderCreators.disableLoading()));
				}
			})
			.catch(() => dispatch(LoaderCreators.disableLoading()));
	};
	const limparCNPJ = () => {
		setFieldValue('cnpj', '');
		setModalCnpjJaCadastrado(false);
	};

	const irLogin = () => {
		history.push('login');
	};

	const setIsentoIE = value => {
		setFieldValue('inscricaoEstadual', '');
		setFieldValue('isentoIE', value);
	};

	const getAtividade = value => {
		if (tipoEmpresa.value === PESSOAJURIDICA.id) {
			return _.find(atividadeEconomicaPrincipalList, atv => atv.Id === value);
		} else {
			return _.find(ocupacaoPrincipalList, atv => atv.Id === value);
		}
	};

	const setTouchedAtividade = () => {
		if (tipoEmpresa.value === PESSOAJURIDICA.id) {
			setFieldTouched('atividadeEconomicaPrincipal', true);
		} else {
			setFieldTouched('ocupacaoPrincipal', true);
		}
	};

	const setAtividade = value => {
		if (tipoEmpresa.value === PESSOAJURIDICA.id) {
			setFieldValue('atividadeEconomicaPrincipal', value ? value.Id : null);
		} else {
			setFieldValue('ocupacaoPrincipal', value ? value.Id : null);
		}
	};

	const getSelectSearch = (list, label, placeholder) => {
		return (
			<FormSelectWithSearch
				key={key}
				placeholder={placeholder}
				label={label}
				options={list}
				getOptionLabel={option => option.Codigo + '-' + option.Descricao}
				value={getAtividade(
					tipoEmpresa.value === PESSOAJURIDICA.id
						? atividadeEconomicaPrincipal.value
						: ocupacaoPrincipal.value
				)}
				onChange={(event, selected) => setAtividade(selected)}
				onFocus={() => setTouchedAtividade()}
				error={checkError(submitCount, metadataAtividadeEconomicaPrincipal)}
				disabled={disableEdit}
				required
			/>
		);
	};

	return (
		<Box paddingTop={`${theme.spacing(1)}px`}>
			<Card>
				<Confirm
					open={modalCnpjJaCadastrado}
					handleClose={limparCNPJ}
					handleSuccess={irLogin}
					title={translate('confirmacao')}
					text={translate('jaexisteCadastroCNPJInformado')}
					textButtonSuccess={translate('sim')}
					textButtonCancel={translate('nao')}
					backgroundColorButtonCancel={theme.palette.secondary.main}
				/>
				<CardHeader
					title={translate('dadosGerais')}
					action={
						!preCadastro && (
							<Aprovacao
								itensAnalise={itensAnalise}
								setItensAnalise={setItensAnalise}
								comentarios={comentarios}
								setComentarios={setComentarios}
								tipoItem={ENUM_ITEMS_ANALISE.find(x => x.internalName === 'Dados_Gerais').value}
								historicoEmpresa={historicoEmpresa}
								user={user}
								disableEdit={disableEdit}
								statusEmpresa={statusEmpresa}
							/>
						)
					}
				/>
				<CardContent>
					<Box display='flex' flexDirection='row' paddingTop={`${theme.spacing(1)}px`}>
						<Box width='25%' paddingRight={`${theme.spacing(1)}px`}>
							<FormInput
								label={`${translate('cnpj')}:`}
								value={cnpjMask(cnpj.value)}
								onChange={event => setFieldValue('cnpj', soNumero(event.target.value))}
								onFocus={() => setFieldTouched('cnpj', true)}
								error={checkError(submitCount, metadataCnpj)}
								required
								disabled={disableEdit}
							/>
						</Box>
						<Box width='75%'>
							<FormInput
								label={`${translate('nomeEmpresa')}:`}
								name={nomeEmpresa}
								error={checkError(submitCount, metadataNomeEmpresa)}
								required
								disabled={disableEdit}
							/>
						</Box>
					</Box>
					<Box display='flex' flexDirection='row'>
						<Box width='25%' paddingRight={`${theme.spacing(1)}px`}>
							<FormInput
								label={`${translate('inscricaoEstadual')}:`}
								value={soNumero(inscricaoEstadual.value)}
								onChange={event => setFieldValue('inscricaoEstadual', event.target.value)}
								onFocus={() => setFieldTouched('inscricaoEstadual', true)}
								error={checkError(submitCount, metadataInscricaoEstadual)}
								disabled={isentoIE.value || disableEdit}
								required={!isentoIE.value}
							/>
							<FormControlLabel
								control={
									<Checkbox
										checked={isentoIE.value}
										name={isentoIE}
										value={isentoIE.checked}
										onChange={event => setIsentoIE(event.target.checked)}
										onFocus={() => setFieldTouched('inscricaoEstadual', true)}
										disabled={disableEdit}
									/>
								}
								label={`${translate('isentoIE')}`}
							/>
						</Box>
						<Box width='40%' paddingRight={`${theme.spacing(1)}px`}>
							<FormInput
								label={`${translate('inscricaoMunicipal')}:`}
								value={soNumero(inscricaoMunicipal.value)}
								onChange={event => setFieldValue('inscricaoMunicipal', event.target.value)}
								onFocus={() => setFieldTouched('inscricaoMunicipal', true)}
								error={checkError(submitCount, metadataInscricaoMunicipal)}
								disabled={disableEdit}
							/>
						</Box>
						<Box width='35%'>
							<FormSelect
								label={`${translate('optanteSimplesNacional')}:`}
								labelInitialItem={translate('selecioneOpcao')}
								items={YES_OR_NO}
								name={optanteSimplesNacional}
								error={checkError(submitCount, metadataOptanteSimplesNacional)}
								required
								disabled={disableEdit}
							/>
						</Box>
					</Box>
					<Box display='flex' flexDirection='row'>
						<Box width='25%' paddingRight={`${theme.spacing(1)}px`}>
							<FormInput
								type='date'
								label={`${translate('dataAbertura')}:`}
								value={dataAbertura.value}
								onChange={event => setFieldValue('dataAbertura', event.target.value)}
								onFocus={() => setFieldTouched('dataAbertura', true)}
								error={checkError(submitCount, metadataDataAbertura)}
								required
								disabled={disableEdit}
							/>
						</Box>
						<Box width='75%' paddingRight='8px'>
							{tipoEmpresa.value === PESSOAJURIDICA.id &&
								atividadeEconomicaPrincipalList.length > 0 &&
								getSelectSearch(
									atividadeEconomicaPrincipalList,
									`${translate('atividadeEconomicaPrincipal')}`,
									`${translate('digiteAtividade')}`
								)}

							{tipoEmpresa.value === MEI.id &&
								ocupacaoPrincipalList.length > 0 &&
								getSelectSearch(
									ocupacaoPrincipalList,
									`${translate('ocupacaoPrincipal')}`,
									`${translate('digiteOcupacao')}`
								)}
						</Box>
					</Box>
				</CardContent>
			</Card>
		</Box>
	);
}
