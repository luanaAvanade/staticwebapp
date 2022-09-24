import React, { useState, useEffect } from 'react';
import {
	Box,
	FormControlLabel,
	Radio,
	RadioGroup,
	FormHelperText,
	Typography
} from '@material-ui/core';
import { useDispatch } from 'react-redux';
import useReactRouter from 'use-react-router';
import { useFormik, Form } from 'formik';
import * as Yup from 'yup';
import { useSnackbar } from 'notistack';
import { Card, FormInput, Button } from 'react-axxiom';
import { CemigVerde } from '@/assets';
import { Image, ContentLogin, Background } from './style';
import { Footer } from '@/layout';
import { translate } from '@/locales';
import theme from '@/theme';
import { snackSuccess, snackError } from '@/utils/snack';
import LoginService from '@/services/login';
import UsuarioService from '@/services/usuario';
import { checkError } from '@/utils/validation';
import { Creators as LoaderCreators } from '@/store/ducks/loader';
import { BoxTextRecuperaAcesso } from '../verifica/style';

export default function EsqueciEmailEnvia() {
	const { location } = useReactRouter();
	const { enqueueSnackbar, closeSnackbar } = useSnackbar();
	const dispatch = useDispatch();
	const minheight = 564;
	const width = 905;

	const regexEmail = /(?<=.{3}).(?=[^@]*?@)/g;

	const [
		data,
		setData
	] = useState([]);

	// Ação da Tela

	useEffect(() => {
		setData(location.state);
		return () => {
			setData([]);
		};
	}, []);

	const validaEmail = async () => {
		if (isValid) {
			if (emailSelecionado.value) {
				const response = await UsuarioService.findById(emailSelecionado.value);

				if (response) {
					if (response.data.Usuario.Email === email.value) {
						atualizar();
					} else {
						callbackError(translate('confirmacaoDeveSerIgualAoEmail'));
					}
				}
			}
		}
	};

	const atualizar = () => {
		dispatch(LoaderCreators.setLoading());
		LoginService.resetPassword({
			Id: email.value
		})
			.then(() => callback())
			.catch(() => callbackError(translate('falhaResetPassword')));
	};

	// Ações de Retorno

	const callback = () => {
		dispatch(LoaderCreators.disableLoading());
		enqueueSnackbar('', snackSuccess(translate('emailEnviadoComSucesso'), closeSnackbar));
	};

	const callbackError = message => {
		dispatch(LoaderCreators.disableLoading());
		enqueueSnackbar('', snackError(message, closeSnackbar));
	};

	// Formulários

	const initialValues = {
		email: '',
		emailSelecionado: ''
	};

	const validationSchema = Yup.object().shape({
		email: Yup.string().required(translate('campoObrigatorio')),
		emailSelecionado: Yup.string().required(translate('campoObrigatorio'))
	});

	const { getFieldProps, handleSubmit, isValid, submitCount, setFieldValue } = useFormik({
		initialValues,
		validationSchema,
		onSubmit: validaEmail
	});

	const [
		email,
		metadataEmail
	] = getFieldProps('email', 'text');

	const [
		emailSelecionado,
		metadataEmailSelecionado
	] = getFieldProps('emailSelecionado', 'text');

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
					<BoxTextRecuperaAcesso>
						<Typography variant='h4'>
							<b>{translate('recuperarDadosAcesso')}</b>
						</Typography>
					</BoxTextRecuperaAcesso>

					<BoxTextRecuperaAcesso>
						<Typography variant='h5'>{translate('recuperacaoCpfCnpj')}</Typography>
					</BoxTextRecuperaAcesso>

					<Form onSubmit={handleSubmit}>
						<Box display='flex' flexDirection='row'>
							<Box
								style={{
									marginTop: 16,
									marginBottom: 16,
									borderRadius: 5,
									padding: 8,
									border: `@/..{checkError(submitCount, metadataEmailSelecionado) ? 1 : 0}px solid red`
								}}
							>
								<RadioGroup
									value={emailSelecionado.value}
									onChange={event => setFieldValue('emailSelecionado', event.target.value)}
								>
									{data &&
										data.length > 0 &&
										data.map(d => {
											return (
												<FormControlLabel
													value={d.Id.toString()}
													control={<Radio />}
													label={d.Email.replace(regexEmail, '*')}
												/>
											);
										})}
								</RadioGroup>
								{checkError(submitCount, metadataEmailSelecionado) && (
									<FormHelperText style={{ color: 'red' }}>
										{metadataEmailSelecionado.error}
									</FormHelperText>
								)}
							</Box>
						</Box>

						<FormInput
							label={`@/..{translate('Confirme seu e-mail de acesso')}:`}
							labelHelper={translate('')}
							fullWidth
							name={email}
							error={checkError(submitCount, metadataEmail)}
						/>
						<Box display='flex'>
							<Button text={translate('envia')} type='submit' fullWidth />
						</Box>
					</Form>
				</Card>
			</ContentLogin>
			<Footer />
		</Background>
	);
}
