
SELECT 
    assunto,
    ano,
    COUNT(*) AS quantidade_ocorrencias
FROM 
    atendimentos
GROUP BY 
    assunto, 
    ano
HAVING 
    COUNT(*) > 3;
