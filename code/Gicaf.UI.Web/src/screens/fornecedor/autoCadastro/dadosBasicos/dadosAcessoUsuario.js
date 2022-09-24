import React, { Fragment, useState, useEffect } from 'react';
import { Card, FormInput, Confirm } from 'react-axxiom';
import { CardHeader, CardContent, Box, InputAdornment, IconButton } from '@material-ui/core';
import { useDispatch } from 'react-redux';
import { Visibility, VisibilityOff } from '@material-ui/icons';
import { translate } from '@/locales';
import { checkError } from '@/utils/validation';
import { telefoneMask, celularMask, cpfMask, soNumero } from '@/utils/mascaras';
import { Creators as LoaderCreators } from '@/store/ducks/loader';
import UsuarioService from '@/services/usuario';
import theme from '@/theme';
import { Mensagem } from './style';
import { ENUM_ITEMS_ANALISE } from '@/utils/constants';
import Aprovacao from '@/screens/fornecedor/autoCadastro/aprovacao';

export default function DadosAcessoUsuario({
	formulario,
	mensagem,
	itensAnalise,
	setItensAnalise,
	comentarios,
	setComentarios,
	preCadastro,
	historicoEmpresa,
	user,
	disableEdit,
	statusEmpresa
}) {
	const dispatch = useDispatch();

	const [
		disabledDadosAcesso,
		setDisabledDadosAcesso
	] = useState(false);

	const [
		modalCpfJaCadastrado,
		setModalCpfJaCadastrado
	] = useState(false);

	const [
		showPassword,
		setShowPassword
	] = useState(false);
	const [
		showConfirmPassword,
		setShowConfirmPassword
	] = useState(false);

	const { submitCount, getFieldProps, setFieldValue, setFieldTouched } = formulario;

	const [
		nomeUsuario,
		metadataNomeUsuario
	] = getFieldProps('nomeUsuario', 'text');

	const [
		cpf,
		metadataCpf
	] = getFieldProps('cpf', 'text');

	const [
		telefone,
		metadataTelefone
	] = getFieldProps('telefone', 'text');

	const [
		celular
	] = getFieldProps('celular', 'text');

	const [
		cargoEmpresa,
		metadataCargoEmpresa
	] = getFieldProps('cargoEmpresa', 'text');

	const [
		email,
		metadataEmail
	] = getFieldProps('email', 'text');

	const [
		confirmarEmail,
		metadataConfirmarEmail
	] = getFieldProps('confirmarEmail', 'text');

	const [
		senha,
		metadataSenha
	] = getFieldProps('senha', 'text');

	const [
		confirmarSenha,
		metadataConfirmarSenha
	] = getFieldProps('confirmarSenha', 'text');

	// Efeito

	const buscarUsuarioByCpf = valor => {
		dispatch(LoaderCreators.setLoading('verificandoExistenciaAcessoSistema'));
		UsuarioService.findByCpf(valor)
			.then(response => {
				setDisabledDadosAcesso(false);
				if (response.data) {
					const user = response.data.Usuario_list[0];
					setFieldValue('idUsuario', user.id);
					setModalCpfJaCadastrado(true);
					dispatch(LoaderCreators.disableLoading());
				}
				dispatch(LoaderCreators.disableLoading());
			})
			.catch(() => dispatch(LoaderCreators.disableLoading()));
	};

	const updateCpf = event => {
		if (!event.target.value) {
			setDisabledDadosAcesso(false);
		}
		setFieldValue('cpf', soNumero(event.target.value));
		if (soNumero(event.target.value).length === 11) {
			buscarUsuarioByCpf(soNumero(event.target.value));
		}
	};

	const limparCPF = () => {
		setFieldValue('idUsuario', '');
		setFieldValue('cpf', '');
		setDisabledDadosAcesso(false);
		setModalCpfJaCadastrado(false);
	};

	const usuarioJaCadastrado = () => {
		setDisabledDadosAcesso(true);
		setModalCpfJaCadastrado(false);
	};

	return (
		<Box paddingTop={`${theme.spacing(1)}px`}>
			<Card>
				<Confirm
					open={modalCpfJaCadastrado}
					handleClose={limparCPF}
					handleSuccess={usuarioJaCadastrado}
					title={translate('confirmacao')}
					textButtonSuccess={translate('sim')}
					text={translate('cpfCadastrado')}
					textButtonCancel={translate('nao')}
					backgroundColorButtonCancel={theme.palette.secondary.main}
				/>
				<CardHeader
					title={translate('acessoAoSistema')}
					action={
						!preCadastro && (
							<Aprovacao
								itensAnalise={itensAnalise}
								setItensAnalise={setItensAnalise}
								comentarios={comentarios}
								setComentarios={setComentarios}
								tipoItem={ENUM_ITEMS_ANALISE.find(x => x.internalName === 'Acesso_Sistema').value}
								historicoEmpresa={historicoEmpresa}
								user={user}
								disableEdit={disableEdit}
								statusEmpresa={statusEmpresa}
							/>
						)
					}
				/>
				<CardContent>
					{mensagem && <Mensagem>{mensagem}</Mensagem>}

					<Box display='flex' flexDirection='row'>
						<Box width='20%' paddingRight={`${theme.spacing(1)}px`}>
							<FormInput
								label={`${translate('cpf')}:`}
								value={cpfMask(cpf.value)}
								onChange={event => {
									updateCpf(event);
								}}
								onFocus={() => setFieldTouched('cpf', true)}
								error={checkError(submitCount, metadataCpf)}
								required
								disabled={!preCadastro || disableEdit}
							/>
						</Box>

						<Box width='80%'>
							<FormInput
								label={`${translate('nomeUsuario')}:`}
								name={nomeUsuario}
								error={!disabledDadosAcesso && checkError(submitCount, metadataNomeUsuario)}
								disabled={!preCadastro || disabledDadosAcesso || disableEdit}
								required
							/>
						</Box>
					</Box>

					<Box display='flex' flexDirection='row'>
						<Box width='33%' paddingRight={`${theme.spacing(1)}px`}>
							<FormInput
								label={`${translate('telefone')}:`}
								value={telefoneMask(telefone.value)}
								onChange={event => setFieldValue('telefone', event.target.value)}
								onFocus={() => setFieldTouched('telefone', true)}
								error={!disabledDadosAcesso && checkError(submitCount, metadataTelefone)}
								disabled={!preCadastro || disabledDadosAcesso || disableEdit}
								required
							/>
						</Box>
						<Box width='33%' paddingRight={`${theme.spacing(1)}px`}>
							<FormInput
								label={`${translate('celular')}:`}
								value={celularMask(celular.value)}
								onChange={event => setFieldValue('celular', event.target.value)}
								onFocus={() => setFieldTouched('celular', true)}
								disabled={!preCadastro || disabledDadosAcesso || disableEdit}
							/>
						</Box>
						<Box width='34%'>
							<FormInput
								label={`${translate('cargoEmpresa')}:`}
								name={cargoEmpresa}
								error={!disabledDadosAcesso && checkError(submitCount, metadataCargoEmpresa)}
								disabled={!preCadastro || disabledDadosAcesso || disableEdit}
								required
							/>
						</Box>
					</Box>

					<Box display='flex' flexDirection='row'>
						<Box width={preCadastro ? '50%' : '100%'} paddingRight={`${theme.spacing(1)}px`}>
							<FormInput
								label={`${translate('email')}:`}
								name={email}
								error={!disabledDadosAcesso && checkError(submitCount, metadataEmail)}
								disabled={!preCadastro || disabledDadosAcesso || disableEdit}
								required
							/>
						</Box>
						{preCadastro && (
							<Fragment>
								<Box width='50%'>
									<FormInput
										label={`${translate('confirmarEmail')}:`}
										name={confirmarEmail}
										error={!disabledDadosAcesso && checkError(submitCount, metadataConfirmarEmail)}
										disabled={!preCadastro || disabledDadosAcesso || disableEdit}
										required
									/>
								</Box>
							</Fragment>
						)}
					</Box>
					{preCadastro && (
						<Fragment>
							<Box display='flex' flexDirection='row'>
								<Box width='50%' paddingRight={`${theme.spacing(1)}px`}>
									<FormInput
										label='Senha:'
										labelHelper={translate('validacaoSenha')}
										type={showPassword ? 'text' : 'password'}
										InputProps={{
											endAdornment: (
												<InputAdornment position='end'>
													<IconButton
														edge='end'
														aria-label='toggle password visibility'
														onClick={() => setShowPassword(!showPassword)}
													>
														{showPassword ? <VisibilityOff /> : <Visibility />}
													</IconButton>
												</InputAdornment>
											)
										}}
										name={senha}
										error={!disabledDadosAcesso && checkError(submitCount, metadataSenha)}
										disabled={!preCadastro || disabledDadosAcesso || disableEdit}
										required
									/>
								</Box>
								<Box width='50%'>
									<FormInput
										label={`${translate('confirmarSenha')}:`}
										name={confirmarSenha}
										type={showConfirmPassword ? 'text' : 'password'}
										InputProps={{
											endAdornment: (
												<InputAdornment position='end'>
													<IconButton
														edge='end'
														aria-label='toggle password visibility'
														onClick={() => setShowConfirmPassword(!showConfirmPassword)}
													>
														{showConfirmPassword ? <VisibilityOff /> : <Visibility />}
													</IconButton>
												</InputAdornment>
											)
										}}
										error={!disabledDadosAcesso && checkError(submitCount, metadataConfirmarSenha)}
										disabled={!preCadastro || disabledDadosAcesso || disableEdit}
										required
									/>
								</Box>
							</Box>
						</Fragment>
					)}
				</CardContent>
			</Card>
		</Box>
	);
}
