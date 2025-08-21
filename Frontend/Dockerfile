# build stage
FROM node:18-alpine as build-stage
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build

# production stage
FROM nginx:1.25-alpine as production-stage
COPY --from=build-stage /app/out /usr/share/nginx/html
EXPOSE 4040
CMD ["nginx", "-g", "daemon off;"]
