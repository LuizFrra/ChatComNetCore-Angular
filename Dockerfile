# Instala o node
FROM node:12.11.1 as angularApp
# Instala o Angular
RUN npm install -g @angular/cli@8.3.8
# Cria um Diretorio chamada chatSpa e já da um cd nele
WORKDIR /chatSpa
# Copia o package.json do host para a pasta criada no passo anterior
COPY ChatApp-SPA/package.json /chatSpa/package.json
# Mostra os Arquivos No Diretorio Criado
RUN ls -al
# Instala todas as dependencias
RUN npm install
# Copia os restos dos arquivos
COPY /ChatApp-SPA /chatSpa
# Da Build do projetp
RUN ng build

# Instala o Nginx
FROM nginx as nginxbuild
# Copia o resultado do build para a pasta do nginx
COPY --from=angularApp /chatSpa/dist/ChatApp-SPA/ /usr/share/nginx/html/

# # Instala o sdk do net core
# FROM microsoft/dotnet:2.2-sdk as buildnetcore
# # Cria a pasta para a api
# WORKDIR /api
# # Copia o csprojet e restaura as dependencias
# COPY ChatApp-API/*.csproj /api
# RUN dotnet --version
# RUN dotnet restore
# # Copia os arquivos da aplicação e da o build
# COPY ChatApp-API/ /api
# RUN dotnet publish ChatApp.sln -c Release -o out
# WORKDIR /api/out
# RUN ls -l
# ENTRYPOINT ["dotnet", "DatingApp.API.dll"]


