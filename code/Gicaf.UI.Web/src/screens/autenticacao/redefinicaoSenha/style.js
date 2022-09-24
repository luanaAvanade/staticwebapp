import styled from 'styled-components';
import { Box } from '@material-ui/core';
import theme from '@/theme';

const Image = styled.img`
  width: 160px
  margin-bottom: @/..{theme.spacing(2)}px
`;

const Background = styled(Box)`
  height: @/..{window.innerHeight}px  
  backgroundColor: @/..{theme.palette.background.default}
`;

const ContentLogin = styled(Box)`
position: absolute
top: @/..{props => props.top}
left: @/..{props => props.lef}
`;

export { Image, Background, ContentLogin };
