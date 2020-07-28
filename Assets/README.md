Prefab Generator

1. Close Current Scene and Open an Empty Scene.
2. Instantiate GameObjcet and add Children according to .json config.
3. Save prefab and reopen scene.

Prefab Config Check Tool

- prefab children structure.
- nested prefab reference.
- components.
- configs.

not ready:
- parser (need new data structure, and unity yaml deserializing).
- instantiate and check properties.

---

预制体生成器

1. 关闭当前场景，开启空场景
2. 实例化对象，根据.json配置添加子对象
3. 保存预制体，重开之前场景

预制体配置检查工具

- 预制体子对象结构（多层Transform）
- 嵌套预制体引用（PrefabUtility.GetCorrespondingObjectFromOriginalSource）
- 组件（节点是否存在对应组件）
- 配置（以上全部涉及的配置）

没准备好使用哪种：
- parser（需要新的数据结构，unity yaml反序列化工具）
- 实例化预制体并检查其属性值