# Etapa Base (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Etapa de Build (SDK)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar os arquivos de projeto e restaurar dependências
COPY ["./POS_TECH_FASE_UM/POS_TECH_FASE_UM.csproj", "./POS_TECH_FASE_UM/"]
COPY ["./TESTES_POS_TECH_FASE_UM/TESTES_POS_TECH_FASE_UM.csproj", "./TESTES_POS_TECH_FASE_UM/"]

# Restaurar dependências
COPY ./POS_TECH_FASE_UM.sln ./  
RUN dotnet restore "POS_TECH_FASE_UM.sln"

# Copiar arquivos e compilar
COPY . ./
RUN dotnet build "./POS_TECH_FASE_UM/POS_TECH_FASE_UM.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etapa de Publicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./POS_TECH_FASE_UM/POS_TECH_FASE_UM.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa Final: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Configuração da conexão com o PostgreSQL na rede "mynw"
ENV ConnectionStrings__DefaultConnection="Host=postgres;Port=5432;Pooling=true;Database=db_pos_fase_1;User Id=admin;Password=1234;"

# Copiar arquivos publicados
COPY --from=publish /app/publish .

# Expor a porta configurada
EXPOSE 7070

# Configurar entrada principal
ENTRYPOINT ["dotnet", "POS_TECH_FASE_UM.dll"]
