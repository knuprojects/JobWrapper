#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Gateway/JobWrapper.Gateway/JobWrapper.Gateway.csproj", "src/Gateway/JobWrapper.Gateway/"]
COPY ["src/SharedKernel/Shared.Contexts/Shared.Contexts.csproj", "src/SharedKernel/Shared.Contexts/"]
COPY ["src/SharedKernel/Shared/Shared.csproj", "src/SharedKernel/Shared/"]
COPY ["src/SharedKernel/Shared.Abstractions/Shared.Abstractions.csproj", "src/SharedKernel/Shared.Abstractions/"]
COPY ["src/SharedKernel/Shared.Api/Shared.Api.csproj", "src/SharedKernel/Shared.Api/"]
COPY ["src/SharedKernel/Shared.Dal/Shared.Dal.csproj", "src/SharedKernel/Shared.Dal/"]
COPY ["src/SharedKernel/Shared.Http/Shared.Http.csproj", "src/SharedKernel/Shared.Http/"]
COPY ["src/SharedKernel/Shared.Observability/Shared.Observability.csproj", "src/SharedKernel/Shared.Observability/"]
COPY ["src/SharedKernel/Shared.Security/Shared.Security.csproj", "src/SharedKernel/Shared.Security/"]
RUN dotnet restore "src/Gateway/JobWrapper.Gateway/JobWrapper.Gateway.csproj"
COPY . .
WORKDIR "/src/src/Gateway/JobWrapper.Gateway"
RUN dotnet build "JobWrapper.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JobWrapper.Gateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS http://*:80
ENV ASPNETCORE_ENVIRONMENT Production
ENTRYPOINT ["dotnet", "JobWrapper.Gateway.dll"]