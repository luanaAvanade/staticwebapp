import React, { useState, useEffect, Fragment } from 'react';
import {
	Modal,
	FormSelect,
	FormInput,
	Table,
	TableHead,
	TableRow,
	TableCell,
	Button,
	Card,
	Confirm
} from 'react-axxiom';
import { Edit, Delete } from '@material-ui/icons';
import _ from 'lodash';
import { CardHeader, Box, TableBody, IconButton, CardContent } from '@material-ui/core';
import { useDispatch } from 'react-redux';
import { Creators as LoaderCreators } from '@/store/ducks/loader';
import { translate } from '@/locales';
import { checkError, emailIsValid } from '@/utils/validation';
import theme from '@/theme';
import ObjectHelper from '@/utils/objectHelper';
import { celularMask } from '@/utils/mascaras';
import { COLUMNS_DADOS_CONTATOS_ADICIONAIS } from './tableHead';
import { ENUM_ITEMS_ANALISE } from '@/utils/constants';
import Aprovacao from '@/screens/fornecedor/autoCadastro/aprovacao';
import { snackError, snackSuccess } from '@/utils/snack';
import { useSnackbar } from 'notistack';

export default function DadosContatosAdicionais({
	formulario,
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
	const { enqueueSnackbar, closeSnackbar } = useSnackbar();

	// Estado Local
	const [
		openModal,
		setOpenModal
	] = useState(false);

	const [
		indexContato,
		setIndexContato
	] = useState();

	const [
		tipoContato,
		setTipoContato
	] = useState(0);

	const [
		nome,
		setNome
	] = useState('');

	const [
		telefone,
		setTelefone
	] = useState('');

	const [
		email,
		setEmail
	] = useState('');

	const [
		submitCountLocal,
		setSubmitCountLocal
	] = useState(0);

	const [
		textButtonModal,
		setTextButtonModal
	] = useState('incluir');

	const [
		textTituloModal,
		setTextTituloModal
	] = useState('adicionarContato');

	const [
		tipoContatoList,
		setTipoContatoList
	] = useState();

	const [
		indexContatoExcluir,
		setIndexContatoExcluir
	] = useState(null);

	// Efeito Inicial

	useEffect(() => {
		//tipoContatoFindAll();
		return () => {
			setTipoContatoList([]);
		};
	}, []);

	// Buscar Dados

	const tipoContatoFindAll = async () => {
		dispatch(LoaderCreators.setLoading());
		const response = null;
		if (response.data) {
			setTipoContatoList(response.data.TipoContato_list);
			dispatch(LoaderCreators.disableLoading());
		} else {
			dispatch(LoaderCreators.disableLoading());
		}
	};

	// Formulario

	const { submitCount, getFieldProps, setFieldValue, setFieldTouched } = formulario;

	const [
		contatosAdicionais,
		metaDataContatosAdicionais
	] = getFieldProps('contatosAdicionais', 'text');

	// Ações da Tela

	const checkEmail = () => {
		if (submitCountLocal > 0 && !email) {
			return translate('campoObrigatorio');
		}
		if (submitCountLocal > 0 && !emailIsValid(email)) {
			return translate('emailInvalido');
		}
		return '';
	};

	const validateForm = () => {
		return tipoContato !== 0 && nome && email && telefone && emailIsValid(email);
	};

	const incluir = () => {
		setSubmitCountLocal(1);
		if (validateForm()) {
			const list = ObjectHelper.clone(contatosAdicionais.value);
			const existeNaLista = list.findIndex(obj => obj.Email === email);
			if (existeNaLista !== -1) {
				if (indexContato !== null && list[indexContato].Email === email) {
				} else {
					callbackError(translate('emailjacadastrado'));
					return false;
				}
			}

			if (indexContato !== null) {
				list[indexContato] = {
					Id: list[indexContato].Id,
					TipoContatoId: tipoContato,
					NomeContato: nome,
					Telefone: telefone,
					Email: email
				};
			} else {
				list.push({
					TipoContatoId: tipoContato,
					NomeContato: nome,
					Telefone: telefone,
					Email: email
				});
			}
			setFieldValue('contatosAdicionais', list);
			setFieldTouched('contatosAdicionais', true);
			closeModal();
			return false;
		}
	};

	const closeModal = () => {
		setSubmitCountLocal(0);
		setOpenModal(false);
	};

	const editarContato = (index, contato) => {
		setTextButtonModal('atualizar');
		//setTextTituloModal('editarContato'), setOpenModal(true);
		setTipoContato(contato.TipoContatoId);
		setNome(contato.NomeContato);
		setEmail(contato.Email);
		setTelefone(contato.Telefone);
		setIndexContato(index);
	};

	const novoContato = () => {
		setTextButtonModal('incluir');
		//setTextTituloModal('adicionarContato'), setOpenModal(true);
		setTipoContato(0);
		setNome('');
		setEmail('');
		setTelefone('');
		setIndexContato(null);
	};

	const deleteContato = () => {
		const list = ObjectHelper.clone(contatosAdicionais.value);
		list.splice(indexContatoExcluir, 1);
		setFieldValue('contatosAdicionais', list);
		setFieldTouched('contatosAdicionais', true);
		setIndexContatoExcluir(null);
	};

	const callbackError = mensagem => {
		enqueueSnackbar('', snackError(mensagem, closeSnackbar));
	};

	// Interações com a Tabela

	let variantTableRow = theme.palette.table.tableRowPrimary;

	return (
		<Box paddingTop={`${theme.spacing(1)}px`}>
			<Confirm
				open={indexContatoExcluir !== null}
				handleSuccess={deleteContato}
				handleClose={() => setIndexContatoExcluir(null)}
				title={translate('confirmacao')}
				text={translate('desejaRealmenteExcluirContato')}
				textButtonSuccess={translate('sim')}
				textButtonCancel={translate('nao')}
				backgroundColorButtonCancel={theme.palette.secondary.main}
			/>

			<Modal
				open={openModal}
				handleClose={() => closeModal()}
				title={translate(textTituloModal)}
				textButton={translate(textButtonModal)}
				onClickButton={() => incluir()}
			>
				<Box display='flex' flexDirection='row'>
					<Box width='35%' paddingRight={`${theme.spacing(1)}px`}>
						<FormSelect
							label={`${translate('tipoContato')}`}
							labelInitialItem={translate('selecioneOpcao')}
							items={tipoContatoList}
							value={tipoContato}
							onChange={event => setTipoContato(event.target.value)}
							error={submitCountLocal > 0 && tipoContato === 0 ? translate('campoObrigatorio') : ''}
							required
						/>
					</Box>
					<Box width='65%' paddingRight={`${theme.spacing(1)}px`}>
						<FormInput
							label={`${translate('nomeContatoAdicional')}:`}
							value={nome}
							onChange={event => setNome(event.target.value)}
							error={submitCountLocal > 0 && !nome ? translate('campoObrigatorio') : ''}
							required
						/>
					</Box>
				</Box>
				<Box display='flex' flexDirection='row'>
					<Box width='35%' paddingRight={`${theme.spacing(1)}px`}>
						<FormInput
							label={`${translate('telefoneContatoAdicional')}:`}
							value={celularMask(telefone)}
							onChange={event => setTelefone(event.target.value)}
							error={submitCountLocal > 0 && !telefone ? translate('campoObrigatorio') : ''}
							required
						/>
					</Box>
					<Box width='65%' paddingRight={`${theme.spacing(1)}px`}>
						<FormInput
							label={`${translate('emailContatoAdicional')}:`}
							value={email}
							onChange={event => setEmail(event.target.value.toString().toLowerCase())}
							error={checkEmail()}
							required
						/>
					</Box>
				</Box>
			</Modal>

			<Card
				borderColor={theme.palette.danger.main}
				error={checkError(submitCount, metaDataContatosAdicionais)}
			>
				<CardHeader
					title={translate('contatoAdicionais')}
					action={
						<Fragment>
							<Box>
								{!preCadastro && (
									<Aprovacao
										itensAnalise={itensAnalise}
										setItensAnalise={setItensAnalise}
										comentarios={comentarios}
										setComentarios={setComentarios}
										tipoItem={
											ENUM_ITEMS_ANALISE.find(x => x.internalName === 'Dados_Contatos_Adicionais')
												.value
										}
										historicoEmpresa={historicoEmpresa}
										user={user}
										disableEdit={disableEdit}
										statusEmpresa={statusEmpresa}
									/>
								)}
							</Box>
							<Box paddingTop={`${theme.spacing(1)}px`}>
								{!disableEdit && (
									<Button
										text={translate('adicionar')}
										backgroundColor={theme.palette.primary.main}
										onClick={() => novoContato()}
										disabled={disableEdit}
									/>
								)}
							</Box>
						</Fragment>
					}
				/>
				<CardContent>
					<Table small>
						<TableHead columns={COLUMNS_DADOS_CONTATOS_ADICIONAIS} />
						<TableBody>
							{contatosAdicionais.value &&
								contatosAdicionais.value.length > 0 &&
								contatosAdicionais.value.map((contato, index) => {
									variantTableRow =
										variantTableRow === theme.palette.table.tableRowPrimary
											? theme.palette.table.tableRowSecondary
											: theme.palette.table.tableRowPrimary;

									return (
										<TableRow key={index} backgroundColor={variantTableRow}>
											<TableCell
												label={
													tipoContatoList && tipoContatoList.length > 0 ? (
														_.find(tipoContatoList, c => contato.TipoContatoId === c.value).label
													) : (
														''
													)
												}
											/>
											<TableCell label={contato.NomeContato} />
											<TableCell label={contato.Telefone} />
											<TableCell label={contato.Email} />
											<TableCell
												title={translate('editar')}
												label={
													<IconButton
														size='small'
														onClick={() => editarContato(index, contato)}
														disabled={disableEdit}
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
														onClick={() => setIndexContatoExcluir(index)}
														disabled={disableEdit}
													>
														<Delete />
													</IconButton>
												}
											/>
										</TableRow>
									);
								})}
							{contatosAdicionais.value &&
							contatosAdicionais.value.length === 0 && (
								<TableRow backgroundColor={variantTableRow}>
									<TableCell align='center' colSpan={6} label={translate('semResultadosAExibir')} />
								</TableRow>
							)}
						</TableBody>
					</Table>
				</CardContent>
			</Card>
		</Box>
	);
}
