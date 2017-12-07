FROM microsoft/dotnet:2.0-sdk AS build-env
WORKDIR /app

# copy csproj and restore as distinct layers
COPY RVBot/*.csproj ./RVBot/
COPY warframe-net-master/*.csproj ./warframe-net-master/
RUN   sed -i "s|<TargetFramework>net462</TargetFramework>|<TargetFramework>netcoreapp2.0</TargetFramework>|g" ./RVBot/RVBot.csproj
RUN dotnet restore ./warframe-net-master/WarframeNET.csproj
RUN dotnet restore ./RVBot/RVBot.csproj /p:TargetFramework=netcoreapp2.0

# copy everything else and build
COPY . ./
RUN dotnet build ./warframe-net-master/WarframeNET.csproj -c Release
RUN dotnet publish ./RVBot/RVBot.csproj -c Release -o /app/out /p:TargetFramework=netcoreapp2.0

# build runtime image
FROM microsoft/dotnet:2.0-runtime
WORKDIR /app
COPY --from=build-env /app/out ./
ENTRYPOINT ["dotnet", "RVBot.dll"]
