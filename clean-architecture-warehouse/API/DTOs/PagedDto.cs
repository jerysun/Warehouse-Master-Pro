namespace API.DTOs;

public record class PagedDto
(
    int TotalCount,
    int PageSize,
    int CurrentPage,
    int TotalPages,
    bool HasNext,
    bool HasPrevious
);
