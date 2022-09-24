import styled from 'styled-components';

const CabecalhoHorizontal = styled.p`
  font-weight: 'bold' !important
  padding-left: @/..{props => props.paddingLeft}px !important
`;

export { CabecalhoHorizontal };
