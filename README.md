# NotificationsService
Разработка распределенного сервиса отправки уведомлений на разные источники

## О проекте
Стек: `.net8.0`
## Структура
- Общая библиотека `CoreLib/CoreLib.sln`
- микросервисы для отправки уведомлений
  1. `SMSSender/SMSSender.sln`
  2. `WhatsappSender/WhatsappSender.sln`
  3. `EmailSender/EmailSender.sln`
- `Contracts/Contracts.sln` - контракты для связывания Notifications Gateway и Notification Microservices
- `MessagePublisher/MessagePublisher.sln` - Notifications Gateway
## Особенности
- Логирование с помощью Serilog.
- Реализованы отправления в следующих каналах: SMS, WhatsApp, Email.
- В качестве MessageBroker используется RabbitMQ.
- База данных - PostgreSql
- Для каждого канала своя очередь сообщений. Результаты каждый микросервис отправки записывает в единую очередь. За ней следит Notifications Gateway, актуализирует статусы отправлений, и в случае неуспешного результата помещает заново в очереди микросервисов отправлений.
## Сборка и запуск
Собрать билды в следующем порядке:
1. `CoreLib.sln`
2. Senders:
   - `SMSSender.sln`,
   - `WhatsappSender.sln`
   - `EmailSender.sln`
3. `Contracts.sln`
4. `MessagePublisher.sln`
