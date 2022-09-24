SELECT Categoria.Codigo, Pergunta.Codigo, Pergunta.Nome,
	   dbo.fn_CalcularModa(CategoriaId, PerguntaId) AS Moda, 
	   dbo.fn_CalcularMedia(CategoriaId, PerguntaId) AS Media, 
	   dbo.fn_CalcularMediana(CategoriaId, PerguntaId) AS Mediana
FROM
(SELECT DISTINCT CategoriaId, PerguntaId
from Resposta) AS Resposta
INNER JOIN Categoria ON Categoria.Id = Resposta.CategoriaId
INNER JOIN Pergunta ON Pergunta.Id = Resposta.PerguntaId