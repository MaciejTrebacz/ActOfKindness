![READMElogo](https://github.com/MarcinSzkurlat/ActOfKindness/assets/94744112/14ad27d1-261b-473c-a5eb-7ceeb27ed545)

![Workflow](https://github.com/MarcinSzkurlat/ActOfKindness/actions/workflows/ci-backend.yml/badge.svg)

# Introduction
This application is the final project completed within Codecool Academy. Our task was to create a web application from scratch during six weekly sprints. At the end of each sprint, we prepared a presentation and presented our application to the entire group.
# About project
Act of Kindness web app connects users on a meaningful level (not like any other social media). We aim to link helpers with people in need and enable real-life interactions between them.

## Web app features
- Users can perform CRUD operations on Events
- Web app defines user roles - user, moderator, administrator
- Each event must be moderated before it reaches public
- Participants can use live chat for each event
- Web app implements logging in/registering users
- Each user can edit dedicated user profile page
- Integrated databases (for users and events)
- Easy picture upload

## Used technologies

| Backend | Frontend | Version control | Tools | IDE |
| ------------- | ------------- | ------------- | ------------- | ------------- |
| ![C#](https://img.shields.io/badge/C%20Sharp-239120.svg?style=for-the-badge&logo=C-Sharp&logoColor=white) | ![React](https://img.shields.io/badge/React-61DAFB.svg?style=for-the-badge&logo=React&logoColor=black) | ![Git](https://img.shields.io/badge/Git-F05032.svg?style=for-the-badge&logo=Git&logoColor=white) | ![Azure](https://img.shields.io/badge/Azure%20DevOps-0078D7.svg?style=for-the-badge&logo=Azure-DevOps&logoColor=white) | ![VS](https://img.shields.io/badge/Visual%20Studio-5C2D91.svg?style=for-the-badge&logo=Visual-Studio&logoColor=white) |
| ![.Net](https://img.shields.io/badge/.NET-512BD4.svg?style=for-the-badge&logo=dotnet&logoColor=white) | ![TypeScript](https://img.shields.io/badge/TypeScript-3178C6.svg?style=for-the-badge&logo=TypeScript&logoColor=white) | ![GitHub](https://img.shields.io/badge/GitHub-181717.svg?style=for-the-badge&logo=GitHub&logoColor=white) | ![Postman](https://img.shields.io/badge/Postman-FF6C37.svg?style=for-the-badge&logo=Postman&logoColor=white) | ![VSCode](https://img.shields.io/badge/Visual%20Studio%20Code-007ACC.svg?style=for-the-badge&logo=Visual-Studio-Code&logoColor=white) |
| ![MSSQL](https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927.svg?style=for-the-badge&logo=Microsoft-SQL-Server&logoColor=white) | ![MOBX](https://img.shields.io/badge/MobX-FF9955.svg?style=for-the-badge&logo=MobX&logoColor=white) |    | ![Figma](https://img.shields.io/badge/Figma-F24E1E.svg?style=for-the-badge&logo=Figma&logoColor=white) | 
| ![JWT](https://img.shields.io/badge/JSON%20Web%20Tokens-000000.svg?style=for-the-badge&logo=JSON-Web-Tokens&logoColor=white) | ![AXIOS](https://img.shields.io/badge/Axios-5A29E4.svg?style=for-the-badge&logo=Axios&logoColor=white) |
|   | ![SemanticUIReact](https://img.shields.io/badge/Semantic%20UI%20React-35BDB2.svg?style=for-the-badge&logo=Semantic-UI-React&logoColor=white) |

## Team Cooperation

- **Scrum Master Rotation**: Each sprint featured a different Scrum Master
- **Scrum Master Responsibilities**: The Scrum Master supervised PR reviews and approvals, as well as sprint backlog reviews and updates
- **Communication**: Daily update meetings and weekly retrospective meetings ensured effective team communication and continuous improvement

## Task sprints

| Sprint 1 | Sprint 2 | Sprint 3 | Sprint 4 | Sprint 5 | Sprint 6 |
| ------------- | ------------- | ------------- | ------------- | ------------- | ------------- |
| User stories | Domain models | Basic UI layout | Identity | Unit tests | Pipeline |
| Figma | First API endpoints with DTO | Seeder | Validators | Image upload | Docker compose |
| Design architecture | Entity Framework | Serilog | UI improvement | SignalR |
| Database structure |   |   | JWT |

## Screenshots

![AoK_Home_page](https://github.com/MarcinSzkurlat/ActOfKindness/assets/94744112/09ac9217-cf88-4837-8258-e6ff2a31599a)

![AoK_Login_to_create_event](https://github.com/MarcinSzkurlat/ActOfKindness/assets/94744112/467f6296-c453-4565-be97-6ae82940ca33)

![AoK_Events](https://github.com/MarcinSzkurlat/ActOfKindness/assets/94744112/ccc27234-3ed8-49c8-9c80-d1ab22d0ab49)

![AoK_Event_details](https://github.com/MarcinSzkurlat/ActOfKindness/assets/94744112/7c90aa05-9862-4ef4-afe8-3049cc59fe3f)

# Getting Started

Clone this repository.
```
git clone https://github.com/MaciejTrebacz/ActOfKindness.git
```

Make sure you have installed Docker on your computer. After that, you can run the below command from the `/ActOfKindness/` directory and get started with the `Act Of Kindness` immediately.
```gitbash
docker compose up
```

You should be able to browse the application by using the below URL:
```
http://localhost:3000
```

You can test the application as `admin`.

Login:
```
admin@test.com
```
Password:
```
ActOfKindness2023,
```
