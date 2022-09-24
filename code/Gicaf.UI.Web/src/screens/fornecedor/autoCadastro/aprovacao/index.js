import React, { useState, useEffect, Fragment } from 'react';
import { Modal } from 'react-axxiom';
import { IconButton, Box, TextField } from '@material-ui/core';
import {
	MoreVert,
	ThumbUp,
	ThumbDown,
	ThumbsUpDown,
	Comment,
	History,
	Spellcheck
} from '@material-ui/icons';
import { translate } from '@/locales';
import { ENUM_STATUS_ANALISE } from '@/utils/constants';
import ObjectHelper from '@/utils/objectHelper';
import { useSnackbar } from 'notistack';
import { snackSuccess, snackError, snackWarning } from '@/utils/snack';
import Historico from './historico';
import Comentarios from './comentarios';

export default function Aprovacao({
	itensAnalise,
	setItensAnalise,
	comentarios,
	setComentarios,
	tipoItem,
	arquivoId,
	historicoEmpresa,
	user,
	disableEdit,
	statusEmpresa
}) {
	const { enqueueSnackbar, closeSnackbar } = useSnackbar();

	// Estados locais
	const [
		status,
		setStatus
	] = useState(null);

	const [
		aprovado,
		setAprovado
	] = useState(1);

	const [
		aprovadoRessalvas,
		setAprovadoRessalvas
	] = useState(1);

	const [
		reprovado,
		setReprovado
	] = useState(1);

	const [
		insertComment,
		setInsertComment
	] = useState(false);

	const [
		comentario,
		setComentario
	] = useState(null);

	const [
		itensAnaliseComentario,
		setItensAnaliseComentario
	] = useState(null);

	const [
		showHistory,
		setShowHistory
	] = useState(false);

	// Efeito Inicial

	useEffect(() => {
		arquivoId = arquivoId !== undefined ? arquivoId : null;
		setStatusAtual(itensAnalise);
		return () => {
			setAprovado(1);
			setAprovadoRessalvas(1);
			setReprovado(1);
		};
	}, []);

	function setStatusAtual(itensAnaliseStatus) {
		if (user.perfilAnalista && disableEdit && itensAnalise === []) {
			setAprovado(opaco);
			setAprovadoRessalvas(opaco);
			setReprovado(opaco);
			return;
		}
		arquivoId = arquivoId !== undefined ? arquivoId : null;
		const indexItem = itensAnaliseStatus.findIndex(
			x => x.TipoItem === tipoItem && x.ArquivoId === arquivoId
		);
		if (indexItem >= 0) {
			const statusItem = itensAnaliseStatus[indexItem].Status;
			switch (statusItem) {
				case ENUM_STATUS_ANALISE.find(x => x.internalName === 'Aprovado').value:
					setAprovado(1);
					setAprovadoRessalvas(opaco);
					setReprovado(opaco);
					setStatus('Aprovado');
					break;
				case ENUM_STATUS_ANALISE.find(x => x.internalName === 'Aprovado_Ressalvas').value:
					setAprovado(opaco);
					setAprovadoRessalvas(1);
					setReprovado(opaco);
					setStatus('Aprovado_Ressalvas');
					break;
				case ENUM_STATUS_ANALISE.find(x => x.internalName === 'Reprovado').value:
					setAprovadoRessalvas(opaco);
					setAprovado(opaco);
					setReprovado(1);
					setStatus('Reprovado');
					break;
				default:
					break;
			}
		}
	}

	const aprovar = () => {
		const itensAnaliseTemp = ObjectHelper.clone(itensAnalise);
		const itemAnalise = {
			TipoItem: tipoItem,
			AutorId: user.id,
			Status: ENUM_STATUS_ANALISE.find(x => x.internalName === 'Aprovado').value,
			ArquivoId: arquivoId ? arquivoId : null
		};

		const indexItem = itensAnaliseTemp.findIndex(
			x => x.TipoItem === tipoItem && x.ArquivoId == arquivoId
		);

		if (indexItem >= 0) {
			itemAnalise.Id = itensAnaliseTemp[indexItem].Id;
			itensAnaliseTemp[indexItem] = itemAnalise;
		} else {
			itensAnaliseTemp.push(itemAnalise);
		}
		setItensAnalise(itensAnaliseTemp);
		setStatusAtual(itensAnaliseTemp);
	};

	const aprovarRessalvas = () => {
		const itensAnaliseTemp = ObjectHelper.clone(itensAnalise);
		const itemAnalise = {
			TipoItem: tipoItem,
			AutorId: user.id,
			Status: ENUM_STATUS_ANALISE.find(x => x.internalName === 'Aprovado_Ressalvas').value,
			ArquivoId: arquivoId ? arquivoId : null
		};

		const indexItem = itensAnaliseTemp.findIndex(
			x => x.TipoItem === tipoItem && x.ArquivoId == arquivoId
		);

		if (indexItem >= 0) {
			setComentario(itensAnaliseTemp[indexItem].Justificativa);
			itemAnalise.Id = itensAnaliseTemp[indexItem].Id;
			itensAnaliseTemp[indexItem] = itemAnalise;
		} else {
			itensAnaliseTemp.push(itemAnalise);
		}
		setItensAnaliseComentario(itensAnaliseTemp);
		setInsertComment(true);
	};

	const reprovar = () => {
		const itensAnaliseTemp = ObjectHelper.clone(itensAnalise);
		const itemAnalise = {
			TipoItem: tipoItem,
			AutorId: user.id,
			Status: ENUM_STATUS_ANALISE.find(x => x.internalName === 'Reprovado').value,
			ArquivoId: arquivoId ? arquivoId : null
		};

		const indexItem = itensAnaliseTemp.findIndex(
			x => x.TipoItem === tipoItem && x.ArquivoId == arquivoId
		);

		if (indexItem >= 0) {
			setComentario(itensAnaliseTemp[indexItem].Justificativa);
			itemAnalise.Id = itensAnaliseTemp[indexItem].Id;
			itensAnaliseTemp[indexItem] = itemAnalise;
		} else {
			itensAnaliseTemp.push(itemAnalise);
		}
		setItensAnaliseComentario(itensAnaliseTemp);
		setInsertComment(true);
	};

	const salvarComentario = () => {
		if (comentario !== null) {
			if (itensAnaliseComentario !== null) {
				const itensAnaliseTemp = ObjectHelper.clone(itensAnaliseComentario);
				const indexItem = itensAnaliseTemp.findIndex(
					x => x.TipoItem === tipoItem && x.ArquivoId == arquivoId
				);
				itensAnaliseTemp[indexItem].Justificativa = comentario;
				setItensAnalise(itensAnaliseTemp);
				setStatusAtual(itensAnaliseTemp);
			} else {
				const comentariosTemp = ObjectHelper.clone(comentarios);
				const data = new Date();
				const comentarioTemp = {
					Local: tipoItem,
					Coment: comentario,
					UsuarioId: user.id,
					DataCriacao: data,
					Usuario: {
						Nome: user.nome
					}
				};
				comentariosTemp.push(comentarioTemp);
				setComentarios(comentariosTemp);
				callbackWarning(translate('comentariosSalvoJuntamenteCadastro'));
			}
		}
		closeModal();
	};

	const closeModal = () => {
		if (insertComment != null) {
			if (itensAnaliseComentario !== null && comentario === null) {
				callbackError(translate('statusAprovacaoNaoAlterado'), null);
			}
			setComentario(null);
			setInsertComment(false);
			setItensAnaliseComentario(null);
			setShowHistory(false);
		} else {
			setShowHistory(false);
		}
	};

	const callbackError = (mensagem, response) => {
		enqueueSnackbar('', snackError(mensagem, closeSnackbar));
	};

	const callbackWarning = mensagem => {
		enqueueSnackbar('', snackWarning(mensagem, closeSnackbar));
	};

	const margin = '3px';

	const padding = '2px';

	const opaco = 0.3;

	return (
		<Fragment>
			{user &&
			user.perfilAnalista && (
				<Fragment>
					<IconButton
						aria-label={translate('aprovar')}
						onClick={() => aprovar()}
						style={{
							margin: margin,
							padding: padding,
							border: `1px solid rgb(0,128,0,@/..{aprovado})`
						}}
						disabled={!user.perfilAnalista || disableEdit}
					>
						<ThumbUp
							fontSize='small'
							style={{
								color: `rgb(0,128,0,@/..{aprovado})`,
								padding: '2px'
							}}
						/>
					</IconButton>
					<IconButton
						aria-label={translate('reprovar')}
						onClick={() => reprovar()}
						style={{
							margin: margin,
							padding: padding,
							border: `1px solid rgb(255,0,0,@/..{reprovado})`
						}}
						disabled={!user.perfilAnalista || disableEdit}
					>
						<ThumbDown
							fontSize='small'
							style={{
								padding: '2px',
								color: `rgb(255,0,0,@/..{reprovado})`
							}}
						/>
					</IconButton>
					<IconButton
						aria-label={translate('aprovarRessalvas')}
						onClick={() => aprovarRessalvas()}
						style={{
							margin: margin,
							padding: padding,
							border: `1px solid rgb(33,195,80,@/..{aprovadoRessalvas})`
						}}
						disabled={!user.perfilAnalista || disableEdit}
					>
						<Spellcheck
							color='secondary'
							fontSize='small'
							style={{
								color: `rgb(33,195,80,@/..{aprovadoRessalvas})`,
								padding: '2px'
							}}
						/>
					</IconButton>
				</Fragment>
			)}
			{user &&
			(!user.perfilAnalista &&
				statusEmpresa !== ENUM_STATUS_ANALISE.find(x => x.internalName === 'Em_Analise').value &&
				status !== null) && (
				<Fragment>
					<IconButton
						aria-label={translate('aprovar')}
						onClick={() => aprovar()}
						style={{
							margin: margin,
							padding: padding,
							border: `1px solid rgb(0,128,0,@/..{aprovado === 1 || aprovadoRessalvas === 1
								? 1
								: aprovado})`
						}}
						disabled={!user.perfilAnalista || disableEdit}
					>
						<ThumbUp
							fontSize='small'
							style={{
								color: `rgb(0,128,0,@/..{aprovado === 1 || aprovadoRessalvas === 1 ? 1 : aprovado})`,
								padding: '2px'
							}}
						/>
					</IconButton>
					<IconButton
						aria-label={translate('reprovar')}
						onClick={() => reprovar()}
						style={{
							margin: margin,
							padding: padding,
							border: `1px solid rgb(255,0,0,@/..{reprovado})`
						}}
						disabled={!user.perfilAnalista || disableEdit}
					>
						<ThumbDown
							fontSize='small'
							style={{
								padding: '2px',
								color: `rgb(255,0,0,@/..{reprovado})`
							}}
						/>
					</IconButton>
				</Fragment>
			)}
			<IconButton
				aria-label={translate('comentar')}
				onClick={() => setInsertComment(true)}
				style={{
					margin: margin,
					padding: padding,
					border: `1px solid rgb(172,179,17,@/..{disableEdit ? 0.5 : 1})`
				}}
				disabled={disableEdit}
			>
				<Comment
					color='secondary'
					fontSize='small'
					style={{
						color: `rgb(172,179,17,@/..{disableEdit ? 0.5 : 1})`,
						padding: '2px'
					}}
				/>
			</IconButton>
			<IconButton
				aria-label={translate('historico')}
				onClick={() => setShowHistory(true)}
				style={{
					margin: margin,
					padding: padding,
					border: `1px solid rgb(0,0,255, 1)`
				}}
			>
				<History
					color='secondary'
					fontSize='small'
					style={{
						color: `rgb(0,0,255,1)`,
						padding: '2px'
					}}
				/>
			</IconButton>

			<Modal
				open={insertComment}
				handleClose={() => closeModal()}
				onClickButton={() => salvarComentario()}
				title={
					itensAnaliseComentario !== null ? translate('justificativa') : translate('comentario')
				}
				textButton={
					itensAnaliseComentario !== null ? translate('justificar') : translate('comentar')
				}
				// maxWidth='sm'
				fullWidth
			>
				<Box>
					<TextField
						id='outlined-textarea'
						value={comentario}
						multiline
						rows='5'
						style={{
							width: '100%'
						}}
						margin='normal'
						variant='outlined'
						onChange={event => setComentario(event.target.value)}
						placeholder={translate('insiraComentario')}
					/>
				</Box>
			</Modal>
			<Modal
				open={showHistory}
				handleClose={() => closeModal()}
				onClickButton={() => closeModal()}
				title={translate('historico')}
				textButton={translate('fechar')}
				maxWidth='sm'
				fullWidth
			>
				<Comentarios
					comentarios={
						comentarios ? (
							comentarios.filter(x => x.Local === tipoItem).sort((a, b) => {
								{
									if (new Date(a.DataCriacao) > new Date(b.DataCriacao)) {
										return -1;
									} else {
										return 1;
									}
								}
							})
						) : (
							comentarios
						)
					}
				/>
				{/* <Historico historicoItens={historicoEmpresa}> </Historico> */}
			</Modal>
		</Fragment>
	);
}
