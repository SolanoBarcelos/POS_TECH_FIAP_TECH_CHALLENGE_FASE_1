# Etapa Base (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Etapa de Build (SDK)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia os arquivos de projeto e restaura as depend�ncias
COPY ["./POS_TECH_FASE_UM/POS_TECH_FASE_UM.csproj", "./POS_TECH_FASE_UM/"]
COPY ["./TESTES_POS_TECH_FASE_UM/TESTES_POS_TECH_FASE_UM.csproj", "./TESTES_POS_TECH_FASE_UM/"]

# Restaurar depend�ncias de todos os projetos na solu��o
COPY ./POS_TECH_FASE_UM.sln ./
RUN dotnet restore "POS_TECH_FASE_UM.sln"

# Copia o restante dos arquivos e compila a aplica��o
COPY . ./
RUN dotnet build "./POS_TECH_FASE_UM/POS_TECH_FASE_UM.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etapa de Publica��o
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./POS_TECH_FASE_UM/POS_TECH_FASE_UM.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa Final: Imagem de Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Configurar vari�vel de ambiente para string de conex�o de produ��o
ENV ConnectionStrings__DefaultConnection="Host=db_pos_fase_1;Port=5432;Pooling=true;Database=db_pos_fase_1;User Id=admin;Password=1234;"

# Copiar arquivos da publica��o para imagem final
COPY --from=publish /app/publish .

# Expor porta no WSL2 (certifique-se que est� configurada corretamente)
EXPOSE 7070

# Configurar entrada principal
ENTRYPOINT ["dotnet", "POS_TECH_FASE_UM.dll"]
