FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RentalCars/RentalCars.csproj", "RentalCars/"]
RUN dotnet restore "RentalCars/RentalCars.csproj"
COPY . .
WORKDIR "/src/RentalCars"
RUN dotnet build "RentalCars.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RentalCars.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Install EF Core CLI
RUN dotnet tool install --global dotnet-ef --version 7.0.0
RUN dotnet add package Microsoft.EntityFrameworkCore.Design --version 7.0.0
ENV PATH="${PATH}:/root/.dotnet/tools"

# Run EF Core migrations
RUN dotnet ef migrations add DockerCreate --project RentalCars.csproj
RUN dotnet ef database update --project RentalCars.csproj

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RentalCars.dll"]