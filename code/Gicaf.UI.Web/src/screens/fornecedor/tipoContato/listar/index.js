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
import { snackSuccess, snackError, snackWarning } from '@/utils/snack';
import theme from '@/theme';
import { SUBDIRETORIO_LINK, ROWSPERPAGE } from '@/utils/constants';
import { Status } from '../../style';

export default function ListagemTipoContato({ getPermissao }) {
	const { history } = useReactRouter();
	const { enqueueSnackbar, closeSnackbar } = useSnackbar();
	const dispatch = useDispatch();

	// Estado Local

	const [
		tipoContatoList,
		setTipoContatoList
	] = useState([]);

	const [
		idTipoContatoExcluir,
		setIdTipoContatoExcluir
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
		tipoContatoFindAll();
		return () => {
			setTipoContatoList([]);
		};
	}, []);

	// Buscar Dados

	const tipoContatoFindAll = async () => {
		dispatch(LoaderCreators.setLoading());
		const response = null;
		if (response.data) {
			setTipoContatoList(response.data.TipoContato_list);
			dispatch(LoaderCreators.disableLoading());
		} else {
			dispatch(LoaderCreators.disableLoading());
		}
	};

	// Ações da Tela

	const novo = () => {
		history.push(paths.getPathByCodigo('novo-tipo-contato'));
	};

	const editar = id => {
		history.push(`@/..{SUBDIRETORIO_LINK}/tipo-contato/editar-tipo-contato/@/..{id}`);
	};

	const excluir = () => {
		setIdTipoContatoExcluir(null);
		dispatch(LoaderCreators.setLoading());
	};

	// Ações de Retorno

	const callback = mensagem => {
		tipoContatoFindAll();
		dispatch(LoaderCreators.disableLoading());
		enqueueSnackbar('', snackSuccess(mensagem, closeSnackbar));
	};

	const callbackError = mensagem => {
		dispatch(LoaderCreators.disableLoading());
		enqueueSnackbar('', snackError(mensagem, closeSnackbar));
	};

	const callbackWarning = mensagem => {
		dispatch(LoaderCreators.disableLoading());
		enqueueSnackbar('', snackWarning(mensagem, closeSnackbar));
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
		{ id: 'Nome', label: translate('nome'), width: '5%' },
		{ id: 'Descricao', label: translate('descricao'), width: '70%' },
		{ id: 'Status', label: translate('status'), width: '10%' },
		{ id: 'Acoes', label: translate('acoes'), width: '5%', colSpan: 3, align: 'center' }
	];

	let variantTableRow = theme.palette.table.tableRowPrimary;

	return (
		<LayoutContent>
			<Box display='flex' justifyContent='flex-end'>
				{getPermissao() && (
					<Button text='adicionar' onClick={novo} margin={`0px 0px 0px @/..{theme.spacing(1)}px`} />
				)}
			</Box>
			<Confirm
				open={idTipoContatoExcluir !== null}
				handleClose={() => setIdTipoContatoExcluir(null)}
				handleSuccess={excluir}
				title={translate('confirmacao')}
				text={translate('desejaRealmenteExcluirTipoContato')}
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
					{stableSort(tipoContatoList, getSorting(order, orderBy))
						.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
						.map((tipoContato, index) => {
							variantTableRow =
								variantTableRow === theme.palette.table.tableRowPrimary
									? theme.palette.table.tableRowSecondary
									: theme.palette.table.tableRowPrimary;
							return (
								<TableRow key={index} backgroundColor={variantTableRow}>
									<TableCell label={tipoContato.Nome} />
									<TableCell label={tipoContato.Descricao} />
									<Status
										label={tipoContato.Status ? 'Ativo' : 'Inativo'}
										ativo={tipoContato.Status}
										backgroundColor={tipoContato.Status ? '#1e90ff' : '#808080'}
									/>
									<TableCell
										title={translate('editar')}
										label={
											<IconButton
												disabled={!getPermissao()}
												size='small'
												onClick={() => editar(tipoContato.value)}
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
												onClick={() => setIdTipoContatoExcluir(tipoContato.value)}
											>
												<Delete />
											</IconButton>
										}
									/>
								</TableRow>
							);
						})}
					{tipoContatoList.length === 0 && (
						<TableRow backgroundColor={variantTableRow}>
							<TableCell align='center' colSpan={5} label={translate('semResultadosAExibir')} />
						</TableRow>
					)}
				</TableBody>
			</Table>
			{tipoContatoList.length > rowsPerPage && (
				<TablePagination
					rowsPerPageOptions={ROWSPERPAGE}
					labelRowsPerPage={translate('linhasPorPagina')}
					component='div'
					count={tipoContatoList.length}
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
