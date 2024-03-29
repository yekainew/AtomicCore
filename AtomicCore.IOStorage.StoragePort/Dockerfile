﻿# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore "AtomicCore.IOStorage.StoragePort.csproj"

# Copy everything else and build
COPY . ./
RUN dotnet publish "AtomicCore.IOStorage.StoragePort.csproj" -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
ENV IOSTORAGE_APPTOKEN=a6e2f27ee1f544cc889898e4397f7b07
ENV IOSTORAGE_SAVEROOTDIR=uploads
ENV IOSTORAGE_ALLOWFILEEXTS=.jpg,.jpeg,.png,.gif,.xls,.xlsx,.doc,.docx,.ppt,.pptx
ENV IOSTORAGE_ALLOWFILEMBSIZELIMIT=50
ENV Logging__Console__FormatterName=simple
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "AtomicCore.IOStorage.StoragePort.dll"]