## 手动在部署
- 修改`Blog.Admin.Application applicationsettings.json`配置
- 使用 `vs` 发布 `Blog.Admin.Web.Entry` 
- 拷贝 `Dockerfile1` 到 `publish` 目录下，`Dockerfile1` 文件用于手动构建，根据实际修改端口
- 上传 `publish` 目录文件到服务器
- 进入 `publish` 目录 `docker build -t blog.api .` 生成 `Docker` 镜像 
- 启动容器
    ``` bash
    # 适用于使用了对象云存储
    # 命令解释 docker run -d -p API端口:容器暴露的端口 --restart=always --name 容器名 镜像名或镜像Id 
    docker run -d -p 8001:8001 --restart=always  --name blog.api blog.api

    # 适用于将附件上传至站点根目录
    # 命令解释 docker run -d -p API端口:容器暴露的端口 --restart=always -v 宿主挂载附件目录:容器内附件目录 --name 容器名 镜像名或镜像Id 
    docker run -d -p 8001:8001 --restart=always -v /home/wwwroot:/app/wwwroot  --name blog.api blog.api
    ```
- 查看容器 `docker ps -a`， 验证 `curl 127.0.0.1:8001/index.htnl`
- 重新发布，每次发布前需要删除现有的容器和镜像

## 自动部署
在 `/home/code/` 目录下部署脚本 `blog.api.sh`，进入目录对脚本设置权限 `chmod 777 blog.api.sh`，执行 `blog.api.sh`

```bash
#!/bin/bash
# 判断文件夹是否存在
if [ -d "blog.admin" ];then
    echo "文件夹已经存在"
    cd blog.admin
    echo "更新代码..."
    git pull
else
    echo "拉取代码..."
    git clone  https://github.com/weichangk/Lilikk.Blog.git blog.admin
    cd blog.admin
fi

echo "拷贝生产配置"
cp -f ./blog.admin.config/applicationsettings.json ./src/backend/Blog.Admin.Application/

echo "获取当前容器是否存在"
containerps=$(docker ps -f name=blog.admin -q)
containerstop=$(docker ps -a -f name=blog.admin -q)
for alpha in "$containerps";do
    if [ "$alpha" == "" ];then
        echo "检查是否存在停止的容器"
        for alpha1 in "$containerstop";do
            if [ "$alpha1" == "" ];then
                echo "不存指定容器"
            else
                echo "存在停止了的 然后直接删除-----------开始------------------"
                docker rm $alpha1
                echo "存在停止了的 然后直接删除-----------完成------------------"
            fi
       done
    else
        echo "存在-停止运行 然后删除----------------------开始-----------------"
        docker stop $alpha
        docker rm $alpha
        echo "存在-停止运行 然后删除---------------------完成------------------"
    fi
done

echo "获取当前镜像是否存在-----------------------------------------------------------------"
dockerlist=$(docker images blog.admin:latest -q)
for alpha2 in "$dockerlist";do
    if [ "$alpha2" == "" ];then
        echo "不存在指定镜像-------------------------------------------------"
    else
        echo "存在当前指定的镜像 删除镜像--------------开始-----------------------------------"
        docker rmi $alpha2
        echo "存在当前指定的镜像 删除镜像--------------完成-----------------------------------"
    fi
done


cd src/backend/

echo "--------生成镜像----------"
docker build -t blog.admin .

# 以下命令根据自身情况执行一种即可
echo --------------挂载配置文件以及上传目录以及日志目录----------------
# 第一种：使用了对象云存储，如果MinIO或者腾讯、阿里对象云存储等
docker run -d -p 8066:8066 --restart=always --name easy.admin easy.admin

# 第二种：附件保存在站点目录下(将容器的附件目录挂载到宿主/home/wwwroot目录中，此目录需要自行创建)
#docker run -d -p 8066:8066 -v /home/wwwroot:/app/wwwroot --restart=always --name easy.admin easy.admin
```