import React, { Fragment } from 'react';
import { Box } from '@material-ui/core';
import { Button } from 'react-axxiom';
import theme from '@/theme';
import { translate } from '@/locales';
import { COMANDO_CADASTRO_FORNECEDOR } from '@/utils/constants';

export function BotoesCadastro({
	setAcao,
	enviarCadastro,
	empresaFindById,
	disableEdit,
	finalizarAnalise,
	statusEmpresa
}) {
	return (
		<Box display='flex' justifyContent='flex-end' paddingTop={`@/..{theme.spacing(1)}px`}>
			{!disableEdit && (
				<Fragment>
					<Button
						text={translate('cancelar')}
						backgroundColor={theme.palette.secondary.main}
						onClick={() => empresaFindById()}
					/>
					<Button
						text={translate('salvar')}
						onClick={() => setAcao(COMANDO_CADASTRO_FORNECEDOR.criarCadastro)}
						margin={`0px 0px 0px ${theme.spacing(1)}px`}
					/>
					{![
						'Em_Analise',
						'Aprovado',
						'Aprovado_Ressalvas'
					].includes(statusEmpresa) && (
						<Button
							text={translate('enviarCadastro')}
							onClick={() => enviarCadastro()}
							margin={`0px 0px 0px ${theme.spacing(1)}px`}
						/>
					)}
					{[
						'Em_Analise'
					].includes(statusEmpresa) && (
						<Button
							text={translate('finalizarAnalise')}
							onClick={() => finalizarAnalise()}
							margin={`0px 0px 0px ${theme.spacing(1)}px`}
						/>
					)}
				</Fragment>
			)}
		</Box>
	);
}
