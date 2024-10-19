FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Infrastructure/BulletinBoard/BulletinBoard.csproj", "src/Infrastructure/BulletinBoard/"]
COPY ["src/Infrastructure/Dal/BulletinBoard.Dal/BulletinBoard.Dal.csproj", "src/Infrastructure/Dal/BulletinBoard.Dal/"]
COPY ["src/Application/BulletinBoard.Application/BulletinBoard.Application.csproj", "src/Application/BulletinBoard.Application/"]
COPY ["src/Domain/BulletinBoard.Domain/BulletinBoard.Domain.csproj", "src/Domain/BulletinBoard.Domain/"]
COPY ["src/Application/BulletinBoard.Application.Abstraction/BulletinBoard.Application.Abstraction.csproj", "src/Application/BulletinBoard.Application.Abstraction/"]
RUN dotnet restore "src/Infrastructure/BulletinBoard/BulletinBoard.csproj"
COPY . .
WORKDIR "/src/src/Infrastructure/BulletinBoard"
RUN dotnet build "BulletinBoard.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BulletinBoard.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BulletinBoard.dll"]
