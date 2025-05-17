using Microsoft.AspNetCore.Mvc;
using SamuraiApp.Shared.Data.DB;
using SamuraiApp.Shared.Model;
using SamuraiApp_API.Responses;

namespace SamuraiApp_API.Endpoints
{
    public static class KenjutsuExtension
    {
        public static void AddEndpointsKenjutsu(this WebApplication app)
        {
            var groupBuilder = app.MapGroup("Kenjutsu")
                .RequireAuthorization()
                .WithTags("Kenjutsu");

            groupBuilder.MapGet("", ([FromServices] DAL<Kenjutsu> dal) =>
            {
                var kenList = dal.Read();
                if (kenList == null) return Results.NotFound();
                var kenResponseList = EntityListToResponseList(kenList);
                return Results.Ok(kenResponseList);
            });
        }

        private static ICollection<KenjutsuResponse>
            EntityListToResponseList(IEnumerable<Kenjutsu> entities)
        {
            return entities.Select(a=>EntityToReponse(a)).ToList();
        }
        private static KenjutsuResponse EntityToReponse(Kenjutsu entity)
        {
            return new KenjutsuResponse(entity.Id, entity.Style);
        }
    }
}
