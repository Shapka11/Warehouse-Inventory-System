FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY Warehouse-Inventory-System.slnx .
COPY Directory.Build.props .
COPY Directory.Packages.props .


COPY src/ src/
COPY tests/ tests/


RUN dotnet restore Warehouse-Inventory-System.slnx

COPY . .

WORKDIR /app/src/Warehouse-Inventory-System
RUN dotnet publish Warehouse-Inventory-System.csproj -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /out .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Warehouse-Inventory-System.dll"]
