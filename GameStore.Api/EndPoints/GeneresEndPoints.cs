using Microsoft.EntityFrameworkCore;

namespace GameStore.Api;

public static  class GeneresEndPoints
{
    public static RouteGroupBuilder MapGenresEndpoints(this WebApplication app){
        var group = app.MapGroup("geners");
        group.Map("/",async (GameStoreContext dbContext) => 
        await dbContext.Genres.Select(genre => genre.ToGenreDto()).AsNoTracking().ToListAsync()
        );
        return group;
    }
}
