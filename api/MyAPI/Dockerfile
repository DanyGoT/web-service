# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["*.csproj", "MyAPI/"]
RUN dotnet restore "./MyAPI/MyAPI.csproj"
WORKDIR "/src/MyAPI"
COPY . .

# Run migrations
RUN dotnet tool install --global dotnet-ef
# RUN dotnet add "./MyAPI.csproj" package Microsoft.EntityFrameworkCore.Design # For migrations

# BUILD
RUN dotnet build "./MyAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build
# RUN ~/.dotnet/tools/dotnet-ef migrations bundle --project "./MyAPI.csproj" --output /app/efbundle # For migrations

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
# COPY --from=build /app/efbundle /app/efbundle # For migrations
RUN dotnet publish "./MyAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
ARG CONNECTION_STRING
WORKDIR /app
# COPY --from=publish /app/efbundle /app/efbundle # For migrations
COPY --from=publish /app/publish .
# CMD ["./efbundle", "--connection", "'$CONNECTION_STRING'"] # For migrations
ENTRYPOINT ["dotnet", "MyAPI.dll"]

#Dont know how to make bundling work. It works if I execute the last command inside the api-container...