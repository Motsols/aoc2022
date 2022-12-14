FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine as build
WORKDIR /app
RUN apk add clang build-base zlib-dev
COPY . .
RUN dotnet publish -c Release -r linux-musl-x64 -o .

FROM alpine:latest
WORKDIR /
RUN apk add libstdc++
COPY --from=build /app/Day02 .
COPY ./input.txt .
ENTRYPOINT ./Day02