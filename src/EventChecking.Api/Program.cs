using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

var events = new List<EventDto>
{
    new(1, "Hoi thao CNTT", "Chia se kien thuc lap trinh", 50),
    new(2, "Ngay hoi viec lam", "Ket noi sinh vien va doanh nghiep", 100),
    new(3, "Giao luu sinh vien", "Hoat dong giao luu tai truong", 80)
};

var eventsLock = new object();

app.MapGet("/api/status", () => Results.Ok(new
{
    service = "EventChecking.Api",
    version = "0.2",
    status = "running"
}));

app.MapGet("/events", () =>
{
    lock (eventsLock)
    {
        return Results.Ok(events.ToArray());
    }
});

app.MapGet("/events/{id:int}", (int id) =>
{
    EventDto? eventItem;

    lock (eventsLock)
    {
        eventItem = events.FirstOrDefault(e => e.Id == id);
    }

    return eventItem is null
        ? Results.NotFound(new ApiError("EVENT_NOT_FOUND", "Khong tim thay su kien."))
        : Results.Ok(eventItem);
});

app.MapPost("/events", async (
    CreateEventRequest request,
    IHubContext<EventHub> hubContext) =>
{
    if (string.IsNullOrWhiteSpace(request.Name) || request.Capacity <= 0)
    {
        return Results.BadRequest(new ApiError(
            "INVALID_EVENT",
            "Ten su kien khong duoc de trong va suc chua phai lon hon 0."));
    }

    EventDto eventItem;

    lock (eventsLock)
    {
        eventItem = new EventDto(
            Id: events.Count == 0 ? 1 : events.Max(e => e.Id) + 1,
            Name: request.Name.Trim(),
            Description: request.Description?.Trim() ?? "",
            Capacity: request.Capacity);

        events.Add(eventItem);
    }

    await hubContext.Clients.All.SendAsync("EventCreated", eventItem);

    return Results.Created($"/events/{eventItem.Id}", eventItem);
});

app.MapHub<EventHub>("/hubs/events");

app.Run();

sealed class EventHub : Hub
{
}

record EventDto(int Id, string Name, string Description, int Capacity);
record CreateEventRequest(string Name, string? Description, int Capacity);
record ApiError(string Code, string Message);
