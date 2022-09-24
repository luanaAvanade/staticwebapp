import React from 'react';
import { Box } from '@material-ui/core';
import { useDispatch } from 'react-redux';
import { useFormik, Form } from 'formik';
import * as Yup from 'yup';
import { useSnackbar } from 'notistack';
import { Card, FormInput, Button } from 'react-axxiom';
import { CemigVerde } from '@/assets';
import { Image, ContentLogin, Background } from './style';
import { Footer } from '@/layout';
import { translate } from '@/locales';
import theme from '@/theme';
import { snackSuccess, snackError, snackWarning } from '@/utils/snack';
import LoginService from '@/services/login';
import UsuarioService from '@/services/usuario';
import { checkError } from '@/utils/validation';
import { Creators as LoaderCreators } from '@/store/ducks/loader';
import { RouterLink } from '../login/style';
import paths from '@/utils/paths';
import { SUBDIRETORIO_LINK } from '@/utils/constants';

export default function SolicitacaoRedefinicaoSenha() {
	const { enqueueSnackbar, closeSnackbar } = useSnackbar();
	const minheight = 260;
	const width = 350;
	const dispatch = useDispatch();

	// Ações da Tela

	const enviar = () => {
		const url = `@/..{document.location.origin}@/..{SUBDIRETORIO_LINK}`;
		dispatch(LoaderCreators.setLoading());
		UsuarioService.findByEmail(email.value)
			.then(response => {
				if (response.data && response.data.Usuario_list.length > 0) {
					if (isValid) {
						dispatch(LoaderCreators.setLoading());
						LoginService.resetPassword({
							Id: email.value,
							LinkConfirmacao: `@/..{url}/redefinicao-senha?email={userName}&token={token}`
						})
							.then(result => callback(result.token))
							.catch(() => callbackError(translate('erroAoSolicitarRedefinicaoSenha')));
					}
				} else {
					callbackWarning(translate('emailNaoCadastrado'));
				}
			})
			.catch(() => {
				callbackError(translate('erroAoSolicitarRedefinicaoSenha'));
			});
	};

	// Ações de Retorno

	const callback = () => {
		dispatch(LoaderCreators.disableLoading());
		enqueueSnackbar('', snackSuccess(translate('emailEnviadoComSucesso'), closeSnackbar));
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
		email: ''
	};

	const validationSchema = Yup.object().shape({
		email: Yup.string().required(translate('campoObrigatorio')).email(translate('emailInvalido'))
	});

	const { getFieldProps, handleSubmit, isValid, submitCount } = useFormik({
		initialValues,
		validationSchema,
		onSubmit: enviar
	});

	const [
		email,
		metadataEmail
	] = getFieldProps('email', 'text');

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
							label={`@/..{translate('email')}:`}
							labelHelper={translate('envieEmailRecebaOLink')}
							fullWidth
							name={email}
							error={checkError(submitCount, metadataEmail)}
						/>

						<Box display='flex' flexDirection='column'>
							<RouterLink to={paths.getPathByCodigo('esqueci-meu-email-verifica')}>
								{translate('esqueciMeuEmail')}
							</RouterLink>
							<Button text={translate('enviar')} type='submit' fullWidth />
						</Box>
					</Form>
				</Card>
			</ContentLogin>
			<Footer />
		</Background>
	);
}
