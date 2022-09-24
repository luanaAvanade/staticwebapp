import React, { Fragment, useState, useEffect } from 'react';
import { Table, TableHead, TableRow, TableCell } from 'react-axxiom';
import {
	Box,
	TableBody,
	IconButton,
	Icon,
	Card,
	CardContent,
	CardHeader,
	List,
	ListItem,
	ListItemIcon,
	ListItemText,
	TextField,
	InputBase,
	Typography
} from '@material-ui/core';
import theme from '@/theme';
import { translate } from '@/locales';
import { stableSort, getSorting } from '@/utils/list';
import {
	ArrowDropDown,
	ArrowLeft,
	ArrowRight,
	ArrowDropUp,
	FiberManualRecord,
	CheckBoxOutlineBlank,
	OfflinePin
} from '@material-ui/icons';
import {
	CabecalhoHorizontal,
	SugestaoAprovarComRessalvas,
	SugestaoAprovar,
	SugestaoReprovar
} from './style';
import {
	SEM_AVALIACAO,
	ESTAVEL,
	INSTAVEL,
	APROVAR,
	APROVAR_COM_RESSALVAS,
	REPROVAR
} from '@/utils/constants';
import NumberFormat from 'react-number-format';
import { HORIZONTAL_COLUMNS_QUALIFICACAO } from '@/utils/constants';

