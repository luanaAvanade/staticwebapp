import styled from 'styled-components';
import { Drawer as DrawerMUI } from '@material-ui/core';
import TopbarComponent from '@/layout/topbar';
import theme from '@/theme';

const Topbar = styled(TopbarComponent)`
	top: 0
	position: flex
	width: 100%
	left: 0
	right: auto
`;

const Drawer = styled(DrawerMUI)`
	z-Index: ${theme.zIndex.drawer}
	width: 271px
`;

const Content = styled.main`
	top: 0
	flex: 1
	height: calc(100% - 70px) !important
	padding: ${theme.spacing(1)}px
	overflow:auto
	outline: none
	position: fixed
	margin-top: ${props => (props.topbar ? '48px' : '0px')} !important
	background-color: ${theme.palette.background.default}
	margin-left: ${props => (props.sidebar ? '270px' : '0px')} !important
	width: ${props => (props.sidebar ? 'calc(-271px + 100vw)' : '100%')} !important
`;

export { Topbar, Drawer, Content };
