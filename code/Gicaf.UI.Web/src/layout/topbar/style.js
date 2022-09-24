import styled from 'styled-components';
import { IconButton as IconButtonMUI, Toolbar as ToolbarMUI } from '@material-ui/core';
import theme from '@/theme';

const Content = styled.div`
	border-bottom: 1px solid ${theme.palette.border}
	background-color: ${theme.palette.common.white}
	display: flex
	align-items: center
	flex-grow: 1
	z-index: ${theme.zIndex.appBar}
`;

const Toolbar = styled(ToolbarMUI)`
	z-index: ${theme.zIndex.toolBar}
	min-height: auto
	margin-left: ${props => (props.sidebar === 'true' ? '270px' : '0px')} !important
	transition:none
`;

const IconButton = styled(IconButtonMUI)`
	margin-right: ${theme.spacing(1)}
	color: ${theme.palette.common.white} !important
`;

const Middle = styled.div`
	display: flex
	flex-grow:1
	justify-content:flex-end
`;

export { Content, Toolbar, IconButton, Middle };
