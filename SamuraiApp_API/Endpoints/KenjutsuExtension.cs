using Microsoft.AspNetCore.Mvc;
using SamuraiApp.Shared.Data.DB;
using SamuraiApp.Shared.Model;
using SamuraiApp_API.Requests;
using SamuraiApp_API.Responses;
using SamuraiApp_Model;

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
            }).WithSummary("Lists all Kenjutsu styles.");

            groupBuilder.MapGet("/{id}",
                (int id,
                [FromServices] DAL<Kenjutsu> dal) =>
            {
                var ken = dal.ReadBy(k=>k.Id == id);
                if (ken == null) return Results.NotFound();
                return Results.Ok(EntityToSamResponse(ken));
            }).WithSummary("Shows Kenjutsu and " +
            "all Samurai that practice it.");

            groupBuilder.MapPost("",
                ([FromServices] DAL<Kenjutsu> dal,
                [FromBody] KenjutsuRequest ken) =>
            {
                dal.Create(new Kenjutsu(ken.style));
                return Results.Created();
            }).WithSummary("Creates a Kenjutsu style.");

            groupBuilder.MapPost("/{id}/samurai",
                (int id,
                [FromServices] DAL<Kenjutsu> dal,
                [FromServices] DAL<Samurai> samDal,
                [FromBody] SamDojKenListRequest sdk) =>
            {
                var ken = dal.ReadBy(k=>k.Id == id);
                if (ken == null) return Results.NotFound();
                foreach (var sId in sdk.ids)
                {
                    var sam = samDal.ReadBy(s=>s.Id == sId);
                    if (sam == null) continue;
                    if (ken.SamCollect.Contains(sam)) continue;
                    ken.SamCollect.Add(sam);
                }
                dal.Update(ken);
                return Results.NoContent();
            }).WithSummary("Teaches multiple Samurai a Kenjutsu " +
            "given their IDs.");

            groupBuilder.MapDelete("/{id}",
                (int id,
                [FromServices] DAL<Kenjutsu> dal) =>
            {
                var ken = dal.ReadBy(k=>k.Id == id);
                if (ken == null) return Results.NotFound();
                dal.Delete(ken);
                return Results.NoContent();
            }).WithSummary("Deletes Kenjutsu.");

            groupBuilder.MapDelete("/{id}/samurai",
                (int id,
                [FromServices] DAL<Kenjutsu> dal) =>
            {
                var ken = dal.ReadBy(k => k.Id == id);
                if (ken == null) return Results.NotFound();
                ken.SamCollect.Clear();
                dal.Update(ken);
                return Results.NoContent();
            }).WithSummary("Unassigns all Samurai from Kenjutsu.");

            groupBuilder.MapDelete("/{id}/samurai/partial",
                (int id,
                [FromServices] DAL<Kenjutsu> dal,
                [FromServices] DAL<Samurai> samDal,
                [FromBody] SamDojKenListRequest sdk) =>
            {
                var ken = dal.ReadBy(k=>k.Id == id);
                if (ken == null)
                    return Results.NotFound();
                foreach (var sId in sdk.ids)
                {
                    var sam = samDal.ReadBy(s=>s.Id == sId);
                    if (sam == null) continue;
                    if (!ken.SamCollect.Contains(sam)) continue;
                    ken.SamCollect.Remove(sam);
                }
                dal.Update(ken);
                return Results.NoContent();
            }).WithSummary("Unassigns some Samurai from Kenjutsu " +
            "given their IDs.");

            groupBuilder.MapPut("/{id}",
                ([FromServices] DAL<Kenjutsu> dal,
                [FromBody] KenjutsuEditRequest ken) =>
            {
                if (ken == null) return Results.NotFound();
                var kenToEdit = dal.ReadBy(k => k.Id == ken.id);
                if (kenToEdit == null) return Results.NotFound();
                kenToEdit.Style = ken.style;
                dal.Update(kenToEdit);
                return Results.NoContent();
            }).WithSummary("Updates Kenjutsu's style " +
            "given its ID.");
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
        private static KenjutsuSamResponse EntityToSamResponse(Kenjutsu ken)
        {
            return new KenjutsuSamResponse(
                ken.Id,
                ken.Style,
                ken.SamCollect
                );
        }
    }
}
