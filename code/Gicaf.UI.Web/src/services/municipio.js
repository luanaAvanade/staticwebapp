import http from '@/utils/http';

export default {
	findByEstado(estado) {
		return http.request('/gql', {
			query: `query{
					Municipio_list(where:"EstadoId = ${estado}"){
  					value:Id
						CodIBGE
						label:Nome
					}
				}
			`
		});
	}
};
