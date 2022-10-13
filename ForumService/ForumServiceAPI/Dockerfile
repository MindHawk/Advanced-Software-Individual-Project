FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_ENVIRONMENT=Development
ENV Logging__Console__FormatterName=simple

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ForumServiceAPI/ForumServiceAPI.csproj", "ForumServiceAPI/"]
COPY ["ForumServiceDAL/ForumServiceDAL.csproj", "ForumServiceDAL/"]
COPY ["ForumServiceLogic/ForumServiceLogic.csproj", "ForumServiceLogic/"]
COPY ["ForumServiceModels/ForumServiceModels.csproj", "ForumServiceModels/"]
COPY ["ForumServiceMessageBus/ForumServiceMessageBus.csproj", "ForumServiceMessageBus/"]
RUN dotnet restore "ForumServiceAPI/ForumServiceAPI.csproj"
COPY . .
WORKDIR "/src/ForumServiceAPI"
RUN dotnet build "ForumServiceAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ForumServiceAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ForumServiceAPI.dll"]
