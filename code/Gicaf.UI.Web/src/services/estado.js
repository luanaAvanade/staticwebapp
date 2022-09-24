import http from '@/utils/http';

export default {
	findAll() {
		return http.request('/gql', {
			query: `query{
					Estado_list{
						value:Id
						label:Nome
						Sigla
					}
				}
			`
		});
	}
};
