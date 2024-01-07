#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AppSemTemplate.csproj", "AppSemTemplate/"]
RUN dotnet restore "AppSemTemplate/AppSemTemplate.csproj"
WORKDIR "AppSemTemplate"
COPY . .
RUN dotnet build "AppSemTemplate.csproj" -c Release -o /app/build
RUN dotnet dev-certs https -ep /root/.aspnet/https/aspnetapp.pfx -p Teste@123
RUN dotnet dev-certs https --trust

FROM build AS publish
COPY --from=build /root/.aspnet/https/aspnetapp.pfx /root/.aspnet/https/
RUN dotnet publish "AppSemTemplate.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish /root/.aspnet/https/aspnetapp.pfx /root/.aspnet/https/
VOLUME /var/data_protection_keys
ENV ASPNETCORE_ENVIRONMENT="Docker"
ENV ASPNETCORE_URLS="https://+:80;https://+:443;"
ENV ASPNETCORE_Kestrel__Certificates__Default__Password="Teste@123"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/root/.aspnet/https/aspnetapp.pfx
ENTRYPOINT ["dotnet", "AppSemTemplate.dll"] 