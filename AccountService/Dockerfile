FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_ENVIRONMENT=Development
ENV Logging__Console__FormatterName=simple
ENV USE_IN_MEMORY_DATABASE=false

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AccountServiceAPI/AccountServiceAPI.csproj", "AccountServiceAPI/"]
COPY ["AccountServiceDAL/AccountServiceDAL.csproj", "AccountServiceDAL/"]
COPY ["AccountServiceLogic/AccountServiceLogic.csproj", "AccountServiceLogic/"]
COPY ["AccountServiceModels/AccountServiceModels.csproj", "AccountServiceModels/"]
COPY ["AccountServiceMessageBus/AccountServiceMessageBus.csproj", "AccountServiceMessageBus/"]
RUN dotnet restore "AccountServiceAPI/AccountServiceAPI.csproj"
COPY . .
WORKDIR "/src/AccountServiceAPI"
RUN dotnet build "AccountServiceAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AccountServiceAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AccountServiceAPI.dll"]
