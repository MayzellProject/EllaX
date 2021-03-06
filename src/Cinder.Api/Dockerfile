FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/Cinder.Api/Cinder.Api.csproj", "src/Cinder.Api/"]
COPY ["src/Cinder.Messages/Cinder.Messages.csproj", "src/Cinder.Messages/"]
COPY ["src/Cinder.Data/Cinder.Data.csproj", "src/Cinder.Data/"]
COPY ["src/Cinder.Core/Cinder.Core.csproj", "src/Cinder.Core/"]
COPY ["src/Cinder.Documents/Cinder.Documents.csproj", "src/Cinder.Documents/"]
RUN dotnet restore "src/Cinder.Api/Cinder.Api.csproj"
COPY . .
WORKDIR "/src/src/Cinder.Api"
RUN dotnet build "Cinder.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Cinder.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Cinder.Api.dll"]