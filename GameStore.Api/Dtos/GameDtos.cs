namespace GameStore.Api.Dtos;

public record class GameDtos(int Id, string Name, string Genre, decimal Price, DateOnly ReleaseDate);
