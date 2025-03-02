FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /app
EXPOSE 80

COPY  Gateway/FreeCourse.Gateway/*.csproj  Gateway/FreeCourse.Gateway/
RUN dotnet restore Gateway/FreeCourse.Gateway/*.csproj
COPY . .
RUN dotnet publish Gateway/FreeCourse.Gateway/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","FreeCourse.Gateway.dll" ]