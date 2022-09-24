import http from '@/utils/http';

export default {
	login(login) {
		return http.request('/login', login);
	},

	resetPassword(login) {
		return http.request('/password/reset', login);
	},

	changePassword(password) {
		return http.request('/password/change', password);
	},

	register(user) {
		return http.request('/register', user);
	},

	emailConfirmationValidation(user) {
		return http.request('/user/emailconfirmationValidation', user);
	},

	getUsersInRole(roleName) {
		return http.request(`/User/GetUsersInRole/..{roleName}`, null, null, 'get');
	}
};
