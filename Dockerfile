FROM mcr.microsoft.com/dotnet/sdk:7.0.400-jammy-amd64 as build
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet publish -o /app/published-app
RUN apk add icu-dev
RUN apk add --no-cache icu-libs
RUN apk add --no-cache icu-data-full
EXPOSE 80

FROM mcr.microsoft.com/dotnet/aspnet:7.0.400-jammy-amd64 as runtime
WORKDIR /app
COPY --from=build /app/published-app /app
ENTRYPOINT [ "dotnet", "PosNews.dll" ]

