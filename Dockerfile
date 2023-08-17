FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet publish /app/published-app -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
WORKDIR /app
COPY --from=build /app/published-app/out /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://*:80
ENTRYPOINT [ "dotnet", "PosNews.dll" ]
