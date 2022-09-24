import { combineReducers } from 'redux';

import loader from './loader';
import sidebar from './sidebar';

export default combineReducers({
	loader,
	sidebar
});
