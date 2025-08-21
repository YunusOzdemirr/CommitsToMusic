FROM node:20-alpine as builder
WORKDIR /app
COPY package*.json ./
RUN npm install

ARG NEXT_PUBLIC_API_URL
ENV NEXT_PUBLIC_API_URL=https://api.commitstomusic.yunusozdemirr.com

COPY . .
COPY next.config.js ./
COPY tailwind.config.ts postcss.config.js ./
RUN npm run build
# Run the project
FROM node:20-alpine as runner
WORKDIR /app
COPY --from=builder /app ./
EXPOSE 3001
CMD ["npm", "run", "start"]