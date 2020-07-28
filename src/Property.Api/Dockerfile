FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY src/Property.Api/Property.Api.csproj src/Property.Api/
RUN dotnet restore "src/Property.Api/Property.Api.csproj"
COPY . .
WORKDIR "/src/src/Property.Api"
RUN dotnet build "Property.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Property.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Property.Api.dll"]
