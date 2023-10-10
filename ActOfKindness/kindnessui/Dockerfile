FROM node:20-alpine AS development 

WORKDIR /app

COPY kindnessui/package.json .
COPY kindnessui/package-lock.json .

RUN npm ci

COPY kindnessui/. .

CMD ["npm", "start"]