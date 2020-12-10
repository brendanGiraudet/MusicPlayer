#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["./MusicPlayerApplication.csproj", "MusicPlayerApplication/"]
WORKDIR "/src/MusicPlayerApplication"
RUN dotnet restore "./MusicPlayerApplication.csproj"
COPY . .
RUN dotnet build "MusicPlayerApplication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MusicPlayerApplication.csproj" -c Release -o /app/publish

FROM base AS final
ENV ASPNETCORE_URLS=http://+:1515
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "MusicPlayerApplication.dll"]