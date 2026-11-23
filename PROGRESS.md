# PROGRESS - EventCheckingSystem

## Tuan 01 - 16/11/2026 - v0.1

- Khoi tao ASP.NET Core Minimal API (.NET 8).
- Tao model EventDto va CreateEventRequest.
- Trien khai GET /events, GET /events/{id}, POST /events.
- Test GET/POST bang Postman, xac nhan 200 OK va 201 Created.
- Du lieu luu tam trong RAM.

## Tuan 02 - 23/11/2026 - v0.2 (can kiem thu va push)

- Bo sung SignalR: AddSignalR va MapHub<EventHub> tai /hubs/events.
- Sau khi POST /events thanh cong, IHubContext phat EventCreated den cac client dang ket noi.
- Tao trang web wwwroot/index.html de xem danh sach va tao su kien.
- Client nhan EventCreated va tai lai GET /events de cap nhat bang du lieu.
- Chuyen endpoint kiem tra trang thai sang /api/status; / phuc vu trang HTML.
- Can chay build, kiem thu hai tab va chup minh chung truoc khi commit.

## Viec chua trien khai

- REST API dang ky, huy dang ky va check-in.
- Database, RabbitMQ, Dashboard Service va Audit Worker.
