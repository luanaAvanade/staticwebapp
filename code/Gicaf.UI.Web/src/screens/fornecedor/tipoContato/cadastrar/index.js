import React, { useState, useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { useFormik, Form } from 'formik';
import * as Yup from 'yup';
import { useSnackbar } from 'notistack';
import { Box } from '@material-ui/core';
import useReactRouter from 'use-react-router';
import { Button, Confirm, FormInput } from 'react-axxiom';
import { LayoutContent } from '@/layout';
import { translate } from '@/locales';
import { Creators as LoaderCreators } from '@/store/ducks/loader';

import { snackError, snackSuccess, snackWarning } from '@/utils/snack';
import theme from '@/theme';
import { checkError } from '@/utils/validation';

import { Switch } from '../../style';

export default function CadastrarTipoContato() {
	const { history, match } = useReactRouter();
	// eslint-disable-next-line no-restricted-globals
	const id = isNaN(parseInt(match.params.id, 10)) ? null : parseInt(match.params.id, 10);
	const { enqueueSnackbar, closeSnackbar } = useSnackbar();
	const dispatch = useDispatch();

	// Estado Local

	const [
		openConfirmAlterar,
		setOpenConfirmAlterar
	] = useState(false);

	const [
		status,
		setStatus
	] = useState(false);

	// Efeito Inicial

	useEffect(() => {
		if (id) {
			tipoContatoFindById();
		}

		return () => {
			setFieldValue('nome', '');
			setFieldValue('descricao', '');
		};
	}, []);

	// Busca de Dados

	const tipoContatoFindById = async () => {
		dispatch(LoaderCreators.setLoading());
		const response = null;
		if (response.data) {
			setFieldValue('nome', response.data.TipoContato.Nome, false);
			setFieldValue('descricao', response.data.TipoContato.Descricao, false);
			setStatus(response.data.TipoContato.Status);
			dispatch(LoaderCreators.disableLoading());
		} else {
			dispatch(LoaderCreators.disableLoading());
		}
	};

	// Ações da Tela

	const getTipoContato = () => {
		return {
			Nome: nome.value,
			Descricao: descricao.value,
			Status: status
		};
	};

	const save = () => {
		if (isValid) {
			if (id) {
				setOpenConfirmAlterar(true);
			} else {
				create();
			}
		}
	};
	const create = async () => {
		
	};

	const update = async () => {
		
	};

	// Ações de Retorno

	const callback = mensagem => {
		voltar();
		enqueueSnackbar('', snackSuccess(mensagem, closeSnackbar));
	};

	const callbackError = mensagem => {
		enqueueSnackbar('', snackError(mensagem, closeSnackbar));
	};

	const callbackWarning = mensagem => {
		enqueueSnackbar('', snackWarning(mensagem, closeSnackbar));
	};
	const voltar = () => {
		history.goBack();
	};

	// Formulário

	const initialValues = {
		nome: '',
		descricao: ''
	};

	const validationSchema = Yup.object().shape({
		nome: Yup.string().required(translate('campoObrigatorio'))
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
		descricao
	] = getFieldProps('descricao', 'text');

	return (
		<LayoutContent>
			<Confirm
				open={openConfirmAlterar}
				handleClose={() => setOpenConfirmAlterar(false)}
				handleSuccess={update}
				title={translate('confirmacao')}
				text={translate('desejaRealmenteAlterarTipoContato')}
				textButtonSuccess={translate('sim')}
				textButtonCancel={translate('nao')}
				backgroundColorButtonCancel={theme.palette.secondary.main}
			/>

			<Form onSubmit={handleSubmit}>
				<FormInput
					label={`@/..{translate('nome')}:`}
					name={nome}
					error={checkError(submitCount, metadataNome)}
				/>
				<FormInput label={`@/..{translate('descricao')}:`} name={descricao} />
				<Switch
					label='Status:'
					onChange={() => {
						setStatus(!status);
					}}
					checked={status}
					checkedName={status ? 'Ativo' : 'Inativo'}
				/>
				<Box display='flex' justifyContent='flex-end'>
					<Button text='Voltar' backgroundColor={theme.palette.secondary.main} onClick={voltar} />
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
