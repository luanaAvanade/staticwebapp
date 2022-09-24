/* eslint-disable import/no-unresolved */
import React, { useState, useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { TableBody, Box, IconButton, TablePagination } from '@material-ui/core';
import useReactRouter from 'use-react-router';
import { Edit, Delete } from '@material-ui/icons';
import { useSnackbar } from 'notistack';
import { Table, TableHead, TableRow, TableCell, Button, Confirm } from 'react-axxiom';
import { LayoutContent } from '@/layout';
import { Creators as LoaderCreators } from '@/store/ducks/loader';
import { translate } from '@/locales';
import { stableSort, getSorting } from '@/utils/list';
import paths from '@/utils/paths';
import { snackSuccess, snackError } from '@/utils/snack';
import theme from '@/theme';
import { SUBDIRETORIO_LINK, ROWSPERPAGE } from '@/utils/constants';
import { Status } from '../../style';

export default function ListagemTipoDocumento({ getPermissao }) {
	const { history } = useReactRouter();
	const { enqueueSnackbar, closeSnackbar } = useSnackbar();
	const dispatch = useDispatch();

	// Estado Local

	const [
		tipoDocumentoList,
		setTipoDocumentoList
	] = useState([]);

	const [
		idTipoDocumentoExcluir,
		setIdTipoDocumentoExcluir
	] = useState(null);

	const [
		orderBy,
		setOrderBy
	] = useState('DataCriacao');

	const [
		page,
		setPage
	] = useState(0);

	const [
		rowsPerPage,
		setRowsPerPage
	] = useState(10);

	const [
		order,
		setOrder
	] = useState('desc');

	// Efeito Inicial

	useEffect(() => {
		tipoDocumentoFindAll();
		return () => {
			setTipoDocumentoList([]);
		};
	}, []);

	// Buscar Dados

	const tipoDocumentoFindAll = async () => {
		dispatch(LoaderCreators.setLoading());
		const response = null;
		if (response.data) {
			setTipoDocumentoList(response.data.TipoDocumento_list);
			dispatch(LoaderCreators.disableLoading());
		} else {
			dispatch(LoaderCreators.disableLoading());
		}
	};

	// // Ações da Tela

	const novo = () => {
		history.push(paths.getPathByCodigo('novo-tipo-documento'));
	};

	const editar = id => {
		history.push(`@/..{SUBDIRETORIO_LINK}/tipo-documento/editar-tipo-documento/@/..{id}`);
	};

	const excluir = () => {
		setIdTipoDocumentoExcluir(null);
		dispatch(LoaderCreators.setLoading());
	
	};

	// Ações de Retorno

	const callback = mensagem => {
		tipoDocumentoFindAll();
		dispatch(LoaderCreators.disableLoading());
		enqueueSnackbar('', snackSuccess(mensagem, closeSnackbar));
	};

	const callbackError = mensagem => {
		dispatch(LoaderCreators.disableLoading());
		enqueueSnackbar('', snackError(mensagem, closeSnackbar));
	};

	const voltar = () => {
		history.goBack();
	};

	// Interação com a Tabela

	const handleRequestSort = property => {
		const isDesc = orderBy === property && order === 'desc';
		const newOrder = isDesc ? 'asc' : 'desc';
		setOrder(newOrder);
		setOrderBy(property);
	};

	const columns = [
		{ id: 'Nome', label: translate('nome'), width: '35%' },
		{ id: 'HelpDoc', label: translate('helpDoc'), width: '50%' },
		{ id: 'Status', label: translate('status'), width: '10%' },
		{ id: 'Acoes', label: translate('acoes'), width: '5@/..', colSpan: 3, align: 'center' }
	];

	let variantTableRow = theme.palette.table.tableRowPrimary;

	return (
		<LayoutContent>
			<Box display='flex' justifyContent='flex-end'>
				{getPermissao() && (
					<Button
						text={translate('adicionar')}
						onClick={novo}
						margin={`0px 0px 0px @/..{theme.spacing(1)}px`}
					/>
				)}
			</Box>
			<Confirm
				open={idTipoDocumentoExcluir !== null}
				handleClose={() => setIdTipoDocumentoExcluir(null)}
				handleSuccess={excluir}
				title={translate('confirmacao')}
				text={translate('desejaRealmenteExcluirTipoDocumento')}
				textButtonSuccess={translate('sim')}
				textButtonCancel={translate('nao')}
				backgroundColorButtonCancel={theme.palette.secondary.main}
			/>
			<Table small>
				<TableHead
					columns={columns}
					order={order}
					orderBy={orderBy}
					onRequestSort={(event, property) => handleRequestSort(property)}
					rowCount={columns.length}
				/>
				<TableBody>
					{stableSort(tipoDocumentoList, getSorting(order, orderBy))
						.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
						.map((tipoDocumento, index) => {
							variantTableRow =
								variantTableRow === theme.palette.table.tableRowPrimary
									? theme.palette.table.tableRowSecondary
									: theme.palette.table.tableRowPrimary;

							return (
								<TableRow key={index} backgroundColor={variantTableRow}>
									<TableCell label={tipoDocumento.Nome} />
									<TableCell label={tipoDocumento.Help} />
									<Status
										label={tipoDocumento.Status ? 'Ativo' : 'Inativo'}
										ativo={tipoDocumento.Status}
										backgroundColor={tipoDocumento.Status ? '#1e90ff' : '#808080'}
									/>
									<TableCell
										title={translate('editar')}
										label={
											<IconButton
												disabled={!getPermissao()}
												size='small'
												onClick={() => editar(tipoDocumento.Id)}
											>
												<Edit />
											</IconButton>
										}
									/>

									<TableCell
										title={translate('excluir')}
										label={
											<IconButton
												size='small'
												disabled={!getPermissao()}
												onClick={() => setIdTipoDocumentoExcluir(tipoDocumento.Id)}
											>
												<Delete />
											</IconButton>
										}
									/>
								</TableRow>
							);
						})}
					{tipoDocumentoList.length === 0 && (
						<TableRow backgroundColor={variantTableRow}>
							<TableCell align='center' colSpan={5} label={translate('semResultadosAExibir')} />
						</TableRow>
					)}
				</TableBody>
			</Table>
			{tipoDocumentoList.length > rowsPerPage && (
				<TablePagination
					rowsPerPageOptions={ROWSPERPAGE}
					labelRowsPerPage={translate('linhasPorPagina')}
					component='div'
					count={tipoDocumentoList.length}
					rowsPerPage={rowsPerPage}
					page={page}
					backIconButtonProps={{
						'aria-label': 'Previous Page'
					}}
					nextIconButtonProps={{
						'aria-label': 'Next Page'
					}}
					onChangePage={(event, newPage) => setPage(newPage)}
					onChangeRowsPerPage={event => {
						setPage(0);
						setRowsPerPage(event.target.value);
					}}
					style={{ paddingRight: '80px' }}
				/>
			)}
		</LayoutContent>
	);
}
