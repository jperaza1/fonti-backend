FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["cod.auth/cod.auth.csproj", "cod.auth/"]
COPY ["cod.application/cod.application.csproj", "cod.application/"]
COPY ["cod.domain/cod.domain.csproj", "cod.domain/"]
COPY ["cod.infraestructure/cod.infraestructure.csproj", "cod.infraestructure/"]
COPY ["cod.persistence/cod.persistence.csproj", "cod.persistence/"]
RUN dotnet restore "cod.auth/cod.auth.csproj"
COPY . .
WORKDIR "/src/cod.auth"
RUN dotnet build "cod.auth.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "cod.auth.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "cod.auth.dll"]
