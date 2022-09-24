import React, { memo } from 'react';
import { withStyles, ListItem, ListItemText } from '@material-ui/core';
import { NavLink } from 'react-router-dom';
import style from './style';

function ItemSidebar({ classes, text, linkTo }) {
	const { activeNavItem, navItem, listItemText, list } = classes;
	return (
		<ListItem dense button className={list}>
			<NavLink activeClassName={activeNavItem} className={navItem} to={linkTo}>
				<ListItemText title={text} classes={{ primary: listItemText }} primary={text} />
			</NavLink>
		</ListItem>
	);
}

export default memo(withStyles(style)(ItemSidebar));
