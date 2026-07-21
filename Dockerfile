FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/Locerp.Web/Locerp.Web.csproj", "src/Locerp.Web/"]
RUN dotnet restore "src/Locerp.Web/Locerp.Web.csproj"
COPY . .
WORKDIR "/src/src/Locerp.Web"
RUN dotnet publish "Locerp.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Locerp.Web.dll"]
