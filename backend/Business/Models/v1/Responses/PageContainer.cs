namespace Business.Models.v1.Responses;

public record PageContainer<T>(
    IList<T> Items,
    int TotalItemCount,
    int PageSize,
    int PageNumber
    );