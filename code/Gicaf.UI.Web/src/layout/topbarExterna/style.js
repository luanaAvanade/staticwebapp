import styled from 'styled-components';
import { Toolbar as ToolbarMUI } from '@material-ui/core';
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

export { Content, Toolbar };
