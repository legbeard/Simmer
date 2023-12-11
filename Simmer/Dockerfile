FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Simmer/Simmer.csproj", "Simmer/"]
RUN dotnet restore "Simmer/Simmer.csproj"
COPY . .
WORKDIR "/src/Simmer"
RUN dotnet build "Simmer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Simmer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Simmer.dll"]
