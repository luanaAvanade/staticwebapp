import React, { Fragment } from 'react';
import { Box } from '@material-ui/core';
import { Button } from 'react-axxiom';
import theme from '@/theme';
import { translate } from '@/locales';
import {ENUM_STATUS_ANALISE} from '@/utils/constants';

export function StatusEmpresa({
	statusEmpresa
}) {
	return (
		<Box
            style={{
                height: 'fit-content',
                backgroundColor: ENUM_STATUS_ANALISE.find(x => x.value === statusEmpresa).color,
                color: 'white',
                fontSize: 'bold',
                fontWeight: 'bold',
                textAlign: 'center',
                borderRadius: '2px',
                fontSize: 'small',
                padding: '2px 16px 2px 16px'
            }}
        >
            {ENUM_STATUS_ANALISE.find(x => x.value === statusEmpresa).label}
        </Box>
	);
}
