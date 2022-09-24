import styled from 'styled-components';
import { Box as BoxMUI, Typography } from '@material-ui/core';
import theme from '@/theme';

const Box = styled(BoxMUI)`
  z-index: ${theme.zIndex.footer}
  position: absolute
  bottom: 0px
  background-color: ${theme.palette.primary.main}
  width: 100%
  height: 22px
`;

const Text = styled(Typography)`
  font-size: 11px !important 
  color: ${theme.palette.common.white} !important
  align-self: center
`;

const Image = styled.img`
  cursor: pointer
  width: 80px
  height: 14px
  margin-left: ${theme.spacing(2)}px
  margin-right: ${theme.spacing(2)}px
  align-self: center
`;

export { Box, Text, Image };
