FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/BankApi.Api/BankApi.Api.csproj", "BankApi.Api/"]
COPY ["src/BankApi.Application/BankApi.Application.csproj", "BankApi.Application/"]
COPY ["src/BankApi.Domain/BankApi.Domain.csproj", "BankApi.Domain/"]
COPY ["src/BankApi.Infrastructure/BankApi.Infrastructure.csproj", "BankApi.Infrastructure/"]

RUN dotnet restore "BankApi.Api/BankApi.Api.csproj"

COPY src/ .

WORKDIR "/src/BankApi.Api"
RUN dotnet build "BankApi.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BankApi.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BankApi.Api.dll"]
