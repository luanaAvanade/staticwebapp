export default theme => ({
	navItem: {
		marginTop: 2,
		paddingLeft: 24,
		width: '100%'
	},
	activeNavItem: {
		paddingLeft: 26,
		paddingTop: 2,
		backgroundColor: theme.palette.primary.light,
		'& $listItemText': {
			fontWeight: 500,
			color: theme.palette.text.primary
		},
		width: '100%'
	},
	listItemText: {
		color: theme.palette.text.secondary
	},
	list: {
		padding: 0
	}
});
