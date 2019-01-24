FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["DockerNetCoreApi/DockerNetCoreApi.csproj", "DockerNetCoreApi/"]
RUN dotnet restore "DockerNetCoreApi/DockerNetCoreApi.csproj"
COPY . .
WORKDIR "/src/DockerNetCoreApi"
RUN dotnet build "DockerNetCoreApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DockerNetCoreApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DockerNetCoreApi.dll"]