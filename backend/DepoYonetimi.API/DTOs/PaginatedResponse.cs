namespace DepoYonetimi.API.DTOs;

/// <summary>
/// Sayfalanmış liste yanıtları için generic response modeli
/// </summary>
public class PaginatedResponse<T>
{
    public bool Success { get; set; } = true;
    public List<T> Data { get; set; } = new List<T>();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public string? Message { get; set; }
}
