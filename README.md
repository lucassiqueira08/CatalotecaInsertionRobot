# Robô de inserção Cataloteca
Robô de inserção de dados da planilha para o banco de dados;

## Pré requisitos da planilha
É necessário que a planilha esteja em formato .xlsx e tenha os campos a seguir no Header da planilha:
 * Name: Nome/Descrição curta do produto
 * Description: Descrição longa do produto

## Dados de entrada
Ao executar o programa, insira os dados abaixo:
 * Host: Endereço do **SQL SERVER** no qual os dados serão inseridos. Default: localhost
 * DB Name: Nome do schema no qual os dados serão inseridos. Default: cataloteca
 * File Path: Caminho completo do arquivo. Exemplo: C:\Users\teste\CatalotecaInsertionRobot\data\BASE.xlsx

