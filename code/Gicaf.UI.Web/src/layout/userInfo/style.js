import styled from 'styled-components';
import { Person as PersonIconMUI } from '@material-ui/icons';
import { Typography, Box } from '@material-ui/core';
import theme from '@/theme';

const PersonIcon = styled(PersonIconMUI)`
  width:35px !important
  height:35px !important
  margin-right:${theme.spacing(2)}
`;

const UserName = styled(Typography)`
  color: ${theme.palette.common.white} !important
  font-size: 12px !important
`;

const Profile = styled(Typography)`
  color: ${theme.palette.common.white} !important
  font-size: 10px !important
`;

const Info = styled(Box)` 
margin-right:${theme.spacing(3)}px
`;

export { PersonIcon, UserName, Profile, Info };
