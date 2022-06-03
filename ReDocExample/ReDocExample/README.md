## 开启swagger注释
- 项目设置勾选“文档文件”
![](https://img2022.cnblogs.com/blog/599607/202206/599607-20220603200447450-930791831.png)

- 加载文档文件
```
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "天气查询说明", Version = "v2" });
    //启用中文注释功能
     var basePath = AppContext.BaseDirectory;
    var xmlPath = Path.Combine(basePath, "ReDocExample.xml");
    c.IncludeXmlComments(xmlPath, true);
});
```

## 添加接口注释及ProducesResponseType
- 添加Remarks注释，可以更详细描述文档内容
- ProducesResponseType可以让调用者更清楚接口不同状态下返回的接口数据格式
![](https://img2022.cnblogs.com/blog/599607/202206/599607-20220603200955066-23629068.png)

## swagger.json转swagger.yaml
- https://oktools.net/json2yaml
使用以上工具将swagger.json转为openapi.yaml文件
## 将部署Redoc 文档服务

1. Github上clone Redoc项目

Redoc地址： https://github.com/Redocly/redoc

2. 将 openapi.yaml 复制到demo目录中

![](https://img2022.cnblogs.com/blog/599607/202206/599607-20220603202743705-1787887003.png)

3. 运行npm run start 
![](https://img2022.cnblogs.com/blog/599607/202206/599607-20220603202710728-1963726427.png)

4. Redoc效果
- 完美的文档样式
![](https://img2022.cnblogs.com/blog/599607/202206/599607-20220603202843014-11042655.png)

