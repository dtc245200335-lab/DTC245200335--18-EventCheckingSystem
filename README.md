# DTC245200335--18-EventCheckingSystem

## EventChecking v0.2 - REST API + SignalR

Do an ca nhan mon Phat trien ung dung phan tan - De tai 18: He thong dang ky su kien va check-in.

Phien ban nay la bai thuc hanh mo rong tu Chuong 3 (SignalR). Du an van su dung du lieu RAM; chua co RabbitMQ, database, dang ky tham du hoac check-in.

### Cong nghe

- C#, ASP.NET Core Minimal API (.NET 8)
- SignalR (server trong ASP.NET Core; JavaScript client tu CDN theo bai hoc)
- HTML, CSS, JavaScript
- Postman hoac trinh duyet de kiem thu HTTP

### Chay du an

Mo Terminal tai thu muc goc repository:

```powershell
dotnet run --project .\src\EventChecking.Api\EventChecking.Api.csproj --launch-profile http
```

Trinh duyet: http://localhost:5265/

API kiem tra: http://localhost:5265/api/status

Luu y: `GET /` cua v0.1 duoc chuyen thanh `GET /api/status` de URL `/` hien thi trang demo HTML.

### REST API

| Method | Endpoint | Chuc nang |
| --- | --- | --- |
| GET | `/api/status` | Kiem tra server |
| GET | `/events` | Danh sach su kien |
| GET | `/events/{id}` | Chi tiet su kien |
| POST | `/events` | Tao su kien trong RAM |

Mau POST `/events` (Body -> raw -> JSON trong Postman):

```json
{
  "name": "Hoi thao AI 2026",
  "description": "Tim hieu ung dung AI trong lap trinh",
  "capacity": 150
}
```

- Thanh cong: `201 Created`.
- Ten trong hoac suc chua <= 0: `400 Bad Request`.
- ID khong ton tai: `404 Not Found`.

### SignalR

- Hub: `/hubs/events`
- Client nhan su kien: `EventCreated`
- Khi POST `/events` thanh cong, `IHubContext<EventHub>` phat `EventCreated` den cac client dang ket noi thong qua `Clients.All`.
- Giao dien tai `/` tu goi `GET /events` de hien thi lai danh sach sau thong bao SignalR.

### Kiem thu hai tab

1. Chay API; mo `http://localhost:5265/` o Tab A va Tab B.
2. Xac nhan ca hai tab hien `SignalR da ket noi`.
3. Tai Tab A, tao mot su kien bang form, hoac POST bang Postman.
4. Xem nhat ky ca hai tab: co `EventCreated`, danh sach tu cap nhat khong reload trang.
5. Thu dong/mat ket noi: su dung REST `GET /events` de doc lai danh sach moi nhat sau khi ket noi lai.
6. Tat va mo lai API: su kien tao moi mat do du lieu van luu trong RAM.

Trang HTML su dung CDN cho thu vien SignalR JavaScript va can Internet khi tai lan dau.

### Pham vi thuc hien

Da co: REST GET/POST, kiem tra du lieu dau vao, SignalR Hub, `IHubContext`, thong bao realtime, giao dien hai tab.

Chua co: dang ky/huy dang ky, sinh ma check-in, check-in chong trung, database, RabbitMQ pub/sub, Audit Worker, Dashboard Service doc lap. Cac muc nay se duoc phat trien sau theo yeu cau de tai va tien do bai hoc.

Luu y: Day la demo hoc tap, khong phai he thong realtime da dam bao do tin cay hoac bao mat cho moi truong production.
