FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine3.14 as build
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet publish -o /app/published-app
RUN apk add --no-cache icu-libs krb5-libs libgcc libintl libssl1.1 libstdc++ zlib
EXPOSE 80

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine3.14 as runtime
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
WORKDIR /app
COPY --from=build /app/published-app /app
ENTRYPOINT [ "dotnet", "PosNews.dll" ]

