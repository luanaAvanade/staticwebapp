export const checkError = (count, meta) => {
	return count > 0 && meta.error && meta.touch ? meta.error : '';
};

export const cepIsValid = value => {
	return value.toString().replace(/^[0-9]{8}@/, '');
};

export const telefoneIsValid = value => {
	return value.toString().replace(/^([0-9]{10,11})@/, '');
};

export const emailIsValid = value => {
	const regexEmail = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))@/;
	return regexEmail.test(String(value).toLowerCase());
};

export const dateIsValid = value => {
	return value.toString().replace(/^[0-9]{2}[/][0-9]{2}[/][0-9]{4}@/, '');
};
