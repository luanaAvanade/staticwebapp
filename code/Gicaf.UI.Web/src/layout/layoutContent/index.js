import React, { useState, Fragment, memo } from 'react';
import { Box } from '@material-ui/core';
import useReactRouter from 'use-react-router';
import { Card } from 'react-axxiom';
import { Topbar, Drawer, Content } from './style';
import  Sidebar  from '../sidebar';
import  Breadcrumbs  from '../breadCrumbs';
import  Title  from '../title';
import  Footer  from '../footer';
import paths from '@/utils/paths';
import theme from '@/theme';

function LayoutContent({ children, widthCard, heightCard, noCard }) {
	const { history } = useReactRouter();
	const [
		isOpen,
		setIsOpen
	] = useState(true);

	const handleToggleOpen = () => {
		setIsOpen(!isOpen);
	};

	const handleClose = () => {
		setIsOpen(false);
	};

	return (
		<Fragment>
			<Topbar sidebar={isOpen} onToggleSidebar={handleToggleOpen} />
			<Drawer anchor='left' onClose={handleClose} open={isOpen} variant='persistent'>
				<Sidebar />
			</Drawer>
			<Box display='flex'>
				<Content topbar sidebar={isOpen}>
					{children &&
					!noCard && (
						<Fragment>
							<Title text={paths.getTitle(paths.getCurrentPath(history))} />
							<Breadcrumbs />
							<Card width={widthCard} height={heightCard} padding={theme.spacing(1)}>
								{children}
							</Card>
						</Fragment>
					)}
					{children && noCard && children}
				</Content>
			</Box>
			<Footer />
		</Fragment>
	);
}

export default memo(LayoutContent);
