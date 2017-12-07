FROM microsoft/dotnet:2.0-sdk AS build-env
WORKDIR /app

# copy csproj and restore as distinct layers
COPY RVBot/*.csproj ./RVBot/
#COPY warframe-net-master/*.csproj ./warframe-net-master/
#RUN dotnet restore warframe-net-master/WarframeNET.csproj
RUN dotnet restore /p:TargetFramework=netcoreapp2.0 RVBot/RVBot.csproj

# copy everything else and build
COPY . ./
RUN dotnet publish RVBot/RVBot.csproj -c Release -o /app/out /p:TargetFramework=netcoreapp2.0

# build runtime image
FROM microsoft/dotnet:2.0-runtime
WORKDIR /app
COPY --from=build-env /app/out ./
ENTRYPOINT ["dotnet", "RVBot.dll"]