import React from 'react';
import { Box } from '@material-ui/core';
import { useDispatch } from 'react-redux';
import useReactRouter from 'use-react-router';
import { useFormik, Form } from 'formik';
import * as Yup from 'yup';
import { useSnackbar } from 'notistack';
import QueryString from 'query-string';
import { Card, FormInput, Button } from 'react-axxiom';
import { CemigVerde } from '@/assets';
import { Image, ContentLogin, Background } from './style';
import { Footer } from '@/layout';
import { translate } from '@/locales';
import theme from '@/theme';
import { snackSuccess, snackError } from '@/utils/snack';
import LoginService from '@/services/login';
import { checkError } from '@/utils/validation';
import { Creators as LoaderCreators } from '@/store/ducks/loader';

export default function RedefinicaoSenha() {
	const { history, location } = useReactRouter();
	const queryString = QueryString.parse(location.search);
	const { enqueueSnackbar, closeSnackbar } = useSnackbar();
	const dispatch = useDispatch();
	const minheight = 260;
	const width = 350;

	// Ação da Tela

	const atualizar = () => {
		if (isValid) {
			dispatch(LoaderCreators.setLoading());
			LoginService.changePassword({
				Id: queryString.email,
				Password: senha.value,
				ConfirmPassword: confirmacao.value,
				Token: queryString.token
			})
				.then(() => callback())
				.catch(err => callbackError(err.message));
		}
	};

	// Ações de Retorno

	const callback = () => {
		history.push('login');
		dispatch(LoaderCreators.disableLoading());
		enqueueSnackbar('', snackSuccess(translate('senhaAtualizadaComSucesso'), closeSnackbar));
	};

	const callbackError = message => {
		dispatch(LoaderCreators.disableLoading());
		enqueueSnackbar('', snackError(message, closeSnackbar));
	};

	// Formulários

	const initialValues = {
		senha: '',
		confirmacao: ''
	};

	const validationSchema = Yup.object().shape({
		senha: Yup.string().required(translate('campoObrigatorio')),
		confirmacao: Yup.string()
			.oneOf(
				[
					Yup.ref('senha')
				],
				translate('confirmacaoDeveSerIgualANovaSenha')
			)
			.required(translate('campoObrigatorio'))
	});

	const { getFieldProps, handleSubmit, isValid, submitCount } = useFormik({
		initialValues,
		validationSchema,
		onSubmit: atualizar
	});

	const [
		senha,
		metadataSenha
	] = getFieldProps('senha', 'text');

	const [
		confirmacao,
		metadataConfirmacao
	] = getFieldProps('confirmacao', 'text');

	return (
		<Background>
			<ContentLogin
				top={`calc( 50% - (@/..{minheight}px/2) - 60px)`}
				left={`calc( 50% - (@/..{width}px/2))`}
			>
				<Card width={`@/..{width}px`} minheight={`@/..{minheight}px`} padding={theme.spacing(2)}>
					<Box display='flex' justifyContent='space-around'>
						<Image src={CemigVerde} />
					</Box>

					<Form onSubmit={handleSubmit}>
						<FormInput
							label={`@/..{translate('novaSenha')}:`}
							fullWidth
							type='password'
							name={senha}
							error={checkError(submitCount, metadataSenha)}
						/>

						<FormInput
							label={`@/..{translate('confirmacao')}:`}
							labelHelper={translate('digiteOMesmoConteudoDaNovaSenha')}
							fullWidth
							type='password'
							name={confirmacao}
							error={checkError(submitCount, metadataConfirmacao)}
						/>

						<Box display='flex'>
							<Button text={translate('atualizar')} type='submit' fullWidth />
						</Box>
					</Form>
				</Card>
			</ContentLogin>
			<Footer />
		</Background>
	);
}
