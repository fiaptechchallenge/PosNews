FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine as build
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet publish -o /app/published-app
RUN apk add icu-dev
RUN apk add --no-cache icu-libs
RUN apk add --no-cache icu-data-full
EXPOSE 80


FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine as runtime
WORKDIR /app
COPY --from=build /app/published-app /app
ENTRYPOINT [ "dotnet", "PosNews.dll" ]

ENV LC_ALL=en_US.UTF-8 \
        LANG=en_US.UTF-8
