# # Build
# FROM node:22 AS build
# WORKDIR /app
# COPY package*.json ./
# RUN npm install
# COPY . .
# RUN npm run build

# # Serve
# # FROM nginx:alpine
# # COPY --from=build /app/dist /usr/share/nginx/html
# # EXPOSE 80
# # CMD [ "nginx", "-g", "daemon off;"]

FROM node:22
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
CMD ["npm", "run", "dev"]

# FROM node:22 AS final
# WORKDIR /app
# COPY --from=build /app .
# RUN npm run dev