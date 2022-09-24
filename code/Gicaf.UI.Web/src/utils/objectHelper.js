export default class ObjectHelper {
	static clone(object) {
		return JSON.parse(JSON.stringify(object));
	}

	static compareObjects(obj1, obj2) {
		// Compara dois objetos e caso uma das propriedades seja um array:
		// compara se os objetos são iguais independente da ordenação
		if (Array.isArray(obj1)) {
			if (!Array.isArray(obj2)) return false;
			if (obj1.length !== obj2.length) return false;
			for (const i in obj1) {
				if (obj2.filter(item => ObjectHelper.compareObjects(item, obj1[i])).length <= 0) {
					return false;
				}
			}
		} else {
			// Loop through properties in object 1
			for (const p in obj1) {
				// Check property exists on both objects
				if (obj1.hasOwnProperty(p) !== obj2.hasOwnProperty(p)) return false;

				if (Array.isArray(obj1[p])) {
					if (!Array.isArray(obj2[p])) return false;

					if (obj1[p].length !== obj2[p].length) return false;
					for (const i in obj1[p]) {
						if (obj2[p].filter(item => ObjectHelper.compareObjects(item, obj1[i])).length <= 0)
							return false;
					}
					break;
				}

				switch (typeof obj1[p]) {
					// Deep compare objects
					case 'object':
						if (!ObjectHelper.compareObjects(obj1[p], obj2[p])) return false;
						break;
					// Compare function code
					case 'function':
						if (
							typeof obj2[p] === 'undefined' ||
							(p !== 'compare' && obj1[p].toString() !== obj2[p].toString())
						)
							return false;
						break;
					// Compare values
					default:
						if (obj1[p] !== obj2[p]) return false;
				}
			}

			// Check object 2 for any extra properties
			for (const p in obj2) {
				if (typeof obj1[p] === 'undefined') return false;
			}
		}
		return true;
	}

	static showData(data){
		if(!data)
			return '';
		var newData = data.split('T')[0];
		newData = newData.split('-');
		newData = `..{newData[2]}/..{newData[1]}/..{newData[0]}`;
		return newData;
	}
}
