using GameStore.Api.DTOs;
using GameStore.Api.Entities;

namespace GameStore.Api;

public  static class GenereMapping
{
    public static GenreDto ToGenreDto(this Genre genre){
        return new GenreDto(genre.Id,genre.Name);
    }

}
