import { ENUM_STATUS_ANALISE, ENUM_ITEMS_ANALISE } from '@/utils/constants';

export function getDisableEdit(user, analiseCadastro, statusAnaliseItem) {
	try {
		if (user.perfilAnalista) {
			if (
				analiseCadastro.StatusAnalise ===
					ENUM_STATUS_ANALISE.find(x => x.internalName === 'Em_Analise').value &&
				analiseCadastro.AtribuidoId === user.id
			) {
				return false;
			} else {
				return true;
			}
		} else {
			switch (analiseCadastro.StatusAnalise) {
				case null:
					return false;
				case ENUM_STATUS_ANALISE.find(x => x.internalName === 'Criado').value:
					return false;
				case ENUM_STATUS_ANALISE.find(x => x.internalName === 'Pendente_Analise').value:
					return true;
				case ENUM_STATUS_ANALISE.find(x => x.internalName === 'Reaberto').value:
					return false;
				case ENUM_STATUS_ANALISE.find(x => x.internalName === 'Reprovado').value:
					if (
						statusAnaliseItem ===
							ENUM_STATUS_ANALISE.find(x => x.internalName === 'Reprovado').value ||
						statusAnaliseItem === null
					) {
						return false;
					} else {
						return true;
					}
				case ENUM_STATUS_ANALISE.find(x => x.internalName === 'Em_Analise').value:
					return true;
				case ENUM_STATUS_ANALISE.find(x => x.internalName === 'Aprovado').value:
					return true;
				case ENUM_STATUS_ANALISE.find(x => x.internalName === 'Aprovado_Ressalvas').value:
					return true;
				default:
					return true;
			}
		}
	} catch (error) {
		return true;
	}
}

export function getStatusItem(itensAnalise, internalNameTipoItem) {
	if (itensAnalise.length === 0) return null;
	const itemTipo = ENUM_ITEMS_ANALISE.find(x => x.internalName === internalNameTipoItem);
	if (itemTipo && itemTipo.hasOwnProperty('value')) {
		const itemAnalise = itensAnalise.find(x => x.TipoItem === itemTipo.value);
		if (itemAnalise) {
			return itemAnalise.hasOwnProperty('Status') ? itemAnalise.Status : null;
		} else {
			return null;
		}
	} else {
		return null;
	}
}
