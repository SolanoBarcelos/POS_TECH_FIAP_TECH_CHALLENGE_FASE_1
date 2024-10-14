# Etapa 1: Construir a aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar o arquivo de solução (.sln)
COPY ["POS_TECH_FASE_UM.sln", "./"]

# Copiar os arquivos .csproj de cada projeto
COPY ["POS_TECH_FASE_UM/POS_TECH_FASE_UM.csproj", "POS_TECH_FASE_UM/"]
COPY ["TESTES_POS_TECH_FASE_UM/TESTES_POS_TECH_FASE_UM.csproj", "TESTES_POS_TECH_FASE_UM/"]

# Restaurar as dependências de todos os projetos na solução
RUN dotnet restore "./POS_TECH_FASE_UM.sln"

# Copiar o restante dos arquivos do projeto
COPY . .

# Compilar todos os projetos da solução
RUN dotnet build "./POS_TECH_FASE_UM.sln" -c Release -o /app/build

# Publicar a aplicação principal (POS_TECH_FASE_UM)
RUN dotnet publish "./POS_TECH_FASE_UM/POS_TECH_FASE_UM.csproj" -c Release -o /app/publish /p:Version=1.0.53

# Etapa 2: Criar a imagem final de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar os artefatos publicados da etapa de build
COPY --from=build /app/publish .

# Expondo as portas que você definiu
EXPOSE 5000

# Definir o comando de entrada para a aplicação principal
ENTRYPOINT ["dotnet", "POS_TECH_FASE_UM.dll"]