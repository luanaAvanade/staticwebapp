import React from 'react';
import { FormInput, Card } from 'react-axxiom';
import { Box, CardContent, CardHeader } from '@material-ui/core';
import { translate } from '@/locales';
import { checkError } from '@/utils/validation';
import theme from '@/theme';
import { ENUM_ITEMS_ANALISE } from '@/utils/constants';
import Aprovacao from '@/screens/fornecedor/autoCadastro/aprovacao';

export default function ContatoCliente({
	formulario,
	itensAnalise,
	setItensAnalise,
	comentarios,
	setComentarios,
	preCadastro,
	historicoEmpresa,
	user,
	disableEdit,
	setChanged,
	statusEmpresa
}) {
	const { submitCount, getFieldProps, setFieldValue } = formulario;

	const [
		nomeContatoCemig,
		metadataNomeContatoCemig
	] = getFieldProps('nomeContatoCemig', 'text');

	const [
		emailContatoCemig,
		metadataEmailContatoCemig
	] = getFieldProps('emailContatoCemig', 'text');

	return (
		<Box paddingTop={`${theme.spacing(1)}px`}>
			<Card>
				<CardHeader
					title={translate('contatoCliente')}
					action={
						!preCadastro && (
							<Aprovacao
								itensAnalise={itensAnalise}
								setItensAnalise={setItensAnalise}
								comentarios={comentarios}
								setComentarios={setComentarios}
								tipoItem={ENUM_ITEMS_ANALISE.find(x => x.internalName === 'Contato_Cliente').value}
								historicoEmpresa={historicoEmpresa}
								user={user}
								disableEdit={disableEdit}
								statusEmpresa={statusEmpresa}
							/>
						)
					}
				/>
				<CardContent>
					<Box display='flex' flexDirection='row'>
						<Box width='50%' paddingRight={`${theme.spacing(1)}px`}>
							<FormInput
								label={`${translate('nomeContatoCemig')}:`}
								name={nomeContatoCemig}
								error={checkError(submitCount, metadataNomeContatoCemig)}
								disabled={disableEdit}
								required
							/>
						</Box>
						<Box width='50%' paddingRight={`${theme.spacing(1)}px`}>
							<FormInput
								label={`${translate('emailContatoCemig')}:`}
								name={emailContatoCemig}
								error={checkError(submitCount, metadataEmailContatoCemig)}
								disabled={disableEdit}
								required
							/>
						</Box>
					</Box>
				</CardContent>
			</Card>
		</Box>
	);
}
