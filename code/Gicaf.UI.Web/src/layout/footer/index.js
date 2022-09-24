import React, { memo } from 'react';
import { Axxiom } from '@/assets';
import { Box, Text, Image } from './style';

function Footer() {
	return (
		<Box justifyContent='flex-end' display='flex'>
			<Text>Powered by</Text>
			<Image src={Axxiom} />
		</Box>
	);
}

export default memo(Footer);
