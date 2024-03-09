using GameStore.Api.DTOs;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.EndPoints;

public static class GamesEndPoints
{
    const string GetGameEndpointName = "GetGame";

    public static RouteGroupBuilder MapGamesEndPoints(this WebApplication app){
            var group = app.MapGroup("games").WithParameterValidation();
            //GET /games
            group.MapGet("/", async (GameStoreContext dbConetxt) => await
            dbConetxt.Games.Include(game=>game.Genre).Select(game => game.ToGameSummaryDto()).AsNoTracking().ToListAsync());

            //Get /games/id
            group.MapGet("/{Id}", async (int Id,GameStoreContext dbContext) =>
            { 
                Game? game =  await dbContext.Games.FindAsync(Id);
                Console.WriteLine(game?.ToString());
                return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());

            }).WithName(GetGameEndpointName);

            //Post /game
            group.MapPost("",async(CreateGameDto newGame,GameStoreContext dbContext)=>{
                Game game = newGame.ToEntity();
                game.Genre = await dbContext.Genres.FindAsync(newGame.GenreId);
                dbContext.Games.Add(game);
                dbContext.SaveChanges();
                return Results.CreatedAtRoute(GetGameEndpointName,new {Id = game.Id},game.ToGameDetailsDto());
            });


            //Put /games/1
            group.MapPut("/{id}",async (int id,UpdateGameDto updatedGame,GameStoreContext dbContext) => {
                var existingGame = await dbContext.Games.FindAsync(id);
           
                if(existingGame is null)
                    return Results.NotFound();
                
                dbContext.Entry(existingGame).CurrentValues.SetValues(updatedGame.ToEntity(id));
                await dbContext.SaveChangesAsync();
                return Results.NoContent();
            });

            //Delete /games/1
            group.MapDelete("/{id}",async (int id,GameStoreContext dbContext) =>
            {
                await dbContext.Games.Where(game => game.Id == id).ExecuteDeleteAsync();
                return Results.NoContent();
            });
        return group;
    }
}