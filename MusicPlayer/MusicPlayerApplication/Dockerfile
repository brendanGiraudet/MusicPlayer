FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 1515
RUN apt update && apt dist-upgrade -y
RUN apt install -y python3-pip
RUN pip3 install --upgrade yt-dlp
RUN apt install -y ffmpeg

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["./MusicPlayerApplication.csproj", "MusicPlayerApplication/"]
WORKDIR "/src/MusicPlayerApplication"
RUN dotnet restore "./MusicPlayerApplication.csproj"
COPY . .
RUN dotnet build "MusicPlayerApplication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MusicPlayerApplication.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "MusicPlayerApplication.dll"]