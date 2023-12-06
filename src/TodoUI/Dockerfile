FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Unni.ToDo.UI/Unni.ToDo.UI.csproj", "Unni.ToDo.UI/"]
RUN dotnet restore "Unni.ToDo.UI/Unni.ToDo.UI.csproj"
COPY . .
WORKDIR "/src/Unni.ToDo.UI"

RUN dotnet build  -c $BUILD_CONFIGURATION

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Unni.ToDo.UI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unni.ToDo.UI.dll"]
