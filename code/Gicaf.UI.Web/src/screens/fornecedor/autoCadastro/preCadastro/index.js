import React, { useState, useEffect, Fragment } from 'react';
import { Box } from '@material-ui/core';
import useReactRouter from 'use-react-router';
import { TopbarExterna, Title } from '@/layout';
import DadosBasicos from '@/screens/fornecedor/autoCadastro/dadosBasicos';
import paths from '@/utils/paths';
import theme from '@/theme';
import { Button } from 'react-axxiom';
import { translate } from '@/locales';
import { COMANDO_CADASTRO_FORNECEDOR } from '@/utils/constants';
import { useSnackbar } from 'notistack';
import { snackError, snackWarning } from '@/utils/snack';

export default function PreCadastro() {
	const { history } = useReactRouter();
	const { enqueueSnackbar, closeSnackbar } = useSnackbar();

	const [
		acao,
		setAcao
	] = useState(null);

	const [
		dadosBasicosIsValid,
		setDadosBasicosIsValid
	] = useState(false);

	const callbackError = mensagem => {
		enqueueSnackbar('', snackError(mensagem, closeSnackbar));
	};
	const callbackWarning = mensagem => {
		enqueueSnackbar('', snackWarning(mensagem, closeSnackbar));
	};

	return (
		<Box>
			<TopbarExterna />
			<Box padding='16px' marginTop='8px'>
				<Title text={paths.getTitle(paths.getCurrentPath(history))} />
				<DadosBasicos
					preCadastro
					acao={acao}
					setAcao={setAcao}
					setDadosBasicosIsValid={setDadosBasicosIsValid}
				/>
				<Box display='flex' justifyContent='flex-end' paddingTop={`@/..{theme.spacing(1)}px`}>
					<Button
						text={translate('cancelar')}
						backgroundColor={theme.palette.secondary.main}
						onClick={() => setAcao(COMANDO_CADASTRO_FORNECEDOR.limparFormulario)}
					/>
					<Button
						text={translate('salvar')}
						onClick={() => {
							if (!dadosBasicosIsValid) {
								callbackWarning(translate('possuemCamposPreenchidosIncorretamente'));
							}
							setAcao(COMANDO_CADASTRO_FORNECEDOR.criarCadastro);
						}}
						margin={`0px 0px 0px @/..{theme.spacing(1)}px`}
					/>
				</Box>
			</Box>
		</Box>
	);
}
