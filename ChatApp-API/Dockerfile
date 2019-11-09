#------------------------------------- SDK -----------------------------------------#

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 as build-net

WORKDIR /chatAppAPI

RUN dotnet --version

COPY *.csproj /chatAppAPI

RUN dotnet restore

COPY . /chatAppAPI

RUN dotnet publish ChatApp.sln -o publish

RUN dotnet ef database update -v

#------------------------------------ RUNTIME -------------------------------------#

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2

WORKDIR /chatAppAPI

COPY --from=build-net /chatAppAPI/publish/ .
#ENV ConnectionStrings:DefaultConnection "testeabaci" #Funciona
ENTRYPOINT [ "dotnet", "DatingApp.API.dll" ]

