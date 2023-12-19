FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

COPY ["Directory.Build.props", ""]
COPY ["src/Blog.Fe.Domain/Blog.Fe.Domain.csproj", "src/Blog.Fe.Domain/"]
COPY ["src/Blog.Fe.Infrastructure/Blog.Fe.Infrastructure.csproj", "src/Blog.Fe.Infrastructure/"]
COPY ["src/Blog.Fe.Presentation/Blog.Fe.Presentation.csproj", "src/Blog.Fe.Presentation/"]

RUN dotnet restore "src/Blog.Fe.Presentation/Blog.Fe.Presentation.csproj"
COPY . .

RUN dotnet build "src/Blog.Fe.Presentation/Blog.Fe.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/Blog.Fe.Presentation/Blog.Fe.Presentation.csproj" -c Release -o /app/publish --no-restore /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Blog.Fe.Presentation.dll"]
