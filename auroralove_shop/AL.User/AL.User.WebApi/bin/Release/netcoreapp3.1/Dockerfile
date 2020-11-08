#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["AL.User.WebApi/AL.User.WebApi.csproj", "AL.User.WebApi/"]
COPY ["AL.User.Services/AL.User.Services.csproj", "AL.User.Services/"]
COPY ["AL.User.IRepository/AL.User.IRepository.csproj", "AL.User.IRepository/"]
COPY ["AL.User.DTO.Models/AL.User.DTO.Models.csproj", "AL.User.DTO.Models/"]
COPY ["AL.User.DB.Entitys/AL.User.DB.Entitys.csproj", "AL.User.DB.Entitys/"]
COPY ["AL.User.IServices/AL.User.IServices.csproj", "AL.User.IServices/"]
COPY ["AL.User.Repository/AL.User.Repository.csproj", "AL.User.Repository/"]
RUN dotnet restore "AL.User.WebApi/AL.User.WebApi.csproj"
COPY . .
WORKDIR "/src/AL.User.WebApi"
RUN dotnet build "AL.User.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AL.User.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AL.User.WebApi.dll"]