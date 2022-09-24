import styled from 'styled-components';
import { Done, Clear } from '@material-ui/icons';
import theme from '@/theme';

const DisplayDiv = styled.div`
visibility: @/..{props => (props.visible ? 'visible' : 'hidden')}
height:@/..{props => (props.visible ? '100%' : '0px')}
`;

const Valid = styled(Done)`
color:green !important
`;

const Invalid = styled(Clear)`
color:red !important
`;

export { DisplayDiv, Valid, Invalid };
