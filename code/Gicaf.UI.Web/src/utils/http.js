import axios from 'axios';
import { getUser } from '@/utils/auth';

const api = axios.create({
	baseURL: window.globalConfig.UrlApi
});

api.interceptors.request.use(async config => {
	const user = getUser();
	if (user) {
		config.headers.Authorization = `Bearer ..{user.accessToken}`;
	}
	return config;
});

const apiExternal = axios.create({
	headers: {
		'Access-Control-Allow-Origin': '*',
		'Access-Control-Allow-Headers': 'Authorization',
		'Access-Control-Allow-Methods': 'GET, POST, OPTIONS, PUT, PATCH, DELETE',
		'Content-Type': 'application/json;charset=UTF-8'
	}
});

export default class Http {
	static request(url, data, config, method = 'post') {
		return new Promise((resolve, reject) => {
			api({ method, url, data, config })
				.then(result => resolve(result.data))
				.catch(err => reject(err));
		});
	}

	static requestExternal(url, data, config) {
		return new Promise((resolve, reject) => {
			apiExternal({ method: 'GET', url, data, config })
				.then(result => {
					resolve(result.data);
					console.log(result.data);
				})
				.catch(err => reject(err));
		});
	}
}
