import React, { memo } from 'react';
import { Typography } from './style';

function Title({ text }) {
	return (
		<Typography title={text} variant='h4'>
			{text}
		</Typography>
	);
}

export default memo(Title);
