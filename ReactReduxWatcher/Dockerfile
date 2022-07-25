FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ReactReduxDocsNotifyer/ReactReduxDocsNotifyer.csproj", "ReactReduxDocsNotifyer/"]
RUN dotnet restore "ReactReduxDocsNotifyer/ReactReduxDocsNotifyer.csproj"
COPY . .
WORKDIR "/src/ReactReduxDocsNotifyer"
RUN dotnet build "ReactReduxDocsNotifyer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ReactReduxDocsNotifyer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReactReduxDocsNotifyer.dll"]
