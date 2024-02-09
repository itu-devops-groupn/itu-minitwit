FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /src

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet build -o /app
RUN dotnet publish -o /publish
WORKDIR /publish
ENV ASPNETCORE_URL=http://+:80/
EXPOSE 80
CMD ["./itu-minitwit"]
