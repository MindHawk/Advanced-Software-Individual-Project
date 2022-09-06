FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_ENVIRONMENT=Development
ENV Logging__Console__FormatterName=simple

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Microservice Prototype.csproj", "./"]
RUN dotnet restore "Microservice Prototype.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "Microservice Prototype.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Microservice Prototype.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Microservice Prototype.dll"]
