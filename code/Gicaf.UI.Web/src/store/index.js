import { compose, createStore } from 'redux';
import reducers from './ducks';

export default function configureStore() {
	const enhancers = [];
	const isDevelopment = process.env.NODE_ENV === 'development';

	if (isDevelopment && typeof window !== 'undefined' && window.devToolsExtension) {
		enhancers.push(window.devToolsExtension());
	}

	const store = createStore(reducers, compose(...enhancers));

	return store;
}
