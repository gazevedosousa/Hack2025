#Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

#Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["API_Simulacao_Hack.csproj", "."]
RUN dotnet restore "./API_Simulacao_Hack.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./API_Simulacao_Hack.csproj" -c $BUILD_CONFIGURATION -o /app/build

#Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./API_Simulacao_Hack.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

#Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API_Simulacao_Hack.dll"]