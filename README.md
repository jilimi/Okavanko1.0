# Okavanko
>Okavanko 仅适用与Rhino6及以上版本
1. Okavanko 是一款根据作者多年幕墙施工经验总结得出的基于Rhino Grasshopper开发的幕墙下料插件；

2. 该插件会随着作者施工经验的增加而进行不断优化和更新； 

3. Okavanko目前包括以下功能模块，`BIM`，`下料`,`基础`，`预览`；

**下面对插件的各功能模块进行简要的介绍：**  

---
## BIM 模块

该模块实现了给Rhino中的几何体添加自定义的信息，并对其进行操作的一些功能组件（Component），该模块包含如下组件：

| 序号 | 名称     | 描述 |
| :--: | -------- | ---- |
|  1   | GetKeys | 获取几何体“键-值”对中的键     |
|2|GetValues|获取几何体“键-值”对中的值|
|3|GetValueByKey|根据“键”获取与之对应的“值”|
|4|GroupGeometryByKey|根据键对几何体成组|
|5|RemoveDataByKey|根据“键”，移除与之对应的“键-值”对|
|6|SetKeyValue|给几何体设置“键-值”对自定义数据|
|7|GetGeometryByValue|根据给定的“值”在目标几何体中选取符合要求的几何体|

根据本人的工程实践经历，**BIM模块**主要用在以下几个方面：

1. 复杂规则的排序；
2. 图形归类；
3. 工程项目现场施工管理；

---

## 基础模块

基础模块你可以把它看作是众多功能的一个“集合”。日后该模块内的功能组件积累到了一定的数目之后，将会对其进行重新细分归类。该模块下功能较多，下面仅阐述几个比较重要的模块；

| 序号 | 名称 | 描述 |应用场景|
| :--: | :--- | :--- | ---- |
|   1  |  ClusterPts|对给定输入点进行聚类，聚类方式可按'x','y','z','d'(距离)进行    | 工程测量数据的处理|
|2|CreateSolidByOffsetSrf|偏移面获取实体|具有厚度的面板创建|
|3|ExtendSurface|对非修剪面进行延伸|/|
|4|PointClosetCurve|选择距离指定点最近的N条曲线|/|
|5|FindClosetBrep|选择距离指定点最近的N个Brep|/|
|6|DispatchBranch|根据需求对DataTree进行分组|/|
|7|CrvsClosetPoint|找出两条线之间最短距离的最大值|/|

---
## 下料模块

下料模块的作用








