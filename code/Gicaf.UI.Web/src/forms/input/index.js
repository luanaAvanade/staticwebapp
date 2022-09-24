import React, { memo } from 'react';
import { Typography, Box, IconButton } from '@material-ui/core';
import { BoxInput, Helper, LabelHelper, TextField, Required } from '@/style';
import colors from '@/theme/colors';

function FormInput({
	name,
	label,
	labelHelper,
	placeholder,
	value,
	onChange,
	required = false,
	error = '',
	disabled,
	InputProps,
	width,
	fullWidth = false,
	type = 'text',
	labelColor = colors.textSecondary,
	helperColor = colors.red,
	onClick,
	icon
}) {
	return (
		<BoxInput width={width} flexGrow={1}>
			<Box display='flex' flexDirection='row'>
				<Typography title={label} variant='h6'>
					{label}
				</Typography>
				{required && <Required>*</Required>}
				{labelHelper && (
					<LabelHelper labelcolor={labelColor} title={labelHelper}>
						( {labelHelper} )
					</LabelHelper>
				)}
			</Box>
			<TextField
				width={onClick ? '5%' : width}
				type={type}
				disabled={disabled}
				name={name}
				error={error}
				title={placeholder}
				placeholder={placeholder}
				InputProps={InputProps}
				fullWidth={fullWidth}
				margin='dense'
				variant='outlined'
				value={value}
				onChange={onChange}
				{...name}
			/>
			{onClick && <IconButton onClick={onClick}>{icon}</IconButton>}
			{error && <Helper helpercolor={helperColor}>{error}</Helper>}
		</BoxInput>
	);
}

const testName = (prev, next) => {
	return (
		prev.name.value === next.name.value &&
		prev.value === next.value &&
		prev.error === next.error &&
		prev.disabled === next.disabled &&
		prev.InputProps === next.InputProps &&
		prev.onClick === next.onClick &&
		prev.onChange === next.onChange
	);
};

const testValue = (prev, next) => {
	return (
		prev.name === next.name &&
		prev.value === next.value &&
		prev.error === next.error &&
		prev.disabled === next.disabled &&
		prev.InputProps === next.InputProps &&
		prev.onClick === next.onClick &&
		prev.onChange === next.onChange
	);
};

const arePropsEqual = (prev, next) => {
	if (prev.name && next.name) {
		return testName(prev, next);
	}

	return testValue(prev, next);
};

export default memo(FormInput, arePropsEqual);
