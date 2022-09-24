import React from 'react';
import { Box } from '@material-ui/core';
import { useDispatch } from 'react-redux';
import useReactRouter from 'use-react-router';
import { useFormik, Form } from 'formik';
import * as Yup from 'yup';
import { Card, FormInput, Button } from 'react-axxiom';
import { CemigVerde } from '@/assets';
import { Image, ContentLogin, Background, BoxTextRecuperaAcesso } from './style';
import { Footer } from '@/layout';
import { translate } from '@/locales';
import theme from '@/theme';
import UsuarioService from '@/services/usuario';
import { checkError } from '@/utils/validation';
import { temOnze } from '@/utils/cpf';
import { temQuatorze } from '@/utils/cnpj';
import { Creators as LoaderCreators } from '@/store/ducks/loader';
import { Typography } from '@/layout/title/style';

export default function EsqueciMeuEmail() {
	const { history } = useReactRouter();
	const dispatch = useDispatch();
	const minheight = 260;
	const width = 450;

	// Ação da Tela

	const findEmailByCNPJ = async cnpj => {
		const response = await UsuarioService.emailByCNPJ(cnpj);
		const uList = [];
		if (response.data.Empresa_list && response.data.Empresa_list.length > 0) {
			response.data.Empresa_list.forEach(em => {
				uList.push(em.Usuarios[0]);
			});
		}

		callback(uList);
	};

	const findEmailByCPF = async cpf => {
		const response = await UsuarioService.emailByCPF(cpf);
		callback(response.data.Usuario_list);
	};

	const atualizar = () => {
		if (isValid) {
			if (cpfCnpj !== 'undefined' && cpfCnpj.value) {
				const isCPF = temOnze(cpfCnpj.value);
				const isCNPJ = temQuatorze(cpfCnpj.value);
				if (isCPF) {
					findEmailByCPF(cpfCnpj.value);
				}
				if (isCNPJ) {
					findEmailByCNPJ(cpfCnpj.value);
				}
			}
		}
	};

	// Ações de Retorno

	const callback = retorno => {
		history.push('esqueci-meu-email-envia', retorno);
		dispatch(LoaderCreators.disableLoading());
	};

	// Formulários

	const initialValues = {
		cpfCnpj: ''
	};

	const validationSchema = Yup.object().shape({
		cpfCnpj: Yup.string().required(translate('campoObrigatorio'))
	});

	const { getFieldProps, handleSubmit, isValid, submitCount } = useFormik({
		initialValues,
		validationSchema,
		onSubmit: atualizar
	});

	const [
		cpfCnpj,
		metadataCpfCnpj
	] = getFieldProps('cpfCnpj', 'text');

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

					<Form onSubmit={handleSubmit}>
						<FormInput
							label={`@/..{translate('cpfcnpj')}:`}
							labelHelper={translate('informeCpfCnpj')}
							fullWidth
							name={cpfCnpj}
							error={checkError(submitCount, metadataCpfCnpj)}
						/>

						<Box display='flex'>
							<Button text={translate('Verificar')} type='submit' fullWidth />
						</Box>
					</Form>
				</Card>
			</ContentLogin>
			<Footer />
		</Background>
	);
}
