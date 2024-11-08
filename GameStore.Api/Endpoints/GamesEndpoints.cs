using System;
using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameDtos> games = [
        new (1, "Street Fighter II", "Fighting", 19.99M, new DateOnly(1999,7,15)),
    new (2, "Final Fantasy XIV", "Roleplaying", 59.99M, new DateOnly(2010, 9, 30)),
    new (3, "FIFA 23", "Sports", 69.99M, new DateOnly(2022, 9, 27))
    ];

    public static WebApplication MapGamesEndpoints(this WebApplication app)
    {
        //GET /GAMES
        app.MapGet("games", () => games);

        //get / games/1
        app.MapGet("games/{id}", (int id) =>
            {
                GameDtos? game = games.Find(game => game.Id == id);

                return game is null ? Results.NotFound() : Results.Ok(game);
            }).WithName(GetGameEndpointName);

        //POST / games
        app.MapPost("games", (CreateGameDto newGame) =>
        {
            GameDtos game = new GameDtos(
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate
            );
            games.Add(game);

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        });

        //PUT / games
        app.MapPut("games/{id}", (int id, UpdateGameDto updateGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            games[index] = new GameDtos(
                id,
                updateGame.Name,
                updateGame.Genre,
                updateGame.Price,
                updateGame.ReleaseDate
            );
            return Results.NoContent();
        });

        //DELETE /games/2
        app.MapDelete("games/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);

            return Results.NoContent();
        });
        return app;
    }
}
