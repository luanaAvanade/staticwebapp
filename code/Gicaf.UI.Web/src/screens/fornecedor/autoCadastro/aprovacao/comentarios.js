import React, { useState, useEffect, Fragment } from 'react';
import { Timeline, TimelineEvent } from 'react-event-timeline';
import { Comment } from '@material-ui/icons';
import { Box } from '@material-ui/core';
import { maxHeight } from '@material-ui/system';

export default function Comentarios({ comentarios }) {
	function format(dataCriacao) {
		let data = new Date(dataCriacao);
		return `@/..{doisDigitos(data.getDate())}/@/..{doisDigitos(
			data.getMonth() + 1
		)}/@/..{data.getFullYear()} @/..{doisDigitos(data.getHours())}:@/..{doisDigitos(data.getMinutes())}`;
	}

	function doisDigitos(value) {
		if (value < 10) {
			return `0@/..{value}`;
		} else {
			return value;
		}
	}

	function renderRow(element) {
		return (
			<TimelineEvent
				titleStyle={{}}
				title={`@/..{element.hasOwnProperty('Usuario') ? element.Usuario.Nome : ''}`}
				createdAt={`@/..{element.hasOwnProperty('DataCriacao') ? format(element.DataCriacao) : ''}`}
				icon={<Comment color='primary' fontSize='small' />}
				bubbleStyle={{
					border: `2px solid #00000091`,
					padding: '2px'
				}}
			>
				{element.Coment}
			</TimelineEvent>
		);
	}

	return (
		<Box style={{ overflowY: 'scroll', maxHeight: '280px' }}>
			<Timeline>{comentarios.map(e => renderRow(e))}</Timeline>
		</Box>
	);
}
