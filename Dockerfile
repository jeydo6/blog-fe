FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETARCH
ARG BUILD_CONFIGURATION=Release

WORKDIR /app

COPY ["Directory.Build.props", ""]
COPY ["src/Blog.Fe.Domain/Blog.Fe.Domain.csproj", "src/Blog.Fe.Domain/"]
COPY ["src/Blog.Fe.Infrastructure/Blog.Fe.Infrastructure.csproj", "src/Blog.Fe.Infrastructure/"]
COPY ["src/Blog.Fe.Presentation/Blog.Fe.Presentation.csproj", "src/Blog.Fe.Presentation/"]

RUN dotnet restore "src/Blog.Fe.Presentation/Blog.Fe.Presentation.csproj" -a $TARGETARCH --use-lock-file
COPY . .

RUN dotnet build "src/Blog.Fe.Presentation/Blog.Fe.Presentation.csproj" -c $BUILD_CONFIGURATION -a $TARGETARCH --no-restore -o build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "src/Blog.Fe.Presentation/Blog.Fe.Presentation.csproj" -c $BUILD_CONFIGURATION -a $TARGETARCH --no-restore -o publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Blog.Fe.Presentation.dll"]
