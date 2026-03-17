FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["cod.data/cod.data.csproj", "cod.data/"]
COPY ["cod.application/cod.application.csproj", "cod.application/"]
COPY ["cod.domain/cod.domain.csproj", "cod.domain/"]
COPY ["cod.infraestructure/cod.infraestructure.csproj", "cod.infraestructure/"]
COPY ["cod.persistence/cod.persistence.csproj", "cod.persistence/"]
RUN dotnet restore "cod.data/cod.data.csproj"
COPY . .
WORKDIR "/src/cod.data"
RUN dotnet build "cod.data.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "cod.data.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "cod.data.dll"]
