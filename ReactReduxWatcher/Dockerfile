FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ReactReduxWatcher/ReactReduxWatcher.csproj", "ReactReduxWatcher/"]
RUN dotnet restore "ReactReduxWatcher/ReactReduxWatcher.csproj"
COPY . .
WORKDIR "/src/ReactReduxWatcher"
RUN dotnet build "ReactReduxWatcher.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ReactReduxWatcher.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReactReduxWatcher.dll"]
