
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Danh sach su kien mau (luu trong RAM)
var events = new List<EventDto>
{
    new(1, "Hoi thao CNTT", "Chia se kien thuc lap trinh", 50),
    new(2, "Ngay hoi viec lam", "Ket noi sinh vien va doanh nghiep", 100),
    new(3, "Giao luu sinh vien", "Hoat dong giao luu tai truong", 80)
};

// Kiem tra API
app.MapGet("/", () => Results.Ok(new
{
    service = "EventChecking.Api",
    status = "running"
}));

// GET: Lay danh sach su kien
app.MapGet("/events", () => Results.Ok(events));

// GET: Lay chi tiet su kien theo ID
app.MapGet("/events/{id:int}", (int id) =>
{
    var eventItem = events.FirstOrDefault(e => e.Id == id);

    return eventItem is null
        ? Results.NotFound(new ApiError(
            "EVENT_NOT_FOUND",
            "Khong tim thay su kien."))
        : Results.Ok(eventItem);
});

// POST: Tao su kien moi
app.MapPost("/events", (CreateEventRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Name)
        || request.Capacity <= 0)
    {
        return Results.BadRequest(new ApiError(
            "INVALID_EVENT",
            "Ten su kien khong duoc de trong va suc chua phai lon hon 0."));
    }

    var eventItem = new EventDto(
        Id: events.Count == 0 ? 1 : events.Max(e => e.Id) + 1,
        Name: request.Name.Trim(),
        Description: request.Description?.Trim() ?? "",
        Capacity: request.Capacity);

    events.Add(eventItem);

    return Results.Created($"/events/{eventItem.Id}", eventItem);
});

app.Run();

// Model su kien
record EventDto(int Id, string Name, string Description, int Capacity);

// Du lieu gui len khi tao su kien
record CreateEventRequest(string Name, string? Description, int Capacity);

// Thong bao loi
record ApiError(string Code, string Message);
