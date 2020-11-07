FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY Property.Api/Property.Api.csproj Property.Api/
COPY Property.Core/Property.Core.csproj Property.Core/
COPY Property.Infrastructure/Property.Infrastructure.csproj Property.Infrastructure/
RUN dotnet restore "Property.Api/Property.Api.csproj"
COPY . .
WORKDIR "/src/Property.Api"
RUN dotnet build "Property.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Property.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Property.Api.dll"]
