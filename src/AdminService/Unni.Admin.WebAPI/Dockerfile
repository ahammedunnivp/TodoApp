#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

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

COPY ["src/AdminService/Unni.Admin.WebAPI/Unni.Admin.WebAPI.csproj", "src/AdminService/Unni.Admin.WebAPI/"]
COPY ["src/AdminService/Unni.Admin.Application/Unni.Admin.Application.csproj", "src/AdminService/Unni.Admin.Application/"]
COPY ["src/AdminService/Unni.Admin.Domain/Unni.Admin.Domain.csproj", "src/AdminService/Unni.Admin.Domain/"]
COPY ["src/AdminService/Unni.Admin.Infrastructure/Unni.Admin.Infrastructure.csproj", "src/AdminService/Unni.Admin.Infrastructure/"]
RUN dotnet restore "./src/AdminService/Unni.Admin.WebAPI/./Unni.Admin.WebAPI.csproj"
COPY . .

WORKDIR "/src/src/AdminService/Unni.Admin.WebAPI"
RUN dotnet build "./Unni.Admin.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet dev-certs https --trust

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Unni.Admin.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Unni.Admin.WebAPI.dll"]