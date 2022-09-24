import styled from 'styled-components';
import theme from '@/theme';
import { ListItem } from '@material-ui/core';

const NavBar = styled.nav`
	background-color: ${theme.palette.background.default}
	display: flex
	flex-direction: column
	height: 100%
	padding-left: 0
	padding-right: 0
	width: 270px
	z-index:${theme.zIndex.drawer} !important
`;

const ListModulo = styled(ListItem)`
	border-left: ${props => (props.active ? '4px' : '0px')} solid ${theme.palette.primary
	.main} !important
`;

export { NavBar, ListModulo };
