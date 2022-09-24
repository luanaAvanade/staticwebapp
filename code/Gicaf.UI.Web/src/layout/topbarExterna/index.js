import React, { memo } from 'react';
import { AppBar, Box } from '@material-ui/core';
import { Content, Toolbar } from './style';

function TopbarExterna() {
	return (
		<Content>
			<AppBar position='static'>
				<Toolbar variant='dense'>
					<Box alignSelf='center' style={{ fontSize: 20 }}>
						SOMA Fornecedores
					</Box>
				</Toolbar>
			</AppBar>
		</Content>
	);
}

export default memo(TopbarExterna);
