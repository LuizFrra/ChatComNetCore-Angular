FROM node:12.11.1 as build

RUN npm install -g @angular/cli@8.3.8

WORKDIR /app

COPY package.json /app/package.json

RUN npm install -f

COPY . /app

RUN chmod +x ./env_setup.sh

ENV api='http://78.47.19.185'

ENV port=5000

RUN  ls -al

RUN ./env_setup.sh

RUN cat src/environments/environment.prod.ts

RUN ng build --configuration=production


#------------------------------------------------ nginx -----------------------------------------------#

FROM nginx

COPY --from=build /app/dist/ChatApp-SPA/ /usr/share/nginx/html/