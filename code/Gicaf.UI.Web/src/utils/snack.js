import React from 'react';
import { IconButton, SnackbarContent } from '@material-ui/core';
import { Close as CloseIcon, CheckCircle, Error, Warning, Info } from '@material-ui/icons';
import theme from '@/theme';

const iconVariant = {
	opacity: 0.9,
	marginRight: theme.spacing(1)
};

const snack = (message, closeSnackbar, backgroundColor, icon = null) => {
	return {
		persist: false,
		anchorOrigin: {
			vertical: 'top',
			horizontal: 'right'
		},
		children: key => (
			<SnackbarContent
				message={
					<span
						style={{
							display: 'flex',
							alignItems: 'center'
						}}
					>
						{icon}
						{message}
					</span>
				}
				style={{ backgroundColor }}
				action={
					<IconButton
						key='close'
						aria-label='Close'
						color='inherit'
						onClick={() => closeSnackbar(key)}
					>
						<CloseIcon />
					</IconButton>
				}
			/>
		)
	};
};

export function snackPrimary(message, closeSnackbar) {
	return snack(message, closeSnackbar, theme.palette.primary.main);
}

export function snackSecondary(message, closeSnackbar) {
	return snack(message, closeSnackbar, theme.palette.secondary.main);
}

export function snackSuccess(message, closeSnackbar) {
	return snack(
		message,
		closeSnackbar,
		theme.palette.success.main,
		<CheckCircle style={iconVariant} />
	);
}

export function snackError(message, closeSnackbar) {
	return snack(message, closeSnackbar, theme.palette.error.main, <Error style={iconVariant} />);
}

export function snackWarning(message, closeSnackbar) {
	return snack(message, closeSnackbar, theme.palette.warning.main, <Warning style={iconVariant} />);
}

export function snackInfo(message, closeSnackbar) {
	return snack(message, closeSnackbar, theme.palette.info.main, <Info style={iconVariant} />);
}
