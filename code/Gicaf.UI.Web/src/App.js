import 'bootstrap/dist/css/bootstrap.css';
import React from 'react';
import { hot } from 'react-hot-loader';
import { Provider } from 'react-redux';
import { BrowserRouter as Router } from 'react-router-dom';
import { SnackbarProvider } from 'notistack';
import { ThemeProvider } from '@material-ui/styles';
import { createBrowserHistory } from 'history';
import { I18nextProvider } from 'react-i18next';
import { i18n } from '@/locales';
import configureStore from '@/store';
import Routes from '@/routes';
import theme from '@/theme';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const history = createBrowserHistory({ basename: baseUrl });

function App() {
	return (
		<Provider store={configureStore()}>
			<ThemeProvider theme={theme}>
				<Router history={history}>
					<SnackbarProvider>
						<I18nextProvider i18n={i18n}>
							<Routes />
						</I18nextProvider>
					</SnackbarProvider>
				</Router>
			</ThemeProvider>
		</Provider>
	);
}

export default hot(module)(App);
