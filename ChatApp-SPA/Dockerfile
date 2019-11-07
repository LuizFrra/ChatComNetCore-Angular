FROM node:12.11.1 as build

RUN npm install -g @angular/cli@8.3.8
WORKDIR /app
COPY package.json /app/package.json
RUN npm install

COPY . /app
RUN ls -al
RUN ng build

FROM nginx
COPY --from=build /app/dist/ChatApp-SPA/ /usr/share/nginx/html/