import { createActions, createReducer } from 'reduxsauce';
import Immutable from 'seamless-immutable';

export const { Types, Creators } = createActions({
	setModulo: [
		'modulo'
	]
});

const sidebar = {
	modulo: 'HOME'
};

const INITIAL_STATE = Immutable(Object.assign({}, sidebar));

const setModulo = (state = INITIAL_STATE, action) => state.merge({ modulo: action.modulo });

export default createReducer(INITIAL_STATE, {
	[Types.SET_MODULO]: setModulo
});
