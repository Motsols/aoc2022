FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Install NativeAOT build prerequisites
RUN apt-get update && apt-get install -y --no-install-recommends \
       clang \
       zlib1g-dev

WORKDIR /source
COPY Day03.csproj .
COPY input.txt .
COPY Program.cs .
RUN dotnet publish -c Release -o publish --use-current-runtime -p:"PublishAot=true;StripSymbols=true;IlcOptimizationPreference=Speed"

FROM mcr.microsoft.com/dotnet/runtime-deps:7.0
WORKDIR /app
COPY --from=build /source/publish .
ENTRYPOINT [ "./Day03" ]