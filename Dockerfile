# Etapa Base (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 7070

# Etapa de Build (SDK)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia o arquivo de projeto e restaura as dependências
COPY ["POS_TECH_FASE_UM/POS_TECH_FASE_UM.csproj", "POS_TECH_FASE_UM/"]
COPY ["TESTES_POS_TECH_FASE_UM/TESTES_POS_TECH_FASE_UM.csproj", "TESTES_POS_TECH_FASE_UM/"]

# Restaurar as dependências de todos os projetos na solução
COPY POS_TECH_FASE_UM.sln ./
RUN dotnet restore "POS_TECH_FASE_UM.sln"

# Copia o restante dos arquivos e compila a aplicação
COPY . .
RUN dotnet build "POS_TECH_FASE_UM/POS_TECH_FASE_UM.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etapa de Publicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "POS_TECH_FASE_UM/POS_TECH_FASE_UM.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa 2: Criar a imagem final de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "POS_TECH_FASE_UM.dll"]
