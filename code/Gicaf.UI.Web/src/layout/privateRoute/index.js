import React, { memo } from 'react';
import { Route, Redirect } from 'react-router-dom';
import { isAuthenticated } from '@/utils/auth';
import { checkFuncionalidade, modulosPermitidos } from '@/utils/modulos';

function PrivateRoute({ funcionalidade, acao, component: Component, ...rest }) {
	const isAuthenticatedAndPermitted = () => {
		if (funcionalidade && acao) {
			if (isAuthenticated()) {
				return checkFuncionalidade(funcionalidade, acao);
			}
		} else {
			return isAuthenticated();
		}
	};

	const getPermissao = () => {
		const permitMap = modulosPermitidos();
		let permissao = permitMap.get(funcionalidade);
		// if (permissao == 'MODIFICA') {
		// 	return true;
		// } else {
		// 	return false;
		// }
		return true;
	};

	const getComponent = props => {
		if (!isAuthenticated()) {
			return <Redirect to={{ pathname: '/', state: { from: props.location } }} />;
		} else {
			return isAuthenticatedAndPermitted() ? (
				<Component {...props} {...{ getPermissao: () => getPermissao() }} />
			) : (
				<Redirect to={{ pathname: '/semPermissao', state: { from: props.location } }} />
			);
		}
	};

	return <Route {...rest} render={props => getComponent(props)} />;
}

export default memo(PrivateRoute);
