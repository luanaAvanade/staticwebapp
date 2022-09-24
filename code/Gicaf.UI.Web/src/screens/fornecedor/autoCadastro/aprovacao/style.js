import styled from 'styled-components';
import { IconButton as IconButtonMUI } from '@material-ui/core';
import theme from '@/theme';

const IconButton = styled(IconButtonMUI)`
  padding: 3px
`;

const DisplayDiv = styled.div`
visibility: @/..{props => (props.visible ? 'visible' : 'hidden')}
height:@/..{props => (props.visible ? '100%' : '0px')}
`;

export { DisplayDiv, IconButton };
