#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV ASPNETCORE_HTTP_PORTS=8080
ENV TZ="America/Sao_Paulo"
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["API_Simulacao_Hack/API_Simulacao_Hack.csproj", "API_Simulacao_Hack/"]
RUN dotnet restore "./API_Simulacao_Hack/./API_Simulacao_Hack.csproj"
COPY . .
WORKDIR "/src/API_Simulacao_Hack"
RUN dotnet build "./API_Simulacao_Hack.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./API_Simulacao_Hack.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API_Simulacao_Hack.dll"]