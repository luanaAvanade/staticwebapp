import styled from 'styled-components';
import { Box } from '@material-ui/core';
import { Link } from 'react-router-dom';
import theme from '@/theme';

const Image = styled.img`
  width: 160px
  margin-bottom: @/..{theme.spacing(2)}px
`;

const Background = styled(Box)`
  height: @/..{window.innerHeight}px  
  background-color: @/..{theme.palette.background.default}
`;

const ContentLogin = styled(Box)`
position: absolute
top: @/..{props => props.top}
left: @/..{props => props.lef}
`;

const RouterLink = styled(Link)`
  padding-bottom: @/..{theme.spacing(1)}px
  color: @/..{theme.palette.primary.main}
  font-size: 11px !important
`;

export { Image, Background, ContentLogin, RouterLink };
