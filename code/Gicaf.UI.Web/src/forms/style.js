import styled from 'styled-components';
import {
	Box,
	FormHelperText,
	Typography,
	TextField as TextFieldMUI,
	Select as SelectMUI
} from '@material-ui/core';
import colors from '@/theme/colors';

const LabelFile = styled.label`
  cursor:pointer
  border-radius: 4px
  background-color: ${props => props.labelfilebackgroundcolor}
  color: ${props => props.labelfilecolor}
  text-align: center
  height:40px
  min-width:20%
  margin-top:0px
  margin-bottom:0px
  padding-top:8px
`;

const InputFile = styled.input`
  width: 0.1px
  height: 0.1px
  opacity: 0
  overflow: hidden
  position: absolute
  cursor: pointer
  display: none
  z-index: -1
`;

const TextField = styled(TextFieldMUI)`
  margin-top: 0px !important
  margin-bottom: 0px !important
  width:${props => (props.width ? props.width : '100%')}
`;

const BoxInput = styled(Box)`
  margin-bottom: 8px
  margin-rigth:16px !important
`;

const BoxValidation = styled(Box)`
  margin-top: 8px
	margin-bottom: 8px
	border: ${props => (props.error ? '1px' : '0px')} solid ${colors.red}
	border-radius: 4px
`;

const Helper = styled(FormHelperText)`
  color: ${props => props.helpercolor} !important
`;

const LabelHelper = styled(Typography)`
  margin-left: 8px !important
  color: ${props => props.labelcolor} !important
  font-size: 10px !important
  font-style: italic !important
`;

const Select = styled(SelectMUI)`
  text-align: ${props => (props.textaling ? props.textaling : 'left')} !important
  width:${props => (props.width ? props.width : '100%')} !important
  max-width:${props => (props.maxWidth ? props.maxWidth : '100%')}!important
`;

const Required = styled(Typography)`
  color: ${colors.red} !important
`;

export {
	BoxInput,
	Helper,
	Select,
	LabelHelper,
	InputFile,
	BoxValidation,
	TextField,
	LabelFile,
	Required
};
