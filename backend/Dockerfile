FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Directory.Build.props Directory.Packages.props global.json PawPaw.slnx ./
COPY src/PawPaw.Core/PawPaw.Core.csproj src/PawPaw.Core/
COPY src/PawPaw.UseCases/PawPaw.UseCases.csproj src/PawPaw.UseCases/
COPY src/PawPaw.Infrastructure/PawPaw.Infrastructure.csproj src/PawPaw.Infrastructure/
COPY src/PawPaw.Web/PawPaw.Web.csproj src/PawPaw.Web/
RUN dotnet restore src/PawPaw.Web/PawPaw.Web.csproj

COPY . .
RUN dotnet publish src/PawPaw.Web/PawPaw.Web.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "PawPaw.Web.dll"]
