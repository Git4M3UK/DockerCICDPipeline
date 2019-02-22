FROM microsoft/dotnet:2.2-sdk
WORKDIR /intergration

COPY intergration.csproj .
RUN dotnet restore 

COPY . .

ENV TEAMCITY_PROJECT_NAME=fake

RUN dotnet test --verbosity normal --logger trx  intergration.csproj

#ENTRYPOINT ["dotnet", "test"]
