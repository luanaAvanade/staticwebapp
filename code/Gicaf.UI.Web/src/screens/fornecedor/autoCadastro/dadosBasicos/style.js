import styled from 'styled-components';
import { TextField as TextFieldMUI } from '@material-ui/core';
import theme from '@/theme';

const Mensagem = styled.p`
  overflow-wrap: break-word;
  background-color: @/..{theme.palette.message.background} !important
  color: @/..{theme.palette.message.color} !important
  borderRadius: 5 !important
`;

const TextFieldNovo = styled(TextFieldMUI)`
  margin-top: 0px !important
  margin-bottom: 0px !important
  width:@/..{props => (props.width ? props.width : '100%')}
`;

export { Mensagem, TextFieldNovo };
