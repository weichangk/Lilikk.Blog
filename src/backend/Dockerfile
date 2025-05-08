FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

RUN sed -i s@/deb.debian.org/@/mirrors.163.com/@g /etc/apt/sources.list.d/debian.sources
RUN apt-get clean
RUN apt-get update 
RUN apt-get install -y fontconfig

WORKDIR /app
EXPOSE 8066
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src
COPY ["Blog.Admin.Web.Entry/Blog.Admin.Web.Entry.csproj", "Blog.Admin.Web.Entry/"]
COPY ["Blog.Admin.Web.Core/Blog.Admin.Web.Core.csproj", "Blog.Admin.Web.Core/"]
COPY ["Blog.Admin.Application/Blog.Admin.Application.csproj", "Blog.Admin.Application/"]
COPY ["Blog.Admin.Core/Blog.Admin.Core.csproj", "Blog.Admin.Core/"]
RUN dotnet restore "Blog.Admin.Web.Entry/Blog.Admin.Web.Entry.csproj"
COPY . .
WORKDIR "/src/Blog.Admin.Web.Entry"
RUN dotnet build "Blog.Admin.Web.Entry.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Blog.Admin.Web.Entry.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV TZ=Asia/Shanghai
ENV ASPNETCORE_URLS 'http://*:8066'

ENTRYPOINT ["dotnet", "Blog.Admin.Web.Entry.dll"]