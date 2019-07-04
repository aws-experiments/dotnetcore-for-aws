FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY *.sln .
COPY src/Dotnetcore.For.Aws.Domain/Dotnetcore.For.Aws.Domain.csproj ./src/Dotnetcore.For.Aws.Domain/
RUN dotnet restore src/Dotnetcore.For.Aws.Domain/Dotnetcore.For.Aws.Domain.csproj
COPY src/Dotnetcore.For.Aws.Service/Dotnetcore.For.Aws.Service.csproj ./src/Dotnetcore.For.Aws.Service/
RUN dotnet restore src/Dotnetcore.For.Aws.Service/Dotnetcore.For.Aws.Service.csproj

# copy everything else and build app
COPY . .

RUN dotnet publish src/Dotnetcore.For.Aws.Service/Dotnetcore.For.Aws.Service.csproj -o /app


FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
COPY --from=build /app /app
WORKDIR /app
EXPOSE 50366
ENV ASPNETCORE_URLS http://+:50366
ENTRYPOINT ["dotnet", "Dotnetcore.For.Aws.Service.dll"]