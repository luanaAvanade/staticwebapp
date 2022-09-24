import React, { memo } from 'react';
import { Cemig } from '@/assets';
import { Link, Image } from './style';
import paths from '@/utils/paths';
import { translate } from '@/locales';

function Logo() {
	return (
		<Link title={translate('titleLogo')} to={paths.getPathByCodigo('home')}>
			<Image src={Cemig} />
		</Link>
	);
}

export default memo(Logo);
