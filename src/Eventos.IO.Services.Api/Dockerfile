FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 44391

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY src/Eventos.IO.Services.Api/Eventos.IO.Services.Api.csproj src/Eventos.IO.Services.Api/
COPY src/Eventos.IO.Infra.CrossCutting.AspNetFilters/Eventos.IO.Infra.CrossCutting.AspNetFilters.csproj src/Eventos.IO.Infra.CrossCutting.AspNetFilters/
COPY src/Eventos.IO.Domain.Core/Eventos.IO.Domain.Core.csproj src/Eventos.IO.Domain.Core/
COPY src/Eventos.IO.Infra.CrossCutting.IoC/Eventos.IO.Infra.CrossCutting.IoC.csproj src/Eventos.IO.Infra.CrossCutting.IoC/
COPY src/Eventos.IO.Infra.Data/Eventos.IO.Infra.Data.csproj src/Eventos.IO.Infra.Data/
COPY src/Eventos.IO.Domain/Eventos.IO.Domain.csproj src/Eventos.IO.Domain/
COPY src/Eventos.IO.Infra.CrossCutting.Identity/Eventos.IO.Infra.CrossCutting.Identity.csproj src/Eventos.IO.Infra.CrossCutting.Identity/
COPY src/Eventos.IO.Application/Eventos.IO.Application.csproj src/Eventos.IO.Application/
RUN dotnet restore src/Eventos.IO.Services.Api/Eventos.IO.Services.Api.csproj
COPY . .
WORKDIR /src/src/Eventos.IO.Services.Api
RUN dotnet build Eventos.IO.Services.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Eventos.IO.Services.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Eventos.IO.Services.Api.dll"]
