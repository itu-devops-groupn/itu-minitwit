FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /src

COPY /src/itu-minitwit/ ./

EXPOSE 8080

RUN dotnet publish -c Release -o /Release
ENTRYPOINT /Release/itu-minitwit
x`