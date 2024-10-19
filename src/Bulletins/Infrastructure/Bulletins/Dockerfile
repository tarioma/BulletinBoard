FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Bulletins/Infrastructure/Bulletins/Bulletins.csproj", "src/Bulletins/Infrastructure/Bulletins/"]
RUN dotnet restore "src/Bulletins/Infrastructure/Bulletins/Bulletins.csproj"
COPY . .
WORKDIR "/src/src/Bulletins/Infrastructure/Bulletins"
RUN dotnet build "Bulletins.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Bulletins.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bulletins.dll"]
