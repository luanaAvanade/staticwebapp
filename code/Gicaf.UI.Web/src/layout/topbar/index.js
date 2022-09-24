import React, { memo } from 'react';
import useReactRouter from 'use-react-router';
import { AppBar } from '@material-ui/core';
import {
	ArrowBackIos as BackIcon,
	ArrowForwardIos as ForwardIcon,
	Input as InputIcon
} from '@material-ui/icons';
import { Content, Toolbar, Middle, IconButton } from './style';
import paths from '@/utils/paths';
import  UserInfo  from '../userInfo';
import { translate } from '@/locales';
import { removeUser } from '@/utils/auth';

function Topbar({ sidebar = false, onToggleSidebar }) {
	const { history } = useReactRouter();

	const handleSignOut = () => {
		removeUser();
		history.push(paths.getPathByCodigo('login'));
	};

	const acao = sidebar ? translate('clickFecharSideBar') : translate('clickAbrirSideBar');

	return (
		<Content>
			<AppBar position='static'>
				<Toolbar sidebar={sidebar.toString()} variant='dense'>
					<IconButton title={acao} onClick={onToggleSidebar} variant='text'>
						{sidebar ? <BackIcon /> : <ForwardIcon />}
					</IconButton>
					<Middle>
						<UserInfo userName='Rafael Mendonça' profile='Analista Desenvolvedor' />
					</Middle>
					<IconButton title={translate('sair')} onClick={handleSignOut}>
						<InputIcon />
					</IconButton>
				</Toolbar>
			</AppBar>
		</Content>
	);
}
function arePropsEqual(prev, next) {
	return prev.sidebar === next.sidebar;
}

export default memo(Topbar, arePropsEqual);
