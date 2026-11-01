\# Demo API



Сервис для управления проектами.



\## Запуск



1\. Создать файл .env.

2\. Запустить Docker:



docker compose up -d --build --wait



\## Разработка



docker compose up -d --wait db



После этого запустить проект через F5 в Visual Studio.



\## Тестирование



dotnet test



\## Основные эндпоинты



\- GET /api/projects

\- GET /api/projects/{id}

\- POST /api/projects

