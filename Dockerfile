FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Finshark.sln ./
COPY Finshark.Api/Finshark.Api.csproj Finshark.Api/
COPY Finshark.Application/Finshark.Application.csproj Finshark.Application/
COPY Finshark.Domain/Finshark.Domain.csproj Finshark.Domain/
COPY Finshark.Infrastructure/Finshark.Infrastructure.csproj Finshark.Infrastructure/

RUN dotnet restore Finshark.sln

COPY Finshark.Api/ Finshark.Api/
COPY Finshark.Application/ Finshark.Application/
COPY Finshark.Domain/ Finshark.Domain/
COPY Finshark.Infrastructure/ Finshark.Infrastructure/

RUN dotnet publish Finshark.Api/Finshark.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Finshark.Api.dll"]
