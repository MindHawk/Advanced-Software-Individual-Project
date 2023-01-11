FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
#ENV ASPNETCORE_ENVIRONMENT=Development
ENV Logging__Console__FormatterName=simple
ENV HOSTED_ENVIRONMENT=docker

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PostServiceAPI/PostServiceAPI.csproj", "PostServiceAPI/"]
COPY ["PostServiceDAL/PostServiceDAL.csproj", "PostServiceDAL/"]
COPY ["PostServiceLogic/PostServiceLogic.csproj", "PostServiceLogic/"]
COPY ["PostServiceModels/PostServiceModels.csproj", "PostServiceModels/"]
COPY ["PostServiceMessageBusProducer/PostServiceMessageBusProducer.csproj", "PostServiceMessageBusProducer/"]
COPY ["PostServiceMessageBusConsumer/PostServiceMessageBusConsumer.csproj", "PostServiceMessageBusConsumer/"]
RUN dotnet restore "PostServiceAPI/PostServiceAPI.csproj"
COPY . .
WORKDIR "/src/PostServiceAPI"
RUN dotnet build "PostServiceAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PostServiceAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PostServiceAPI.dll"]
