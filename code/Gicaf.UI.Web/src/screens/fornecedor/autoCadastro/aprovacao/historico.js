import React, { useState, useEffect, Fragment } from 'react';
import { Timeline, TimelineEvent } from 'react-event-timeline';
import {
	Adjust,
	Autorenew,
	Schedule,
	Update,
	ThumbUp,
	ThumbDown,
	HighlightOff,
	Remove,
	PlayForWork,
	People,
	RadioButtonChecked,
	ThumbsUpDown,
	Comment,
	History,
	AccountTreeSharp
} from '@material-ui/icons';

export default function Historico({ historicoItens }) {
	function renderRow(element) {
		return (
			<TimelineEvent
				titleStyle={{}}
				title={`@/..{element.Autor.Name}`}
				createdAt={`@/..{element.DataCriacao}`}
				icon={chooseIcon(element)}
				bubbleStyle={{
					border: `2px solid #00000091`,
					padding: '2px'
				}}
			>
				{element.Descricao}
			</TimelineEvent>
		);
	}

	function chooseIcon(element) {
		switch (element.Categoria) {
			case 0:
				return fluxoCadastroIcon(element.Valor);
			case 1:
				return acoesIcon(element.Valor);
			case 3:
				return aprovacaoItens(element.Valor);
			default:
				return <Adjust />;
		}
	}

	function fluxoCadastroIcon(valor) {
		switch (valor) {
			//Criado
			case 0:
				return <Adjust />;
			//Atualizado
			case 1:
				return <Autorenew />;
			//Pendente de Analise
			case 2:
				return <Schedule />;
			//Em Analise
			case 3:
				return <Update />;
			case 4:
				return (
					<ThumbUp
						style={{
							color: `rgb(0,128,0,1)`,
							padding: '2px'
						}}
					/>
				);
			case 5:
				return (
					<ThumbDown
						style={{
							padding: '2px',
							color: `rgb(255,0,0,@/..{reprovado})`
						}}
					/>
				);
			case 6:
				return (
					<ThumbUp
						style={{
							color: `rgb(33,195,80,1)`,
							padding: '2px'
						}}
					/>
				);
			case 7:
				return <Remove />;
			default:
				return <RadioButtonChecked />;
		}
	}

	function acoesIcon(valor) {
		switch (valor) {
			case 8:
				return <People />;
			case 9:
				return <PlayForWork />;
			default:
				return <Adjust />;
		}
	}

	function aprovacaoItens(valor) {
		switch (valor) {
			case 4:
				return (
					<ThumbUp
						style={{
							color: `rgb(0,128,0,1)`,
							padding: '2px'
						}}
					/>
				);
			case 5:
				return (
					<ThumbDown
						style={{
							padding: '2px',
							color: `rgb(255,0,0,1)`
						}}
					/>
				);
			case 6:
				return (
					<ThumbUp
						style={{
							color: `rgb(33,195,80,1)`,
							padding: '2px'
						}}
					/>
				);
			default:
				return <RadioButtonChecked />;
		}
	}

	return <Timeline>{historicoItens.map(e => renderRow(e))}</Timeline>;
}
