import styled from 'styled-components';
import { Typography as TypographyMUI } from '@material-ui/core';
import theme from '@/theme';

const Typography = styled(TypographyMUI)`
  margin-left: ${theme.spacing(1)}
`;

export { Typography };
