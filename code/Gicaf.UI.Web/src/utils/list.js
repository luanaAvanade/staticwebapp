import ObjectHelper from './objectHelper';

function desc(a, b, orderBy) {
	try {
		if (orderBy.includes('.')) {
			var o = orderBy.split('.');
			var atemp = a;
			var btemp = b;

			for (var i = 0; i < o.length; i++) {
				atemp = atemp[o[i]];
				btemp = btemp[o[i]];
				orderBy = o[i];
			}
			a[orderBy] = atemp;
			b[orderBy] = btemp;
		}
		if (b[orderBy] < a[orderBy]) {
			return -1;
		}
		if (b[orderBy] > a[orderBy]) {
			return 1;
		}
		return 0;
	} catch (error) {
		return 0;
	}
}

export function stableSort(array, cmp) {
	const stabilizedThis = array.map((el, index) => [
		el,
		index
	]);
	stabilizedThis.sort((a, b) => {
		const order = cmp(a[0], b[0]);
		if (order !== 0) return order;
		return a[1] - b[1];
	});
	return stabilizedThis.map(el => el[0]);
}

export function getSorting(order, orderBy) {
	//return order === 'desc' ? (a, b) => desc(a, b, orderBy) : (a, b) => -desc(a, b, orderBy);

	if (order === 'desc') {
		order = (a, b) => desc(a, b, orderBy);
	} else {
		order = (a, b) => -desc(a, b, orderBy);
	}
	return order;
}

export function getArrayWithAttribute(array, campo) {
	const newArray = ObjectHelper.clone(array);

	newArray.forEach(object => {
		Object.keys(object).forEach(atributo => {
			if (atributo !== campo) {
				delete object[atributo];
			}
		});
	});

	return newArray;
}

export function getSimpleArrayWithAttribute(array, campo) {
	const newArray = [];

	array.forEach(object => {
		newArray.push(object[campo]);
	});

	return newArray;
}

export function getArrayWithMultipleAttributes(array, listaCampos) {
	const newArray = ObjectHelper.clone(array);

	newArray.forEach(object => {
		Object.keys(object).forEach(atributo => {
			if (!listaCampos.includes(atributo)) {
				delete object[atributo];
			}
		});
	});

	return newArray;
}

export function getArrayWithNotAttribute(array, campo) {
	const newArray = ObjectHelper.clone(array);

	newArray.forEach(object => {
		delete object[campo];
	});

	return newArray;
}
