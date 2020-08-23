#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["WebAPI/WebAPI.csproj", "WebAPI/"]
COPY ["Kernel.EF/Kernel.EF.csproj", "Kernel.EF/"]
COPY ["Kernel.Core/Kernel.Core.csproj", "Kernel.Core/"]
COPY ["Kernel.Repository/Kernel.Repository.csproj", "Kernel.Repository/"]
COPY ["Kernel.IService/Kernel.IService.csproj", "Kernel.IService/"]
COPY ["Kernel.Model/Kernel.Model.csproj", "Kernel.Model/"]
COPY ["Kernel.Dapper/Kernel.Dapper.csproj", "Kernel.Dapper/"]
COPY ["Kernel.Buildin/Kernel.Buildin.csproj", "Kernel.Buildin/"]
COPY ["Kernel.MediatR/Kernel.MediatR.csproj", "Kernel.MediatR/"]
COPY ["Kernel.Service/Kernel.Service.csproj", "Kernel.Service/"]
RUN dotnet restore "WebAPI/WebAPI.csproj"
COPY . .
WORKDIR "/src/WebAPI"
RUN dotnet build "WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebAPI.dll"]