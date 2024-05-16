## English Version
# Project Overview
# Purpose
* The MotorcycleRental API is designed to facilitate motorcycle rentals, managing motorcycles, plans, rentals, and deliveries through a RESTful interface.

# Technology Stack
* .NET Core 8
* MongoDB
* MediatR
* Docker
* Kafka
## Getting Started
# Prerequisites
* Docker
* Docker Compose
* Zookeeper
* Kafka
# Installation
# Install Docker and Docker Compose.
* Navigate to the docker-compose-mongo-db directory and run docker-compose up to set up the MongoDB database.
* Navigate to the docker-compose-kafka directory and run docker-compose up to set up the Kafka message broker.
# The API automatically initializes the database on startup, creating the necessary collections and seeding them with initial data:
*  Deliveries
*  Motorcycles
*  Rentals
*  Plans

## API Endpoints
# Motorcycles
* GET /api/motorcycles: Retrieve all motorcycles.
* POST /api/motorcycles: Add a new motorcycle.
* GET /api/motorcycles/{id}: Fetch a motorcycle by ID.
* PUT /api/motorcycles/{id}: Update a motorcycle.
* DELETE /api/motorcycles/{id}: Remove a motorcycle.
# Deliveries
* GET /api/deliveries: List all deliveries.
* POST /api/deliveries: Register a new delivery.
* GET /api/deliveries/{id}: Get a delivery by ID.
* PUT /api/deliveries/{id}: Update a delivery.
* DELETE /api/deliveries/{id}: Delete a delivery.
# Plans
* GET /api/plans: Fetch all plans.
* POST /api/plans: Create a plan.
* GET /api/plans/{id}: Retrieve a plan by ID.
* PUT /api/plans/{id}: Modify a plan.
* DELETE /api/plans/{id}: Remove a plan.
# Rentals
* GET /api/rentals: List all rentals.
* POST /api/rentals: Initiate a new rental.
* GET /api/rentals/{id}: Retrieve details of a specific rental.
* PUT /api/rentals/{id}: Update rental information.
* DELETE /api/rentals/{id}: Delete a rental.
* GET /api/rentals/calculateTotalCost/{rentalId}: Calculate the total cost of a rental based on actual return date.
## Error Handling
* Common API errors include 404 for resource not found, 400 for bad requests, and 500 for internal server errors.

# Versioning
* The API may be versioned in future releases to ensure backward compatibility.

# Contact and Support
Contact details for API support or community forums should be listed here.
ediorhoden@gmail.com

##Versão em Português
#Visão Geral do Projeto
#Propósito
* A API MotorcycleRental é projetada para facilitar aluguéis de motocicletas, gerenciando motocicletas, planos, aluguéis e entregas por meio de uma interface RESTful.

# Pilha de Tecnologia
* .NET Core 8
* MongoDB
* MediatR
* Docker
* Kafka
# Primeiros Passos
# Pré-requisitos
* Docker
* Docker Compose
* Zookeeper
* Kafka
# Instalação
#Instale o Docker e o Docker Compose.
* Navegue até o diretório docker-compose-mongo-db e execute docker-compose up para configurar o banco de dados MongoDB.
* Navegue até o diretório docker-compose-kafka e execute docker-compose up para configurar o broker de mensagens Kafka.
# A API inicializa automaticamente o banco de dados na inicialização, criando as coleções necessárias e populando-as com dados iniciais:
*  Deliveries
*  Motorcycles
*  Rentals
*  Plans
# Endpoints da API
# Motocicletas
* GET /api/motorcycles: Recupera todas as motocicletas.
* POST /api/motorcycles: Adiciona uma nova motocicleta.
* GET /api/motorcycles/{id}: Busca uma motocicleta pelo ID.
* PUT /api/motorcycles/{id}: Atualiza uma motocicleta.
* DELETE /api/motorcycles/{id}: Remove uma motocicleta.
# Entregas
* GET /api/deliveries: Lista todas as entregas.
* POST /api/deliveries: Registra uma nova entrega.
* GET /api/deliveries/{id}: Obtém uma entrega pelo ID.
* PUT /api/deliveries/{id}: Atualiza uma entrega.
* DELETE /api/deliveries/{id}: Deleta uma entrega.
# Planos
* GET /api/plans: Busca todos os planos.
* POST /api/plans: Cria um plano.
* GET /api/plans/{id}: Recupera um plano pelo ID.
* PUT /api/plans/{id}: Modifica um plano.
* DELETE /api/plans/{id}: Remove um plano.
# Aluguéis
* GET /api/rentals: Lista todos os aluguéis.
* POST /api/rentals: Inicia um novo aluguel.
* GET /api/rentals/{id}: Recupera detalhes de um aluguel específico.
* PUT /api/rentals/{id}: Atualiza informações do aluguel.
* DELETE /api/rentals/{id}: Deleta um aluguel.
* GET /api/rentals/calculateTotalCost/{rentalId}: Calcula o custo total de um aluguel baseado na data de retorno real.
# Tratamento de Erros
*  Erros comuns da API incluem 404 para recurso não encontrado, 400 para solicitações ruins e 500 para erros internos do servidor.

# Versionamento
* A API pode ser versionada em lançamentos futuros para garantir compatibilidade com versões anteriores.

# Contato e Suporte
Detalhes de contato para suporte à API ou fóruns da comunidade devem ser listados aqui.
ediorhoden@gmail.com
