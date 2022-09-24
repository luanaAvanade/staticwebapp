const STRICT_STRIP_REGEX = /[-/.]/g;
const LOOSE_STRIP_REGEX = /[^\d]/g;
const BLACKLIST = [
	'00000000000000',
	'11111111111111',
	'22222222222222',
	'33333333333333',
	'44444444444444',
	'55555555555555',
	'66666666666666',
	'77777777777777',
	'88888888888888',
	'99999999999999'
];

export const temOnze = number => {
	const stripped = strip(number);
	if (stripped.length === 11) {
		return true;
	}
	return false;
};

export const cpfIsValid = number => {
	const stripped = strip(number);

	// CNPJ can't be blacklisted
	if (BLACKLIST.indexOf(stripped) >= 0) {
		return false;
	}

	let numbers = stripped.substr(0, 9);
	numbers += verifierDigitCpf(numbers);
	numbers += verifierDigitCpf(numbers);

	return numbers.substr(-2) === stripped.substr(-2);
};

export const strip = (number, strict) => {
	const regex = strict ? STRICT_STRIP_REGEX : LOOSE_STRIP_REGEX;
	return (number || '').toString().replace(regex, '');
};

export const verifierDigitCpf = numbers => {
	numbers = numbers.split('').map(number => {
		return parseInt(number, 10);
	});

	const modulus = numbers.length + 1;

	const multiplied = numbers.map((number, index) => {
		return number * (modulus - index);
	});

	const mod =
		multiplied.reduce((buffer, number) => {
			return buffer + number;
		}) % 11;

	return mod < 2 ? 0 : 11 - mod;
};
