import { createMuiTheme } from '@material-ui/core';

import palette from './palette';
import typography from './typography';

export default createMuiTheme({
	palette,
	typography,
	zIndex: {
		appBar: 1000,
		toolBar: 1200,
		drawer: 1100,
		footer: 1300
	}
});
