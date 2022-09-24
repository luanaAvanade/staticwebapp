import styled from 'styled-components';
import { Link } from 'react-router-dom';
import { Breadcrumbs as BreadcrumbsMUI } from '@material-ui/core';
import { Home as HomeIconMUI } from '@material-ui/icons';
import theme from '@/theme';

const Crumb = styled(Link)`
  color:${theme.palette.text.secondary}
`;

const Content = styled(BreadcrumbsMUI)`
  justify-content: center
  flex-wrap: wrap
  margin-bottom: ${theme.spacing(1)}px !important
  padding: ${theme.spacing(1, 1, 1, 0)}
`;

const HomeIcon = styled(HomeIconMUI)`
  margin-right: ${theme.spacing(0.5)}px
  width: 20px !important
  height: 20px !important
`;

export { Content, Crumb, HomeIcon };
