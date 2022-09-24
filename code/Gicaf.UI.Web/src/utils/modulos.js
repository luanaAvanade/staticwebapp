import { getUser } from './auth';
import _ from 'lodash';

export const modulosPermitidos = () => {
	const usuario = getUser();
	let moduleMap = new Map();
	if (usuario === null) return moduleMap;
	let modulos = _.filter(usuario.permissions, p => {
		if (p.c || p.u || p.d || p.r) {
			return p;
		}
	});

	modulos.forEach(mod => {
		if (mod.c || mod.u || mod.d) {
			if (mod.children && mod.children.length) {
				mod.children.forEach(child => {
					if (!moduleMap.has(child)) {
						moduleMap.set(child, 'MODIFICA');
					}
				});
			}
			if (!moduleMap.has(mod.name)) {
				moduleMap.set(mod.name, 'MODIFICA');
			}
		}
		if (!mod.c && !mod.u && !mod.d && mod.r) {
			if (mod.children && mod.children.length) {
				mod.children.forEach(child => {
					if (!moduleMap.has(child)) {
						moduleMap.set(child, 'VISUALIZA');
					}
				});
			}
			if (!moduleMap.has(mod.name)) {
				moduleMap.set(mod.name, 'VISUALIZA');
			}
		}
	});

	return moduleMap;
};

export const checkFuncionalidade = (funcionalidade, acao) => {
	//const usuario = getUser();

	let isPermitted = true;
	// usuario.permissions.forEach(permission => {
	// 	if (permission.children && permission.children.length && permission[acao]) {
	// 		permission.children.forEach(children => {
	// 			if (children === funcionalidade) {
	// 				isPermitted = true;
	// 			}
	// 		});
	// 	} else {
	// 		if (permission.name === funcionalidade && permission[acao]) {
	// 			isPermitted = true;
	// 		}
	// 	}
	// });
	return isPermitted;
};
