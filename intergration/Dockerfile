FROM microsoft/dotnet:2.2-sdk
WORKDIR /intergration

COPY intergration.csproj .
RUN dotnet restore 

COPY . .

RUN dotnet test --verbosity normal --logger trx  intergration.csproj

ENTRYPOINT ["dotnet", "test"]
