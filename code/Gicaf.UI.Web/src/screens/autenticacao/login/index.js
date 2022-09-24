import React from 'react';
import { useDispatch } from 'react-redux';
import { Box, FormHelperText } from '@material-ui/core';
import useReactRouter from 'use-react-router';
import { useFormik, Form } from 'formik';
import * as Yup from 'yup';
import ReCAPTCHA from 'react-google-recaptcha';
import { useSnackbar } from 'notistack';
import { Card, FormInput, Button } from 'react-axxiom';
import CemigVerde from '@/assets/cemig-verde.png';
import { Image, Background, ContentLogin, RouterLink } from './style';
import Footer from '@/layout/footer';
import { translate } from '@/locales';
import { snackError, snackWarning } from '@/utils/snack';
import paths from '@/utils/paths';
import theme from '@/theme';
import { Creators as LoaderCreators } from '@/store/ducks/loader';
import LoginService from '@/services/login';
import { setUser } from '@/utils/auth';
import { checkFuncionalidade } from '@/utils/modulos';
import { FORNECEDOR_CADASTRO, CRIAR, FORNECEDOR_ANALISE_CADASTRO } from '@/utils/constants';
import { checkError, emailIsValid } from '@/utils/validation';

export default function Login() {
	const { history } = useReactRouter();
	const { enqueueSnackbar, closeSnackbar } = useSnackbar();
	const minheight = 260;
	const width = 350;
	const dispatch = useDispatch();

	// Ação da Tela

	const entrar = () => {
		// if (isValid) {
		// 	dispatch(LoaderCreators.setLoading());
		// 	LoginService.login({ Id: login.value, Password: senha.value })
		// 		.then(response => {
		// 			if (!response.autResult.authenticated) {
		// 				callbackError(translate(response.autResult.message));
		// 			} else {
		// 				callback(
		// 					JSON.stringify(
		// 						Object.assign(
		// 							{
		// 								accessToken: response.autResult.accessToken,
		// 								permissions: response.autResult.user.permissions
		// 							},
		// 							response.appUser
		// 						)
		// 					)
		// 				);
		// 			}
		// 		})
		// 		.catch(() => {
		// 			callbackWarning(translate('usuarioOuSenhaInvalido'));
		// 		});
		// }

		callback(
			JSON.stringify(
				Object.assign(
					{
						accessToken: "",
						permissions: null
					},
					null
				)
			)
		);
	};

	// Ação de Retorno

	const callback = user => {
		// dispatch(LoaderCreators.disableLoading());
		// setUser(user);
		// if (checkFuncionalidade(FORNECEDOR_CADASTRO, CRIAR)) {
		 	history.push(paths.getPathByCodigo('cadastro-complementar'));
		// } else if (checkFuncionalidade(FORNECEDOR_ANALISE_CADASTRO, CRIAR)) {
		// 	history.push(paths.getPathByCodigo('analise-cadastro'));
		// } else {
		//	history.push(paths.getPathByCodigo('home'));
		//}
	};

	const callbackError = mensagem => {
		dispatch(LoaderCreators.disableLoading());
		enqueueSnackbar('', snackError(mensagem, closeSnackbar));
	};

	const callbackWarning = mensagem => {
		dispatch(LoaderCreators.disableLoading());
		enqueueSnackbar('', snackWarning(mensagem, closeSnackbar));
	};

	// Formulários

	const initialValues = {
		login: '',
		senha: '',
		recaptcha: ''
	};

	const testRecaptcha = value => {
		if (emailIsValid(login.value)) {
			if (!value) {
				return false;
			}
		}
		return true;
	};

	const validationSchema = Yup.object().shape({
		login: Yup.string().required(translate('campoObrigatorio')),
		senha: Yup.string().required(translate('campoObrigatorio')),
		recaptcha: Yup.string().test('recaptcha', translate('campoObrigatorio'), value =>
			testRecaptcha(value)
		)
	});

	const { getFieldProps, handleSubmit, isValid, submitCount, setFieldValue } = useFormik({
		initialValues,
		validationSchema,
		onSubmit: entrar
	});

	const [
		login,
		metadataLogin
	] = getFieldProps('login', 'text');

	const [
		senha,
		metadataSenha
	] = getFieldProps('senha', 'text');

	const [
		// eslint-disable-next-line no-unused-vars
		recaptcha,
		metadataRecaptcha
	] = getFieldProps('recaptcha', 'text');

	return (
		<Background>
			<ContentLogin
				top={`calc( 50% - (${minheight}px/2) - 60px)`}
				left={`calc( 50% - (${width}px/2))`}
			>
				<Card width={`${width}px`} minheight={`${minheight}px`} padding={theme.spacing(2)}>
					<Box display='flex' justifyContent='space-around'>
						<Image src={CemigVerde} />
					</Box>

					<Form onSubmit={handleSubmit}>
						<FormInput
							label='Usuário:'
							fullWidth
							name={login}
							error={checkError(submitCount, metadataLogin)}
						/>

						<FormInput
							label='Senha:'
							fullWidth
							type='password'
							name={senha}
							error={checkError(submitCount, metadataSenha)}
						/>
						{emailIsValid(login.value) && (
							<Box
								style={{
									marginTop: 16,
									marginBottom: 16,
									borderRadius: 5,
									padding: 8,
									border: `${checkError(submitCount, metadataRecaptcha) ? 1 : 0}px solid red`
								}}
							>
								<ReCAPTCHA
									// DEV 6LeIxAcTAAAAAJcZVRqyHh71UMIEGNQ_MXjiZKhI
									// Rafael 6Lekyr0UAAAAACHk12i1CUaBBVETiCMs3GWrqVOP
									sitekey='6LeIxAcTAAAAAJcZVRqyHh71UMIEGNQ_MXjiZKhI'
									onChange={value => setFieldValue('recaptcha', value)}
								/>
								{checkError(submitCount, metadataRecaptcha) && (
									<FormHelperText style={{ color: 'red' }}>
										{metadataRecaptcha.error}
									</FormHelperText>
								)}
							</Box>
						)}
						<Box display='flex' flexDirection='column'>
							<RouterLink to={paths.getPathByCodigo('solicitacao-redefinicao-senha')}>
								{translate('esqueciMinhaSenha')}
							</RouterLink>
							<Button text={translate('entrar')} type='submit' fullWidth />
						</Box>
					</Form>
				</Card>
			</ContentLogin>
			<Footer />
		</Background>
	);
}
