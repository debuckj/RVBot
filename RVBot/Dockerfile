FROM microsoft/dotnet:1.1-sdk AS build-env
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore /p:TargetFramework=netcoreapp1.1

# copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out /p:TargetFramework=netcoreapp1.1

# build runtime image
FROM microsoft/dotnet:1.1-runtime 
WORKDIR /app
COPY --from=build-env /app/out ./
ENTRYPOINT ["dotnet", "RVBot.dll"]

