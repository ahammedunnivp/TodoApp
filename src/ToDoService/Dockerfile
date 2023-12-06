FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src


RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

COPY ["src/ToDoService/Unni.ToDo.WebAPI/Unni.Todo.WebAPI.csproj", "Unni.ToDo.WebAPI/"]
COPY ["src/ToDoService/Unni.Todo.Application/Unni.Todo.Application.csproj", "Unni.Todo.Application/"]
COPY ["src/ToDoService/Unni.Todo.Domain/Unni.Todo.Domain.csproj", "Unni.Todo.Domain/"]
COPY ["src/ToDoService/Unni.ToDo.Infrastructure/Unni.Todo.Infrastructure.csproj", "Unni.ToDo.Infrastructure/"]
RUN dotnet restore "Unni.ToDo.WebAPI/Unni.Todo.WebAPI.csproj"
COPY . .
WORKDIR "/src/Unni.ToDo.WebAPI"

RUN dotnet build  -c $BUILD_CONFIGURATION

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Unni.Todo.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
RUN dotnet dev-certs https --trust

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unni.Todo.WebAPI.dll"]
