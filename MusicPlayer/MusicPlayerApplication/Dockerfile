#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 1515
EXPOSE 1516
EXPOSE 80
EXPOSE 443
RUN apt update && apt dist-upgrade -y
RUN apt install -y python3-pip
RUN pip3 install --upgrade youtube-dl
RUN apt install -y ffmpeg

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
ENV ASPNETCORE_URLS=https://+:1515;http://+:1516
ENV ASPNETCORE_HTTPS_PORT=1515
ENV ASPNETCORE_Kestrel__Certificates__Default__Password="music_playerpass"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/music_player.pfx
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "MusicPlayerApplication.dll"]