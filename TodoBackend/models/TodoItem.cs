namespace TodoBackend.models;

public class TodoItem
{
    public string? Id { get; set; }= Guid.NewGuid().ToString();
    public string Description { get; set; }= string.Empty;
    public bool Done { get; set; }
}