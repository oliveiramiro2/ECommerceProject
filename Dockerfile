# --- Estágio 1: Compilação e Build ---
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia o arquivo de solução e os arquivos de projeto individualmente para otimizar o cache de camadas
COPY ["ECommerce.slnx", "./"]
COPY ["ECommerce.Domain/ECommerce.Domain.csproj", "ECommerce.Domain/"]
COPY ["ECommerce.Application/ECommerce.Application.csproj", "ECommerce.Application/"]
COPY ["ECommerce.Infrastructure/ECommerce.Infrastructure.csproj", "ECommerce.Infrastructure/"]
COPY ["ECommerce.API/ECommerce.API.csproj", "ECommerce.API/"]
COPY ["ECommerce.Tests.Unit/ECommerce.Tests.Unit.csproj", "ECommerce.Tests.Unit/"]
COPY ["ECommerce.Tests.Integration/ECommerce.Tests.Integration.csproj", "ECommerce.Tests.Integration/"]

# Restaura as dependências NuGet de todos os projetos
RUN dotnet restore "ECommerce.slnx"

# Copia todo o restante do código fonte
COPY . .
WORKDIR "/src/ECommerce.API"

# Compila e publica a aplicação em modo Release
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# --- Estágio 2: Imagem Final de Execução (Runtime) ---
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Expõe a porta padrão da aplicação web
EXPOSE 8080

# Copia os arquivos publicados do estágio anterior
COPY --from=build /app/publish .

# Define o ponto de entrada executando a API
ENTRYPOINT ["dotnet", "ECommerce.API.dll"]