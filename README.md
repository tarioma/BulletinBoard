# Доска объявлений

Строка подключения к базе данных PostgreSQL в файлах
`src/Infrastructure/BulletinBoard.WebAPI/appsettings.json` и
`src/Infrastructure/BulletinBoard.WebAPI/appsettings.Development.json`

Применение миграций:

```
cd .\src\Infrastructure\BulletinBoard.Infrastructure
```

```
dotnet ef --startup-project ../BulletinBoard.WebAPI/ database update
```
