import { createActions, createReducer } from 'reduxsauce';
import Immutable from 'seamless-immutable';

export const { Types, Creators } = createActions({
	setLoading: [
		'message'
	],
	disableLoading: []
});

const loader = {
	open: false,
	message: ''
};

const INITIAL_STATE = Immutable(Object.assign({}, loader));

const setLoading = (state = INITIAL_STATE, action) =>
	state.merge({
		open: true,
		message: action.message ? action.message : 'Carregando'
	});

const disableLoading = (state = INITIAL_STATE) => state.merge(loader);

export default createReducer(INITIAL_STATE, {
	[Types.SET_LOADING]: setLoading,
	[Types.DISABLE_LOADING]: disableLoading
});
