FROM node:20.2.0-alpine3.17
RUN mkdir /app
WORKDIR /app
COPY . .
RUN npm install
RUN npm run build
EXPOSE 3000
CMD ["npm", "start"]