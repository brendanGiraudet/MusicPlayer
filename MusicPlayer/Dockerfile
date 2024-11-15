FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 1515
RUN apt update && apt dist-upgrade -y
RUN apt install -y yt-dlp
RUN apt install -y ffmpeg

# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
WORKDIR "/src/MusicPlayerApplication"
RUN dotnet restore
RUN dotnet build -c Release -o /app/build

# Test
FROM build AS test
WORKDIR "/src/MusicPlayer.UnitTest"
RUN dotnet test -c Release -o /app/publish

# Publication
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "MusicPlayerApplication.dll"]