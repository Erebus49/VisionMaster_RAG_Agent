# ChatDemoCs - VisionMaster + DeepSeek AI Assistant Demo

`ChatDemoCs` 是一个基于 .NET Framework 4.8 的 Windows Forms 示例程序。它仿照
`DeepLearningDemoCs` 的 VisionMaster 接入方式，保留方案加载、方案保存、流程选择、
单次运行、连续运行、图像显示和参数配置能力，并在界面右下方嵌入 DeepSeek /
OpenAI-compatible 聊天助手。

## 1. 功能概览

- **VisionMaster 方案接入**：支持选择并加载 `.sol` 方案文件。
- **流程控制**：支持流程下拉选择、运行一次、连续运行/停止连续运行。
- **渲染与配置**：内置 `VmRenderControl`、`VmGlobalToolControl`、`VmMainViewConfigControl`。
- **日志与结果**：右侧保留运行结果和日志列表，日志按日期写入 `Log` 目录。
- **结果统计**：连续运行结束后，可在 `结果统计` 弹窗中查看每类标签的样本数、占比直方图，并一键调用 DeepSeek 对分布情况做分析与改进建议。
- **AI 助手**：聊天面板固定在右下角，用于在调试视觉方案时咨询代码、SDK、算法或错误信息。
- **聊天设置**：API Key、Base URL、模型、温度、最大 Tokens、超时时间和系统提示词通过模态设置窗口配置。
- **国际化资源**：主界面提供 `zh-cn` / `en-us` 资源文件。

## 2. 项目结构

```text
ChatDemoCs/
├── ChatDemoCs.csproj
├── App.config
├── Program.cs
├── MainForm.cs / MainForm.Designer.cs / MainForm.resx
├── MainForm.zh-cn.resx / MainForm.en-us.resx
├── RenderControl.cs / RenderControl.Designer.cs / RenderControl.resx
├── MainViewControl.cs / MainViewControl.Designer.cs / MainViewControl.resx
├── ChatPanel.cs / ChatPanel.Designer.cs / ChatPanel.resx
├── SettingsForm.cs / SettingsForm.Designer.cs / SettingsForm.resx
├── ResultStatistics.cs
├── ResultStatisticsForm.cs / ResultStatisticsForm.Designer.cs / ResultStatisticsForm.resx
├── DeepSeekClient.cs
├── ChatMessage.cs
├── AppSettings.cs
└── Properties/
    ├── AssemblyInfo.cs
    ├── Resources.resx / Resources.Designer.cs
    └── Settings.settings / Settings.Designer.cs
```

## 3. 编译方式

### Visual Studio

1. 打开 `d:\ApplicationDemo\ApplicationDemo.sln`。
2. 选择 `ChatDemoCs` 项目。
3. 右键设为启动项目。
4. 执行 `生成解决方案` 或按 `Ctrl+Shift+B`。
5. 按 `F5` 运行。

### MSBuild

```powershell
& 'C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe' `
    'd:\ApplicationDemo\ChatDemoCs\ChatDemoCs.csproj' `
    /t:Rebuild `
    /p:Configuration=Release
```

当前已验证：

- **Debug 编译通过**
- **Release 编译通过**

输出路径：

```text
ChatDemoCs\bin\Debug\ChatDemoCs.exe
ChatDemoCs\bin\Release\ChatDemoCs.exe
```

## 4. VisionMaster 使用流程

1. 启动 `ChatDemoCs.exe`。
2. 在右侧 `Solution` 区域点击 `Select`，选择 VisionMaster `.sol` 方案。
3. 点击 `Load` 加载方案。
4. 加载成功后，流程列表会自动填充。
5. 在流程下拉框中选择目标流程。
6. 点击 `Run Once` 执行一次流程。
7. 点击 `Run Continuous` 开始连续运行，再次点击可停止。
8. 点击 `Render` 显示图像渲染视图。
9. 点击 `Config` 切换到 VisionMaster 参数配置视图。
10. 点击 `Save` 保存当前方案。
11. 连续运行停止后，`结果统计` 按钮变为可用，点击查看本次运行的标签分布；在弹窗右下角点击 `AI 分析` 让 DeepSeek 给出分布解读。

> 标签来源：优先读取流程级输出 `out`（字符串型），否则按 `label / class / name / result / ocr / text` 关键字匹配字符串输出，最后回退到第一个字符串型输出。

## 5. AI 助手配置

1. 在右下角聊天区域点击 `Settings`。
2. 填写以下字段：
   - **API Key**：DeepSeek 或兼容服务的密钥。
   - **Base URL**：DeepSeek 官方地址默认是 `https://api.deepseek.com`。
   - **Model**：例如 `deepseek-chat`、`deepseek-reasoner`。
   - **Temperature**：采样温度。
   - **Max Tokens**：单次回复最大 token 数。
   - **Timeout**：HTTP 请求超时时间。
   - **System Prompt**：助手角色提示词。
3. 点击 `Save` 保存配置。

配置会写入程序运行目录旁的 `ChatDemoCs.exe.config`。

## 6. 聊天使用方式

- **发送消息**：在输入框输入问题，点击 `Send`。
- **快捷发送**：按 `Ctrl + Enter`。
- **停止回复**：点击 `Stop` 取消当前流式请求。
- **清空上下文**：点击 `Clear`。
- **导出记录**：点击 `Export` 保存聊天记录。
- **模型切换**：可在聊天面板中切换模型并持久化。

聊天请求使用 `/v1/chat/completions` 接口，支持 Server-Sent Events 流式输出。

## 7. 日志说明

程序会同时维护两类日志：

- **界面日志**：显示在右侧 `Log` 区域，最新日志在最上方。
- **文件日志**：写入运行目录下的 `Log` 文件夹，按日期生成。

示例：

```text
ChatDemoCs\bin\Release\Log\2026-05-15.log
```

## 8. 常见问题

| 问题 | 处理方式 |
|---|---|
| 无法加载 `.sol` | 确认 VisionMaster SDK 已安装，并且方案路径存在。 |
| 找不到 VM SDK 程序集 | 确认本机安装路径包含 `VisionMaster_EDU4.3.0`，并与 `.csproj` 引用路径一致。 |
| AI 返回 `API Key is empty` | 打开 `Settings` 填写 API Key。 |
| HTTP 401 | 检查 API Key 是否正确。 |
| HTTP 404 | 检查 Base URL，DeepSeek 默认使用 `https://api.deepseek.com`。 |
| 请求超时 | 增大 `Timeout`，或检查网络代理。 |
| 中文显示异常 | 确认系统字体支持中文，必要时调整 `ChatPanel.Designer.cs` 中的字体。 |

## 9. 注意事项

- VisionMaster 方案运行依赖本机 SDK、授权和相关运行时环境。
- 聊天功能需要网络访问 DeepSeek 或兼容 API 服务。
- 如果程序从受保护目录运行，配置保存可能失败，建议复制到用户可写目录运行。
