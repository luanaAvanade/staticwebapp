import i18n from 'i18next';
import LanguageDetector from 'i18next-browser-languagedetector';
import pt from './pt';
import en from './en';
import { htmlParser } from '@/utils/string';

i18n.use(LanguageDetector).init({
	resources: { pt, en },
	fallbackLng: 'pt',
	debug: true,
	ns: [
		'translations'
	],
	defaultNS: 'translations',
	keySeparator: false,
	interpolation: {
		escapeValue: false,
		formatSeparator: ','
	},
	react: {
		wait: true
	}
});

const translate = key => i18n.t(key);

const translateWithHtml = key => htmlParser(i18n.t(key));

export { i18n, translate, translateWithHtml };
