FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Simmer.Benchmarks/Simmer.Benchmarks.csproj", "Simmer.Benchmarks/"]
RUN dotnet restore "Simmer.Benchmarks/Simmer.Benchmarks.csproj"
COPY . .
WORKDIR "/src/Simmer.Benchmarks"
RUN dotnet build "Simmer.Benchmarks.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Simmer.Benchmarks.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Simmer.Benchmarks.dll"]
