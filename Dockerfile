FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine as build
WORKDIR /
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
EXPOSE 80
EXPOSE 443
EXPOSE 1433
COPY . .
RUN dotnet restore
RUN dotnet publish -o /app/published-app

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine as runtime
WORKDIR /app
COPY --from=build /app/published-app /app
ENTRYPOINT [ "dotnet", "PosNews.dll" ]

