import { translate } from '@/locales';

export const COLUMNS_DADOS_CONTATOS_ADICIONAIS = [
	{
		id: 'tipoContato',
		label: translate('tipoContato'),
		title: translate('tipoContato'),
		width: 150
	},
	{ id: 'NomeContatoAdicional', label: translate('nomeContatoAdicional'), width: 420 },
	{
		id: 'telefone',
		label: translate('telefone'),
		width: 200
	},
	{
		id: 'email',
		title: translate('email'),
		label: translate('email'),
		width: 200
	},
	{
		id: 'acoes',
		label: translate('acoes'),
		width: '5%',
		colSpan: 2,
		align: 'center'
	}
];
