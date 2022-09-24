import http from '@/utils/http';

export default {
	buscarPorCep(cep) {
		return http.requestExternal(
			`https://proxier.now.sh/api?url=https://viacep.com.br/ws/..{cep}/json/`
		);
	}
};
