# Инструкция по настройке проекта

## Изменения в проекте

Проект был обновлен для использования:
1. **PostgreSQL** вместо SQL Server в качестве СУБД
2. **Регистрации пользователей** с хэшированием паролей (Argon2id)
3. **Восстановления пароля** через email с кодом подтверждения

## Необходимые настройки

### 1. Настройка подключения к PostgreSQL

Откройте файл `bd.cs` и измените строку подключения:

```csharp
var connectionString = "Host=localhost;Database=ResearcherLabDB;Username=postgres;Password=your_password";
```

Замените параметры на ваши:
- `Host` - адрес сервера PostgreSQL
- `Database` - имя базы данных
- `Username` - имя пользователя
- `Password` - пароль

### 2. Настройка SMTP для отправки email

Откройте файл `email.cs` и измените настройки:

```csharp
private const string SmtpHost = "smtp.mail.ru";
private const int SmtpPort = 587;
private const string SenderEmail = "your_email@mail.ru";
private const string SenderAppPassword = "your_app_password";
```

#### Для Mail.ru:
1. Зарегистрируйте почту на mail.ru
2. Включите поддержку SMTP в настройках почты
3. Создайте пароль для внешних приложений (https://e.mail.ru/settings/2fa-apps-password)
4. Укажите ваш email и пароль приложения

#### Для Gmail:
1. Используйте `smtp.gmail.com` и порт `587`
2. Включите двухфакторную аутентификацию
3. Создайте пароль приложения (https://myaccount.google.com/apppasswords)

### 3. Установка PostgreSQL

Если у вас не установлен PostgreSQL:
1. Скачайте с https://www.postgresql.org/download/
2. Установите, запомните пароль пользователя postgres
3. Создайте базу данных:
```sql
CREATE DATABASE "ResearcherLabDB";
```

### 4. Установка пакетов NuGet

В Visual Studio:
1. Откройте консоль менеджера пакетов (Tools > NuGet Package Manager > Package Manager Console)
2. Выполните команду:
```
Update-Package
```

Или восстановите пакеты при сборке проекта.

## Структура базы данных

Проект автоматически создаст следующие таблицы при первом запуске:

### researchers
- id (первичный ключ)
- first_name
- last_name
- date_of_birth
- email
- password (хэш Argon2id)
- phone_number
- research_field
- first_publication_date

### password_reset_codes
- id (первичный ключ)
- email
- code
- created_at
- is_used

## Функционал

### Регистрация
1. Откройте окно регистрации (кнопка "Регистрация" на форме авторизации)
2. Заполните все поля
3. Пароль будет захэширован и сохранён в БД

### Авторизация
1. Введите email и пароль
2. Пароль проверяется с помощью хэша

### Восстановление пароля
1. Нажмите "Забыли пароль?" на форме авторизации
2. Введите email
3. Код подтверждения будет отправлен на почту
4. Введите код из письма
5. Введите новый пароль

## Примечания

- Код подтверждения действителен 15 минут
- Пароли хранятся в захэшированном виде (Argon2id)
- Каждый код подтверждения может быть использован только один раз
