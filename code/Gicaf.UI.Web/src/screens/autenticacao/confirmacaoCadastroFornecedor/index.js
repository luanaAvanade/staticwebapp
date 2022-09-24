import React, { useState, useEffect } from 'react';
import { Box, Typography } from '@material-ui/core';
import { useDispatch } from 'react-redux';
import useReactRouter from 'use-react-router';
import QueryString from 'query-string';
import { Card, Button } from 'react-axxiom';
import { CemigVerde } from '@/assets';
import { Image, ContentLogin, Background, BoxContentText } from './style';
import { Footer } from '@/layout';
import { translate, translateWithHtml } from '@/locales';
import theme from '@/theme';
import LoginService from '@/services/login';
import { Creators as LoaderCreators } from '@/store/ducks/loader';

export default function ConfirmacaoCadastroFornecedor() {
	const { history, location } = useReactRouter();
	const queryString = QueryString.parse(location.search);
	const dispatch = useDispatch();
	const minheight = 260;
	const width = 350;

	// Estado Local

	const [
		emailConfirmado,
		setEmailConfirmado
	] = useState(false);

	const [
		confirma,
		setConfirma
	] = useState(false);

	// Efeito Inicial

	useEffect(() => {
		dispatch(LoaderCreators.setLoading());
		LoginService.emailConfirmationValidation({
			UserName: queryString.userName,
			Token: queryString.token
		})
			.then(response => {
				setEmailConfirmado(response.succeeded);
				setConfirma(true);
				dispatch(LoaderCreators.disableLoading());
			})
			.catch(erro => console.log(JSON.stringify(erro)));
		return () => {};
	}, []);

	// Ação da Tela

	const irParaLogin = () => {
		history.push('login');
	};

	return (
		<Background>
			{confirma && (
				<ContentLogin
					top={`calc( 50% - (@/..{minheight}px/2) - 60px)`}
					left={`calc( 50% - (@/..{width}px/2))`}
				>
					<Card width={`@/..{width}px`} minheight={`@/..{minheight}px`} padding={theme.spacing(2)}>
						<Box display='flex' justifyContent='space-around'>
							<Image src={CemigVerde} />
						</Box>
						<BoxContentText>
							<Typography variant='h6'>
								<b>
									{emailConfirmado ? (
										translate('emailConfirmadoComSucesso')
									) : (
										translateWithHtml('falhaCadastroEntreContatoAdministrador')
									)}
								</b>
							</Typography>
						</BoxContentText>
						<BoxContentText>
							<Typography variant='h6'>
								{emailConfirmado ? translateWithHtml('paraSerRedeirecionadoClickNoBotao') : ''}
							</Typography>
						</BoxContentText>
						<Box display='flex'>
							<Button text={translate('ok')} fullWidth onClick={irParaLogin} />
						</Box>
					</Card>
				</ContentLogin>
			)}
			<Footer />
		</Background>
	);
}
