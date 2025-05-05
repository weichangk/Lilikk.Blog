# 请参阅 https://aka.ms/customizecontainer 以了解如何自定义调试容器，以及 Visual Studio 如何使用此 Dockerfile 生成映像以更快地进行调试。

# 此阶段用于在快速模式(默认为调试配置)下从 VS 运行时
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

#修改apt-get源，加速apt下载
RUN sed -i s@/deb.debian.org/@/mirrors.163.com/@g /etc/apt/sources.list.d/debian.sources
#RUN cat /etc/apt/sources.list
#安装fontconfig
RUN apt-get clean
RUN apt-get update 
RUN apt-get install -y fontconfig

USER $APP_UID
WORKDIR /app
EXPOSE 8066
EXPOSE 443


# 此阶段用于生成服务项目
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Easy.Admin.Web.Entry/Easy.Admin.Web.Entry.csproj", "Easy.Admin.Web.Entry/"]
COPY ["Easy.Admin.Web.Core/Easy.Admin.Web.Core.csproj", "Easy.Admin.Web.Core/"]
COPY ["Easy.Admin.Application/Easy.Admin.Application.csproj", "Easy.Admin.Application/"]
COPY ["Easy.Admin.Core/Easy.Admin.Core.csproj", "Easy.Admin.Core/"]
RUN dotnet restore "./Easy.Admin.Web.Entry/Easy.Admin.Web.Entry.csproj"
COPY . .
WORKDIR "/src/Easy.Admin.Web.Entry"
RUN dotnet build "./Easy.Admin.Web.Entry.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 此阶段用于发布要复制到最终阶段的服务项目
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Easy.Admin.Web.Entry.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 此阶段在生产中使用，或在常规模式下从 VS 运行时使用(在不使用调试配置时为默认值)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# 设置时区
ENV TZ=Asia/Shanghai
ENV ASPNETCORE_URLS 'http://*:8066'

ENTRYPOINT ["dotnet", "Easy.Admin.Web.Entry.dll"]