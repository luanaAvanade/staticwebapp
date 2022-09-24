import styled from 'styled-components';
import { OfflinePin } from '@material-ui/icons';

const CabecalhoHorizontal = styled.p`
  font-weight: ${props => (props.negrito ? 'bold' : 'unset')} !important
  padding-left: ${props => props.paddingLeft}px !important
`;

const SugestaoAprovar = styled(OfflinePin)`
	color: green;
`;

const SugestaoReprovar = styled(OfflinePin)`
	color: red;
`;

const SugestaoAprovarComRessalvas = styled(OfflinePin)`
	color: orange;
`;

export { CabecalhoHorizontal, SugestaoAprovarComRessalvas, SugestaoAprovar, SugestaoReprovar };
