# JobsityChallenge Project

## Overview
**JobsityChallenge** is a simple chat application that demonstrates user registration, authentication, chat room management, real-time messaging, and a bot service for stock price requests.  
The application is built with **.NET 8, Docker, RabbitMQ, SQL Server, and SignalR**, showcasing backend and frontend functionality for real-time communication and third-party service integration.

### Architecture
The solution consists of three main components:
- **API (ASP.NET Core Web API)**: Handles authentication, chat rooms, and messaging.
- **Client (Blazor WebAssembly)**: Provides a web-based chat interface.
- **Bot Service (Console App + RabbitMQ Consumer)**: Processes stock commands and replies with stock quotes.

Message flow: Client → API → RabbitMQ → Bot → API → SignalR → Client

## Features
- **User Registration and Authentication**: Secure registration and login with JWT tokens.
- **Chat Room Creation**: Users can create chat rooms through the interface.
- **Real-Time Chat**: Join rooms and exchange messages instantly via SignalR.
- **Stock Quote Bot**: Messages in the format `/stock=<symbol>` trigger the bot to fetch stock prices from [Stooq](https://stooq.com).
- **RabbitMQ Integration**: Enables communication between the Web API and the Bot service.

## Tech Stack
- **Backend**: ASP.NET Core 8 (Web API)
- **Frontend**: Blazor WebAssembly
- **Real-Time Communication**: SignalR
- **Message Broker**: RabbitMQ
- **Database**: SQL Server (Entity Framework Core)
- **Containerization**: Docker & Docker Compose


## Prerequisites
- [Docker](https://docs.docker.com/get-docker/) & Docker Compose
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (for local development/testing)

## Setup and Installation

1. Unzip solution
Unzip jobsity-challenge.zip

run:
cd JobsityChallenge

2. Build and Run with Docker Compose
Build and start all services with Docker Compose

run:
docker-compose up --build

3. Access the Application
- Frontend (Client): http://localhost:5002
- Web API (Swagger): http://localhost:5001/swagger
- RabbitMQ Management UI: http://localhost:15672 (username: guest, password: guest)

## Usage
- Authentication
Register: Create an account through the Blazor frontend.
Login: Authenticate with your credentials and receive a JWT token. After login, you are redirected to the chat rooms page.

- Real-Time Chat
Create or join a chat room.
Messages with the format /stock=<symbol> will trigger the bot to fetch stock information.

- Bot Service
The BotService listens for messages starting with /stock= and fetches stock prices from a third-party API (https://stooq.com). Results are posted back to the chat room.

## Testing
Unit tests are available in the tests folder. To run tests:

run:
cd tests/JobsityChallenge.Tests
dotnet test

## Troubleshooting
- Database Connection Issues: Ensure SQL Server is running and that the connection string in appsettings.json is correct.
- RabbitMQ Port Conflicts: If ports 5672 or 15672 are already in use, update the docker-compose.override.yml with different mapping
- Docker Build Issues: Rebuild containers with docker-compose down followed by docker-compose up --build.

