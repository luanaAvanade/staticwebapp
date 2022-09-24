import React, { memo } from 'react';
import _ from 'lodash';
import { Typography } from '@material-ui/core';
import useReactRouter from 'use-react-router';
import { Content, Crumb, HomeIcon } from './style';
import paths from '@/utils/paths';
import { SUBDIRETORIO_LINK } from '@/utils/constants';

function Breadcrumbs() {
	const { history } = useReactRouter();
	const allParts = paths.getPartsOfPath(history);
	let otherParts = [];
	let lastPart = _.last(allParts);

	if (!isNaN(lastPart)) {
		lastPart = _.last(allParts.slice(2, allParts.length - 1));
		otherParts = allParts.slice(2, allParts.length - 2);
	} else {
		otherParts = allParts.slice(2, allParts.length - 1);
	}

	const currentName = paths.getName(lastPart);

	return (
		<Content>
			<Crumb title={paths.getName('home')} to={paths.getPathByCodigo('home')}>
				<HomeIcon />
			</Crumb>
			{otherParts.map((crumb, index) => {
				const crumbPath =
					SUBDIRETORIO_LINK +
					[
						'',
						...otherParts.slice(0, index + 1)
					].join('/');
				return (
					<Crumb title={paths.getName(crumb)} key={crumbPath} to={crumbPath}>
						{paths.getName(crumb)}
					</Crumb>
				);
			})}
			<Typography title={currentName} color='textPrimary'>
				{currentName}
			</Typography>
		</Content>
	);
}

export default memo(Breadcrumbs);
