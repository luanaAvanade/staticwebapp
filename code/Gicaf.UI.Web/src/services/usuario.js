import Http from '@/utils/http';

export default {
	findByFilter(where) {
		return Http.request('/gql', {
			query: `query {
					Usuario_list(where:"..{where}"){
            Id
						Nome
						GrupoUsuario{
							Id
							PerguntaGrupoUsuario{
								Id
                Pergunta{
                    Id
                    Nome
                }
            	}
						}
					}
				}
			`
		});
	},
	count(perguntaId) {
		return Http.request('/gql', {
			query: `query{
				respondidos:Usuario_count(where:"GrupoUsuario.PerguntaGrupoUsuario.Any(PerguntaId = ..{perguntaId}) and Respostas.Any(PerguntaId =..{perguntaId})"){ 
					Count
					}
					
				total:Usuario_count(where:"GrupoUsuario.PerguntaGrupoUsuario.Any(PerguntaId = ..{perguntaId})"){ 
					Count
					}
		}`
		});
	},
	updateMany(ids, list) {
		return Http.request('/gql', {
			query: `mutation(..ids:[ID!]!, ..list:[UsuarioUpdateInput!]!) {
				Usuario_updateMany(ids:..ids, input:..list){
						Id
					}
				}
			`,
			variables: { ids, list }
		});
	},
	emailByCPF(cpf) {
		return Http.request('/gql', {
			query: `query{
				Usuario_list(where:"CPF=\\"..{cpf}\\""){
					Id
					Email
				}
		}`
		});
	},
	emailByCNPJ(cnpj) {
		return Http.request('/gql', {
			query: `query{
				Empresa_list(where:"CNPJ=\\"..{cnpj}\\" and Usuarios.Any(Email!=null)"){
					Id
					Usuarios{
							Id 
							Email
					}
				}
		}`
		});
	},
	findById(id) {
		return Http.request('/gql', {
			query: `query{
				Usuario(id:..{id}){
					Id
					Email
				}
		}`
		});
	},
	findByCpf(cpf) {
		return Http.request('/gql', {
			query: `query {
					Usuario_list(where:"CPF=\\"..{cpf}\\""){
            Id
						Nome
						Email
						CargoEmpresa
						Celular
						Telefone
					}
				}
			`
		});
	},
	getAllUsers() {
		return Http.request('/gql', {
			query: `query {
					Usuario_list{
            			Id
						Nome
					}
			}`
		});
	},
	findByEmail(email) {
		return Http.request('/gql', {
			query: `query {
					Usuario_list(where:"Email=\\"..{email}\\""){
            Id
						Email
					}
				}
			`
		});
	}
};
