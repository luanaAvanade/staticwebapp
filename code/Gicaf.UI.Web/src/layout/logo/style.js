import styled from 'styled-components';
import { Link as LinkDOM } from 'react-router-dom';
import theme from '@/theme';

const Link = styled(LinkDOM)`
  display: flex
	justify-content: center
	align-items: center
	height: 48px
	flex-shrink: 0
	background-color: ${theme.palette.primary.main}
	font-size: 0
	margin-bottom: ${theme.spacing(2)}px !important
	box-shadow: 0px 2px 4px -1px rgba(0,0,0,0.2), 0px 4px 5px 0px rgba(0,0,0,0.14), 0px 1px 10px 0px rgba(0,0,0,0.12)
`;

const Image = styled.img`
	cursor: pointer
	width: 100px
`;

export { Link, Image };
