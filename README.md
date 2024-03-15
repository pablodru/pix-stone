# API-PIX

Este projeto consiste numa API que simula o papel do Banco Central no PIX, os usuários da API seriam as Payment Service Provider (PSP) que utilizam para criar chaves pix e pagamentos.

## Instalação e Execução 🚀

Para rodar o projeto localmente, siga os seguinter passos:

1. Clone o repositório: `git clone https://github.com/pablodru/pix-stone`;
2. Acesse o diretório do projeto: `cd pix-stone`;
3. Construa e inicie os contâineres docker: `docker-compose up --build`:
4. Executar migrações do banco de dados: `docker-compose run migrate`;
5. Inicie o RabbitMQ com: `cd RabbitMQ` e depois `docker compose up -d`;
6. Depois disso, a aplicação estará disponível em: `http://localhost:5109`;
Nota: Se necessário, personalize as configurações no arquivo docker-compose.yml para atender às suas necessidades específicas.

## Tecnologias 🔧

Para a construção do projeto foi utilizado as seguintes tecnologias:

- .Net: v8.0.202
- Entity Framework Core: v8.0.2
- PostgreSQL
- RabbitMQ
- Docker
- Grafana K6
- Knex
