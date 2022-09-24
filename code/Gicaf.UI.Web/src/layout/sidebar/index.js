import React, { useEffect, memo } from 'react';
import { List, ListItemText, Collapse, Box } from '@material-ui/core';
import { useDispatch, useSelector } from 'react-redux';
import useReactRouter from 'use-react-router';
import { KeyboardArrowDown, KeyboardArrowUp } from '@material-ui/icons';
import _ from 'lodash';
import { Creators as SidebarCreators } from '@/store/ducks/sidebar';
import  Logo  from '../logo';
import  ItemSidebar  from '../itemSidebar';
import paths from '@/utils/paths';
import { translate } from '@/locales';
import { NavBar, ListModulo } from './style';
import { modulosPermitidos } from '@/utils/modulos';

function SideBar() {
	const { history } = useReactRouter();

	const getModulos = () => {
		const modulesMap = modulosPermitidos();
		let modulos = [];

		// HOME
		const home = {
			codigo: 'home',
			text: translate('home'),
			linkTo: paths.getPathByCodigo('home')
		};

		// FORNECEDOR
		let subModulosFornecedor = [];
		const fornecedor = {
			codigo: 'fornecedor',
			text: 'Fornecedor',
			open: false,
			subModulos: subModulosFornecedor
		};

		// SUBMODULO FORNECEDOR
		const cadastroComplementar = {
			codigo: 'cadastro-complementar',
			text: translate('cadastroComplementar'),
			linkTo: paths.getPathByCodigo('cadastro-complementar')
		};
		const analistaCadastro = {
			codigo: 'analise-cadastro',
			text: 'Analise de Cadastros',
			linkTo: paths.getPathByCodigo('analise-cadastro')
		};

		// MEC
		let subModulosMec = [];
	
		// VALIDACOES PARA ADICIONAR OS SUBMODULOS


		// SUBMODULO FORNECEDOR
		//if (modulesMap.has('FORNECEDOR_ANALISE_CADASTRO')) {
			subModulosFornecedor.push(analistaCadastro);
		//}
		//if (modulesMap.has('FORNECEDOR_CADASTRO')) {
			subModulosFornecedor.push(cadastroComplementar);


		// SUBMODULO AUTENTICADOR
		if (modulesMap.has('AUTENTICADOR_ACESSO')) {
		}

		// Modulo Home sempre será adicionado
		modulos.push(home);
		// Caso exista submodulo o modulo é adicionado
		if (subModulosMec.length > 0) {
			//modulos.push(mec);
		}
		if (subModulosFornecedor.length > 0) {
			modulos.push(fornecedor);
		}

		return modulos;
	};

	const dispatch = useDispatch();
	const sidebarModuloRedux = useSelector(stateRedux => stateRedux.sidebar.modulo);

	useEffect(() => {
		const firstLevel = _.find(getModulos(), { codigo: paths.getCurrentPath(history) });

		if (firstLevel && !checkModulo(firstLevel)) {
			setModulo(firstLevel);
		} else {
			getModulos().forEach(modulo => {
				if (modulo.subModulos) {
					const secondLevel = _.find(modulo.subModulos, {
						codigo: paths.getCurrentPath(history)
					});
					if (secondLevel && !checkModulo(modulo)) {
						setModulo(modulo);
					}
				}
			});
		}
	}, []);

	const handleClick = modulo => {
		if (modulo.linkTo) {
			history.push(modulo.linkTo);
		}
		if (!checkModulo(modulo)) {
			setModulo(modulo);
		} else {
			setModulo('');
		}
	};

	const setModulo = modulo => {
		dispatch(SidebarCreators.setModulo(checkModulo(modulo) ? '' : modulo.codigo));
	};

	const checkModulo = modulo => {
		return modulo.codigo === sidebarModuloRedux;
	};

	return (
		<NavBar>
			<Logo />
			<List component='div' disablePadding>
				{getModulos().map(modulo => {
					return (
						<Box display='flex' key={modulo.codigo} flexDirection='column'>
							<ListModulo
								active={checkModulo(modulo)}
								dense
								button
								onClick={() => handleClick(modulo)}
							>
								<ListItemText
									style={{ paddingLeft: 0 }}
									inset
									primary={
										<Box display='flex' justifyContent='space-between'>
											{modulo.text}
											{!modulo.linkTo &&
												(checkModulo(modulo) ? <KeyboardArrowUp /> : <KeyboardArrowDown />)}
										</Box>
									}
								/>
							</ListModulo>
							<Collapse in={checkModulo(modulo)} timeout='auto' unmountOnExit>
								{modulo.subModulos &&
									modulo.subModulos.length > 0 &&
									modulo.subModulos.map(subModulo => {
										return (
											<ItemSidebar
												key={subModulo.codigo}
												text={subModulo.text}
												linkTo={subModulo.linkTo}
											/>
										);
									})}
							</Collapse>
						</Box>
					);
				})}
			</List>
		</NavBar>
	);
}

export default memo(SideBar);
