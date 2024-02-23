FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /src

COPY /src/ ./

EXPOSE 8080

RUN dotnet publish Chirp.Razor -c Release -o /Release
ENTRYPOINT /Release/Chirp.Razor