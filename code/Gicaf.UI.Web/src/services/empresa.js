import http from '@/utils/http';
import moment from 'moment';

export default {
	create(empresa) {
		return http.request('/gql', {
			query: `mutation($empresa: EmpresaCreateInput!) {
				Empresa_insert (input: $empresa)
					{
						Id
					}
				}
			`,
			variables: { empresa }
		});
	},

	update(id, empresa) {
		return http.request('/gql', {
			query: `mutation(..id:ID! ..empresa: EmpresaUpdateInput!) {
				Empresa_update (id:..id input:..empresa)
					{
						Id
						NomeEmpresa
						CNPJ
						InscricaoEstadual
						IsentoIE
						InscricaoMunicipal
						OptanteSimplesNacional
						DataAbertura
						AtividadeEconomicaPrincipalId
						OcupacaoPrincipalId
						TipoEmpresa
						TipoCadastro
						CapitalSocialTotalSociedade
						DataRegistroSociedade
						ContatoSolicitante {
							NomeContato
							Email
						} 
						ContatosAdicionais {
							Id
							NomeContato
							Telefone
							Email
							TipoContatoId
						}
						Usuarios { 
							Id
							Nome
							CPF
							Telefone
							Celular
							Email
							CargoEmpresa
						}
						Enderecos { 
							Id
							CEP
							TipoEndereco,
							Logradouro,
							Numero,
							Complemento,
							Bairro,
							Municipio{
								Id
								CodIBGE
								EstadoId
							}
						}
						DadosPessoaFisica {
							Id
							DataNascimento
							CPF
							PisPasepNit
							Sexo
							Municipio{
								Id
								CodIBGE
								EstadoId
							}
						}
						GruposFornecimento {
							GrupoCategoria{
								Id
								Codigo
								Nome
								Categoria{
										Codigo
										Descricao
										Tipo
								}
							}
							TipoFornecimento
						}
						DadosBancarios{
							Id,
							Banco{
								Id
							}
							Agencia
							DigitoAgencia
							Conta
							DigitoConta
						}
						Socios{
							Id
							TipoPessoa
							ValorParticipacao
							Codigo
							Nome
							Administrador
						}
						GruposDeAssinatura{
							Id
							ValorLimite
							TipoAssinatura
							Assinaturas{
								Id
								SocioId
								Obrigatoriedade
							}
						}
						TermoAceiteEmpresa
	          {
		        	Aceite
			        EmpresaId
		        	Id
		        	TermosAceiteId
		       }
						AnaliseCadastro{
							Id
							StatusAnalise
							AtribuidoId
							TransmitidoId
							ItensAnalise{
								Id
								TipoItem
								AutorId
								Status
								ArquivoId
								Justificativa
							}
						}
					}
				}
			`,
			variables: { id, empresa }
		});
	},

	findByCnpj(cnpj) {
		return http.request('/gql', {
			query: `query{
				Empresa_list(where:"CNPJ=\\"..{cnpj}\\""){
					Id
					CNPJ
					NomeEmpresa
					DataAbertura
					Enderecos{
						CEP
					}
					AtividadeEconomicaPrincipalId
					OcupacaoPrincipalId
					}
				}
			`
		});
	},

	findById(Id) {
		return http.request('/gql', {
			query: `query{
				Empresa_list(where:"Id=\\"..{Id}\\""){
					Id
					NomeEmpresa
					CNPJ
					InscricaoEstadual
					IsentoIE
					InscricaoMunicipal
					OptanteSimplesNacional
					DataAbertura
					AtividadeEconomicaPrincipalId
					OcupacaoPrincipalId
					TipoEmpresa
					TipoCadastro
					CapitalSocialTotalSociedade
					DataRegistroSociedade
					Situacao
					TermoAceiteEmpresa{
						Id
						Aceite 
						EmpresaId 
						TermosAceiteId
						TermosAceite
					
						{
							Titulo
							SubTitulo
							Status
							Descricao
						}
					}
					
					ContatoSolicitante {
						Id
						NomeContato
						Email
					} 
					ContatosAdicionais {
						Id
						NomeContato
						Telefone
						Email
						TipoContatoId
					}
					Usuarios { 
						Id
						Nome
						CPF
						Telefone
						Celular
						Email
						CargoEmpresa
					}
					Enderecos { 
						Id
						CEP
						TipoEndereco,
						Logradouro,
						Numero,
						Complemento,
						Bairro,
						Municipio{
							Id
							CodIBGE
							EstadoId
						}
					}
					DadosPessoaFisica {
						Id
						DataNascimento
						CPF
						PisPasepNit
						Sexo
						Municipio{
							Id
							CodIBGE
							EstadoId
						}
					}
					GruposFornecimento {
						GrupoCategoria{
							Id
							Codigo
							Nome
							Categoria{
									Codigo
									Descricao
									Tipo
							}
							GruposFornecimento(where:"EmpresaId != \\"..{Id}\\""){
								Empresa{
									Id,
									NomeEmpresa
								}
							}
						}
						TipoFornecimento
					}
					DadosBancarios{
						Id,
						Banco{
							Id
						}
						Agencia
						DigitoAgencia
						Conta
						DigitoConta
					}
					DadosBalancosPatrimoniais {
						Id
						DataReferencia 
						AtivoTotal 
						CirculanteAtivo  
						Disponibilidades 
						Estoques 
						OutrosAtivosCirculante 
						AtivoNaoCirculante 
						PassivoTotal 
						CirculantePassivo 
						EmprestimosFinanciamentoCirculante 
						OutrosPassivosCirculantes 
						NaoCirculantePassivo 
						EmprestimosFinanciamentoNaoCirculante 
						OutrosPassivosNaoCirculantes 
						PatrimonioLiquido
					}
					DadosDREs {
						Id
						DataReferencia 
						ReceitaOperacionalLiquida
						CustoProdutosVendidosMercadoriasVendidasServicosPrestados
						ResultadoOperacionalBruto
						DespesasVendasAdministrativasGeraisOutras
						DespesasFinanceiras
						ReceitasFinanceiras
						ResultadoOperacionalAntesIrCssl
						ResultadoLiquidoPeriodo
					}
					Socios{
						Id
						TipoPessoa
						ValorParticipacao
						Codigo
						Nome
						Administrador
					}
					GruposDeAssinatura{
						Id
						ValorLimite
						TipoAssinatura
						Assinaturas{
							Id
							SocioId
							Obrigatoriedade
						}
					}
					Documentos {
						DataBasePeriodo
						TipoDocumento{
								Id
								Nome
								TipoDocumentoFuncionalidade{
                   Funcionalidade
									 Obrigatorio
									}
						}
						Arquivo {
								Id
							  NomeArquivo
								CodigoExterno
						}
					}
					AnaliseCadastro{
						Id
						StatusAnalise
						AtribuidoId
						TransmitidoId
						ItensAnalise{
							Id
							TipoItem
							AutorId
							Status
							ArquivoId
							Justificativa
						}
					}
					Comentarios{
						Id
						Local
						Coment
						DataCriacao
						Usuario{
							Id
							Nome
						}	
					}
					CalculoRiscoLista{
							Data
							CCL
							NIG
							SD
							ClassificacaoFase1
							LC
							LS
							EG
							CE
							ALDB
							ALDL
							AL
							ICJ
							ROE
							ME
							ML
							GA
							RiscoLC
							RiscoLS
							RiscoEG
							RiscoCE
							RiscoALDB
							RiscoALDL
							RiscoAL
							RiscoICJ
							RiscoROE
							RiscoME
							RiscoML
							RiscoGA
						}				
				}
			}`
		});
	},

	findCalculoRiscoByGrupoCategoriaId(GrupoCategoriaId) {
		return http.request('/gql', {
			query: `query {
				Empresa_list(where:"GruposFornecimento.Any(GrupoCategoriaId=..{GrupoCategoriaId})"){
					Id,
					NomeEmpresa,
					CalculoRiscoLista{
							Data
							CCL  
							NIG  
							SD  
							ClassificacaoFase1 
							LC
							LS
							EG
							CE
							ALDB
							ALDL
							AL
							ICJ
							ROE
							ME
							ML
							GA
							RiscoLC
							RiscoLS
							RiscoEG
							RiscoCE
							RiscoALDB
							RiscoALDL
							RiscoAL
							RiscoICJ
							RiscoROE
							RiscoME
							RiscoML
							RiscoGA
						}
					}
				}
			`
		});
	},

	findByCnpjReceita(cnpj) {
		return http.requestExternal(
			`https://cors-anywhere.herokuapp.com/http://www.receitaws.com.br/v1/cnpj/..{cnpj}`
		);
	},
	removeDocumento(id) {
		// return Http.request('/gql', {
		// 	query: `mutation(..id:ID!) {
		// 		Arquivo_delete (id:..id){ Id }
		// 	}
		// 	`,
		// 	variables: { id }
		// });
	},

	importDocumentoDadosFinanceiros(documentacao, empresaId) {
		const formData = new FormData();
		const docFormatado = [];
		documentacao.forEach(doc => {
			//	let fileDoc = doc.files && doc.files.length > 0 ? doc.files[0].file : 'undefined';
			docFormatado.push(
				`
					{
						TipoDocumentoId: ..{doc.tipoId}
						DataBasePeriodo: \\"..{moment(
							doc.files && doc.files.length > 0 ? doc.files[0].dataEmissao : new Date()
						).format('YYYY-MM-DD')}\\"
						Arquivo: {
							Key: \\"file..{docFormatado.length}\\"
						}
						TipoDocumento: {
							Codigo: ..{doc.codigo}
						}
					}`
			);
			formData.append(
				`file..{(docFormatado.length - 1).toString()}`,
				doc.files && doc.files.length > 0 ? doc.files[0].file : 'undefined'
			);
		});

		const query = `{
				"query":"mutation {
					Empresa_update (id: ..{empresaId}
						input:{
							Documentos: [
								 ..{docFormatado}
										]
								})
						{
							Id
							DadosBalancosPatrimoniais{
								Id
								DadosCalculadosPeloRobo
								DataReferencia
								AtivoTotal
								CirculanteAtivo
								Disponibilidades
								Estoques
								OutrosAtivosCirculante
								AtivoNaoCirculante
								PassivoTotal
								CirculantePassivo
								EmprestimosFinanciamentoCirculante
								OutrosPassivosCirculantes
								NaoCirculantePassivo
								EmprestimosFinanciamentoNaoCirculante
								OutrosPassivosNaoCirculantes
								PatrimonioLiquido 							 
								}
								DadosDREs
                {
                DadosCalculadosPeloRobo
                DataReferencia
                ReceitaOperacionalLiquida
                CustoProdutosVendidosMercadoriasVendidasServicosPrestados
                ResultadoOperacionalBruto
                DespesasVendasAdministrativasGeraisOutras
                DespesasFinanceiras
                ReceitasFinanceiras
                ResultadoOperacionalAntesIrCssl
                ResultadoLiquidoPeriodo
								}
							}
					}"}`;

		formData.append('query', query);

		const config = { headers: { 'Content-Type': 'multipart/form-data' } };

		return http.request('/gql/file', formData, config);
	},

	importDocumento(documentacao, empresaId) {
		const formData = new FormData();
		const docFormatado = [];
		documentacao.forEach(doc => {
			docFormatado.push(
				`
				{
					TipoDocumentoId: ..{doc.TipoDocumentoId}
					DataBasePeriodo: \\"..{doc.DataBasePeriodo}\\"
					Arquivo: {
						Key: \\"file..{docFormatado.length}\\"
					}
				}`
			);
			formData.append(`file..{(docFormatado.length - 1).toString()}`, doc.Arquivo);
		});
		const query = `{
				"query":"mutation {
					Empresa_update (id: ..{empresaId}
						input:{
							Documentos: 
											[
												..{docFormatado}
											]
								})
						{
							Id
						}
					}"
				}`;
		formData.append('query', query);
		const config = { headers: { 'Content-Type': 'multipart/form-data' } };

		return http.request('/gql/file', formData, config);
	},

	findEmpresasWithWhereClause(Where) {
		return http.request('/gql', {
			query: `query{
				Empresa_list(where:"..{Where}" orderBy:"DataCriacao desc"){
					Id
					NomeEmpresa
					CNPJ
					TipoEmpresa
					TipoCadastro
					DataCriacao
					Situacao
					ContatoSolicitante {
						NomeContato
						Email
					} 
					Enderecos { 
						Municipio{
							Id
							CodIBGE
							EstadoId
						}
					}
					GruposFornecimento {
						GrupoCategoria{
							Id
							Codigo
							Nome
							Categoria{
									Codigo
									Descricao
									Tipo
							}
						}
						TipoFornecimento
					}
					Socios{
						Id
						Codigo
					}					
					AnaliseCadastro{
						Id
						StatusAnalise
						AtribuidoId
					    Atribuido{
					        Id
					        Nome
							}
						TransmitidoId
					    Transmitido{
					        Id
					        Nome
							}						
					}
					CalculoRiscoLista(take: 1 orderBy:"\\"Data desc\\""){
						ClassificacaoFase1
					}	
				}
			}`
		});
	},
	findEmpresasWithWhereClausePaged(Where, Take, Skip, Order) {
		return http.request('/gql', {
			query: `query{
				Empresa_list(take:..{Take} skip:..{Skip} paged: true where:"..{Where}" orderBy:"DataCriacao desc"){
					Id
					NomeEmpresa
					CNPJ
					TipoEmpresa
					TipoCadastro
					DataCriacao
					Situacao
					ContatoSolicitante {
						NomeContato
						Email
					} 
					Enderecos { 
						Municipio{
							Id
							CodIBGE
							EstadoId
						}
					}
					GruposFornecimento {
						GrupoCategoria{
							Id
							Codigo
							Nome
							Categoria{
									Codigo
									Descricao
									Tipo
							}
						}
						TipoFornecimento
					}
					Socios{
						Id
						Codigo
					}					
					AnaliseCadastro{
						Id
						StatusAnalise
						AtribuidoId
					    Atribuido{
					        Id
					        Nome
							}
						TransmitidoId
					    Transmitido{
					        Id
					        Nome
							}						
					}
					CalculoRiscoLista(take: 1 orderBy:"\\"Data desc\\""){
						ClassificacaoFase1
					}	
				}
			}`
		});
	}
};