export default function ResultadoAnalise({
	empresa,
	montarCabecalho,
	qualificacaoLista,
	indicadoresForaRefMercado,
	indicadoresForaRefMercadoRefCemig,
	inserirIcone
}) {
	const [
		qualificacaoEmpresa,
		setQualificacaoEmpresa
	] = useState([]);

	const [
		classificacao,
		setClassificacao
	] = useState();

	const [
		tendenciaPrimeira,
		setTendenciaPrimeira
	] = useState(SEM_AVALIACAO);

	const [
		tendenciaSegunda,
		setTendenciaSegunda
	] = useState(SEM_AVALIACAO);

	const [
		sugestao,
		setSugestao
	] = useState();

	useEffect(
		() => {
			if (qualificacaoLista.find(qlf => qlf.Id == empresa.Id)) {
				setQualificacaoEmpresa(
					qualificacaoLista.find(qlf => qlf.Id == empresa.Id).CalculoRiscoLista
				);
				setClassificacao(empresa.CalculoRiscoLista[0].ClassificacaoFase1);
				avaliaTendencia(empresa.CalculoRiscoLista);
				avaliaSugestao(qualificacaoLista.find(qlf => qlf.Id == empresa.Id).CalculoRiscoLista);
				return () => {};
			}
		},
		[
			qualificacaoLista
		]
	);

	const avaliaSugestao = qualificacaoEmpresa => {
		if (qualificacaoEmpresa[0].ClassificacaoFase1 <= 1) {
			if (
				qualificacaoEmpresa[1].ClassificacaoFase1 <= 1 &&
				qualificacaoEmpresa[2].ClassificacaoFase1 <= 1
			) {
				setSugestao(APROVAR);
			}
			if (
				qualificacaoEmpresa[1].ClassificacaoFase1 > 1 ||
				qualificacaoEmpresa[2].ClassificacaoFase1 > 1
			) {
				if (algumIndForaRef()) {
					if (algumIndForaFaixaBaseAtual()) {
						setSugestao(REPROVAR);
					} else {
						setSugestao(APROVAR_COM_RESSALVAS);
					}
				} else {
					setSugestao(APROVAR_COM_RESSALVAS);
				}
			}
		}
		if (qualificacaoEmpresa[0].ClassificacaoFase1 == 2) {
			if (algumIndForaRef()) {
				if (algumIndForaFaixaBaseAtual()) {
					setSugestao(REPROVAR);
				} else {
					setSugestao(APROVAR_COM_RESSALVAS);
				}
			} else {
				setSugestao(APROVAR_COM_RESSALVAS);
			}
		}
		if (qualificacaoEmpresa[0].ClassificacaoFase1 == 3) {
			if (algumIndForaRef()) {
				if (algumIndForaFaixaBaseAtual()) {
					setSugestao(REPROVAR);
				} else {
					setSugestao(APROVAR_COM_RESSALVAS);
				}
			} else {
				setSugestao(APROVAR_COM_RESSALVAS);
			}
		}
	};

	const algumIndForaRef = () => {
		if (indicadoresForaRefMercado.length > 0) {
			return true;
		} else {
			return false;
		}
	};

	const algumIndForaFaixaBaseAtual = () => {
		if (indicadoresForaRefMercadoRefCemig.length > 0) {
			return true;
		} else {
			return false;
		}
	};

	const avaliaTendencia = value => {
		let tendencia1 = SEM_AVALIACAO;
		let tendencia2 = SEM_AVALIACAO;

		if (value[0].ClassificacaoFase1 == value[1].ClassificacaoFase1) {
			tendencia1 = ESTAVEL;
		}
		if (value[0].ClassificacaoFase1 != value[1].ClassificacaoFase1) {
			tendencia1 = INSTAVEL;
		}
		if (value[1].ClassificacaoFase1 == value[2].ClassificacaoFase1) {
			tendencia2 = ESTAVEL;
		}
		if (value[1].ClassificacaoFase1 != value[2].ClassificacaoFase1) {
			tendencia2 = INSTAVEL;
		}

		setTendenciaPrimeira(tendencia1);
		setTendenciaSegunda(tendencia2);
	};

	const montarCabecalhoTendencia = () => {
		let columns = [];

		const anoAtual = new Date().getFullYear();
		const anoUltimo = anoAtual - 1;
		const anoPenultimo = anoAtual - 2;
		const anoAntepenultimo = anoAtual - 3;

		columns.push({
			id: anoUltimo,
			label: anoUltimo + ' - ' + anoPenultimo,
			width: '50%',
			align: 'center'
		});
		columns.push({
			id: anoPenultimo,
			label: anoPenultimo + ' - ' + anoAntepenultimo,
			width: '50%',
			align: 'center'
		});
		return columns;
	};

	return (
		<Box paddingTop={`@/..{theme.spacing(1)}px`}>
			<Card style={{ marginTop: 8 }}>
				<CardContent>
					<Box display='flex' flexDirection='row'>
						<Box width='50%' paddingRight={`@/..{theme.spacing(1)}px`}>
							<Card>
								<CardHeader title={translate('riscoEstruturaFinanceira')} />
								<CardContent>{inserirIcone(classificacao, true, 'flex-start')}</CardContent>
							</Card>
							<Card>
								<CardHeader title={translate('tendenciaRiscoEstruturaFinanceira')} />
								<CardContent>
									<Table>
										<TableHead columns={montarCabecalhoTendencia()} />
										<TableRow>
											<TableCell align='center' label={tendenciaPrimeira} />
											<TableCell align='center' label={tendenciaSegunda} />
										</TableRow>
									</Table>
								</CardContent>
							</Card>
						</Box>
						<Box width='50%' paddingRight={`@/..{theme.spacing(1)}px`}>
							<Card>
								<CardHeader title={translate('sugestaoCadastro')} />
								<CardContent>
									<List>
										<ListItem>
											<ListItemIcon>
												{sugestao == APROVAR ? <SugestaoAprovar /> : <FiberManualRecord />}
											</ListItemIcon>
											<ListItemText primary='APROVAR' />
										</ListItem>
										<ListItem>
											<ListItemIcon>
												{sugestao == APROVAR_COM_RESSALVAS ? (
													<SugestaoAprovarComRessalvas />
												) : (
													<FiberManualRecord />
												)}
											</ListItemIcon>
											<ListItemText primary='APROVAR COM RESSALVAS' />
										</ListItem>
										<ListItem>
											<ListItemIcon>
												{sugestao == REPROVAR ? <SugestaoReprovar /> : <FiberManualRecord />}
											</ListItemIcon>
											<ListItemText primary='REPROVAR' />
										</ListItem>
									</List>
								</CardContent>
							</Card>
						</Box>
					</Box>
					<Box display='flex' flexDirection='row'>
						<Box width='100%' paddingRight={`@/..{theme.spacing(1)}px`}>
							<Card>
								<CardHeader title={translate('indicadoresForaEspecificadoRef')} />
								<CardContent>
									{indicadoresForaRefMercado.length > 0 && (
										<Table>
											<TableHead columns={montarCabecalho('Referência', 1)} />
											{HORIZONTAL_COLUMNS_QUALIFICACAO.map(cabecalhoColumn => {
												if (
													indicadoresForaRefMercado.find(i => i.campo == cabecalhoColumn.codigo)
												) {
													return (
														<TableRow>
															<TableCell
																label={
																	<CabecalhoHorizontal
																		paddingLeft={cabecalhoColumn.paddingLeft}
																		negrito={cabecalhoColumn.negrito}
																	>
																		{cabecalhoColumn.label}
																	</CabecalhoHorizontal>
																}
															/>

															{qualificacaoEmpresa.map((qlf, index) => {
																return (
																	<TableCell
																		key={index}
																		label={
																			<Box display='flex' flexDirection='row'>
																				<NumberFormat
																					value={qlf[cabecalhoColumn.codigo]}
																					displayType='text'
																					thousandSeparator='.'
																					decimalSeparator=','
																					decimalScale='2'
																					fixedDecimalScale
																				/>
																				{inserirIcone(qlf['Risco' + cabecalhoColumn.codigo], false)}
																			</Box>
																		}
																	/>
																);
															})}
														</TableRow>
													);
												}
											})}
										</Table>
									)}
								</CardContent>
							</Card>
						</Box>
					</Box>
					<Box display='flex' flexDirection='row'>
						<Box width='100%' paddingRight={`@/..{theme.spacing(1)}px`}>
							<Card>
								<CardHeader title={translate('indicadoresForaEspecificadoRefMercUnivCemig')} />
								<CardContent>
									{indicadoresForaRefMercadoRefCemig.length > 0 && (
										<Table>
											<TableHead columns={montarCabecalho('Referência', 1)} />
											{HORIZONTAL_COLUMNS_QUALIFICACAO.map(cabecalhoColumn => {
												if (
													indicadoresForaRefMercadoRefCemig.find(
														i => i.campo == cabecalhoColumn.codigo
													)
												) {
													return (
														<TableRow>
															<TableCell
																label={
																	<CabecalhoHorizontal
																		paddingLeft={cabecalhoColumn.paddingLeft}
																		negrito={cabecalhoColumn.negrito}
																	>
																		{cabecalhoColumn.label}
																	</CabecalhoHorizontal>
																}
															/>

															{qualificacaoEmpresa.map((qlf, index) => {
																return (
																	<TableCell
																		key={index}
																		label={
																			<Box display='flex' flexDirection='row'>
																				<NumberFormat
																					value={qlf[cabecalhoColumn.codigo]}
																					displayType='text'
																					thousandSeparator='.'
																					decimalSeparator=','
																					decimalScale='2'
																					fixedDecimalScale
																				/>
																				{inserirIcone(qlf['Risco' + cabecalhoColumn.codigo], false)}
																			</Box>
																		}
																	/>
																);
															})}
														</TableRow>
													);
												}
											})}
										</Table>
									)}
								</CardContent>
							</Card>
						</Box>
					</Box>
				</CardContent>
			</Card>
		</Box>
	);
}
