// migrar para react-axxiom
import React from 'react';
import styled from 'styled-components';
import { withStyles } from '@material-ui/core/styles';
import { Button } from 'react-axxiom';
import {
	Switch as SwitchUI,
	Box,
	Typograph,
	Dialog,
	DialogActions,
	IconButton,
	Typography,
	DialogContent,
	DialogTitle
} from '@material-ui/core';
import { Close } from '@material-ui/icons';

import Grid from '@material-ui/core/Grid';
import {
	ArrowDropUp,
	ArrowDropDown,
	FiberManualRecord,
	ArrowRight,
	ArrowLeft
} from '@material-ui/icons';

const colors = {
	white: '#FFFFFF',
	green: '#336666',
	textSecondary: '#66788A',
	red: '#ED4740',
	border: '#DFE3E8',
	tableHead: '#E9ECEF',
	disabledBackground: '#E0E0E0'
};

const BotaoAtivo = styled.button`
	background-color: #1e90ff;
	color: #fafafa;
	border: none;
`;
const BotaoInativo = styled.button`
	background-color: #808080;
	color: #fafafa;
	border: none;
`;

const RiscoBaixo = styled(ArrowDropUp)`
	color: green;
`;

const RiscoAlto = styled(ArrowDropDown)`
	color: red;
`;

const RiscoMedioEsquerda = styled(ArrowLeft)`
	color: orange;
`;

const RiscoMedioDireita = styled(ArrowRight)`
	color: orange;
`;

const SemDados = styled(FiberManualRecord)`
	color: gray;
	font-size:18px !important;
`;

const LabelRiscoBaixo = styled(Typography)`
	color: green !important;
`;

const LabelRiscoAlto = styled(Typography)`
	color: red !important;
`;

const LabelRiscoMedio = styled(Typography)`
	color: orange !important;
`;

const LabelSemDados = styled(Typography)`
	color: gray !important;
`;

const SwitchUIStyled = withStyles(theme => ({
	switchBase: {
		color: theme.palette.grey,
		'&$checked': {
			color: colors.green,
			'& + $track': {
				backgroundColor: theme.palette.primary.main,
				borderColor: theme.palette.primary.main
			}
		}
	},
	track: {
		border: `1px solid ${theme.palette.grey[500]} !important`,
		backgroundColor: theme.palette.common.grey
	},
	checked: {}
}))(SwitchUI);
function Switch({ label, checked, onChange, checkedName }) {
	return (
		<Box>
			<Box>{label}</Box>
			<Grid component='label' container alignItems='center' spacing={1}>
				{/* <Grid item>Inativo</Grid> */}
				<Grid item>
					<SwitchUIStyled checked={checked} onChange={onChange} />
				</Grid>
				<Grid item>{checkedName}</Grid>
			</Grid>
		</Box>
	);
}

function Status({ label, color = colors.white, ativo = false }) {
	return (
		<BoxStatusContainer>
			<BoxStatus color={color} backgroundColor={ativo ? '#1e90ff' : '#808080'}>
				<span>{label}</span>
			</BoxStatus>
		</BoxStatusContainer>
	);
}
const TitleModal = styled(DialogTitle)`
  padding: 8px !important
`;

const TextTitleModal = styled(Typography)`
  line-height: 30px !important
`;

const ContentModal = styled(DialogContent)`
  padding: 8px !important
`;
function Modal({
	open,
	handleClose,
	title,
	componentSubtitle,
	children,
	onClickButton,
	textButton,
	titleFechar,
	maxWidth,
	fullWidth,
	exibirBotao = true
}) {
	return (
		<Dialog open={open} onClose={handleClose} maxWidth={maxWidth} fullWidth={fullWidth}>
			<TitleModal>
				<Box display='flex' flexDirection='row'>
					<Box flexGrow={1}>
						<TextTitleModal title={title} variant='h5'>
							{title}
						</TextTitleModal>
					</Box>
					<Box alignSelf='flex-end'>
						<IconButton size='small' title={titleFechar} onClick={handleClose}>
							<Close />
						</IconButton>
					</Box>
				</Box>
				{componentSubtitle && (
					<Box display='flex' flexDirection='row'>
						<Box flexGrow={1}>{componentSubtitle}</Box>
					</Box>
				)}
			</TitleModal>
			{children && <ContentModal>{children}</ContentModal>}
			<DialogActions>
				{exibirBotao && <Button onClick={onClickButton} color='primary' text={textButton} />}
			</DialogActions>
		</Dialog>
	);
}

const BoxStatus = styled(Box)`
	color: ${props => props.color} !important
	background-color: ${props => props.backgroundColor} !important
	border: none;
	border-radius: 3px;
	width: 65px;
	text-align: center;
	font-size: 14px;

`;
const BoxStatusContainer = styled(Box)`
		display: table-cell;
    vertical-align: middle;
    align-items: center;
`;
export {
	BotaoAtivo,
	BotaoInativo,
	Switch,
	Status,
	RiscoBaixo,
	RiscoAlto,
	SemDados,
	RiscoMedioEsquerda,
	RiscoMedioDireita,
	LabelRiscoBaixo,
	LabelRiscoMedio,
	LabelRiscoAlto,
	LabelSemDados,
	Modal
};
