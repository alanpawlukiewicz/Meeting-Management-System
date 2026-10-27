# ---------- build ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY MeetingManager.Core/MeetingManager.Core.csproj MeetingManager.Core/
COPY MeetingManager.Domain/MeetingManager.Domain.csproj MeetingManager.Domain/
COPY MeetingManager.Infrastructure/MeetingManager.Infrastructure.csproj MeetingManager.Infrastructure/
COPY MeetingManager.WebApi/MeetingManager.WebApi.csproj MeetingManager.WebApi/
RUN dotnet restore MeetingManager.WebApi/MeetingManager.WebApi.csproj

COPY . .
RUN dotnet publish MeetingManager.WebApi/MeetingManager.WebApi.csproj \
    -c Release -o /app/publish --no-restore /p:UseAppHost=false

# ---------- runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

USER app
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "MeetingManager.WebApi.dll"]
