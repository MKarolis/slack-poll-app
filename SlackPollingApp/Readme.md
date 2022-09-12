# Slack polling app built with .NET Core and Blazor

---
Built by Karolis Medekša for .NET programming course.

Vilnius University, Faculty of Mathematics and Informatics, 2022

---

## Local setup steps
- Make sure that an instance of MongoDb is running locally on port 27017
- Create an app on Slack Workspace
- set the bot secret locally:
```shell
cd SlackPollingApp
dotnet user-secrets set "SlackConfig:BotToken" "xoxb-XXXXXXXXXXXXXXXXXXXXXXXXXX"
```