using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SamuraiApp.Shared.Data.DB;
using SamuraiApp.Shared.Model;
using SamuraiApp_API.Requests;
using SamuraiApp_API.Responses;
using SamuraiApp_Model;

namespace SamuraiApp_API.Endpoints
{
    public static class SamuraiExtension
    {
        public static void AddEndpointsSamurai(this WebApplication app)
        {
            var groupBuilder = app.MapGroup("Samurai")
                .RequireAuthorization()
                .WithTags("Samurai");

            groupBuilder.MapGet("", ([FromServices] DAL<Samurai> dal) =>
            {
                var samList = dal.Read();
                if (samList == null) return Results.NotFound();
                var samResponseList = EntityListToResponseList(samList);
                return Results.Ok(samResponseList);
            }).WithSummary("Lists all Samurai and their clans.");

            groupBuilder.MapGet("/{id}",
                (int id,
                [FromServices] DAL<Samurai> dal,
                [FromServices] DAL<Kenjutsu> kenDal) =>
            {
                var sam = dal.ReadBy(s=>s.Id == id);
                if (sam == null) return Results.NotFound();
                var kenList = new List<Kenjutsu>();
                foreach (var kRes in sam.KenCollect)
                {
                    var ken = kenDal.ReadBy(k=>k.Id == kRes.Id);
                    kenList.Add(ken);
                }
                return Results.Ok(EntityToFullResponse(sam));
            }).WithSummary("Shows Samurai with their clan, " +
            "what Dojo they're enrolled in, and " +
            "all their known Kenjutsu.");

            groupBuilder.MapPost("",
                ([FromServices] DAL<Samurai> dal,
                [FromBody] SamuraiRequest sam) =>
            {
                dal.Create(new Samurai(sam.name, sam.clan));
                return Results.Created();
            }).WithSummary("Creates a Samurai and puts them in a clan.");
                
            groupBuilder.MapDelete("/{id}",
                (int id,
                [FromServices] DAL<Samurai> dal) =>
            {
                var sam = dal.ReadBy(s=>s.Id == id);
                if (sam == null) return Results.NotFound();
                dal.Delete(sam);
                return Results.NoContent();
            }).WithSummary("Deletes Samurai.");

            groupBuilder.MapPut("",
                ([FromServices] DAL<Samurai> dal,
                [FromBody] SamuraiEditRequest sam) =>
            {
                if (sam == null) return Results.NotFound();
                var samToEdit = dal.ReadBy(s=>s.Id == sam.id);
                if (samToEdit == null) return Results.NotFound();
                samToEdit.Name = sam.name;
                samToEdit.Clan = sam.clan;
                dal.Update(samToEdit);
                return Results.NoContent();
            }).WithSummary("Updates Samurai's name and clan " +
            "given their ID.");

            groupBuilder.MapPost("/{id}/dojo",
                (int id,
                [FromServices] DAL<Samurai> dal,
                [FromServices] DAL<Dojo> dojDal,
                [FromBody] SamDojKenRequest sdk) =>
            {
                var sam = dal.ReadBy(s=>s.Id == id);
                var doj = dojDal.ReadBy(d=>d.Id == sdk.id);
                if (sam == null || doj == null)
                    return Results.NotFound();
                sam.Dojo = doj;
                dal.Update(sam);
                return Results.NoContent();
            }).WithSummary("Enrolls Samurai to a Dojo given its ID.");

            groupBuilder.MapDelete("/{id}/dojo",
                (int id,
                [FromServices] DAL<Samurai> dal) =>
                {
                    var sam = dal.ReadBy(s=>s.Id == id);
                    if (sam == null) return Results.NotFound();
                    // apparently doesn't work
                    // must set key to null explicitly
                    // not sure if that works with current DB configuration
                    // will leave as is
                    sam.Dojo = null;
                    dal.Update(sam);
                    return Results.NoContent();
                }).WithSummary("Removes Samurai from Dojo.");

            groupBuilder.MapPost("/{id}/kenjutsu",
                (int id,
                [FromServices] DAL<Samurai> dal,
                [FromServices] DAL<Kenjutsu> kenDal,
                [FromBody] SamDojKenRequest sdk) =>
            {
                var sam = dal.ReadBy(s=>s.Id == id);
                var ken = kenDal.ReadBy(k=>k.Id == sdk.id);
                if (sam == null || ken == null)
                    return Results.NotFound();
                sam.KenCollect.Add(ken);
                dal.Update(sam);
                return Results.NoContent();
            }).WithSummary("Teaches Samurai a Kenjutsu given its ID.");

            groupBuilder.MapDelete("/{id}/kenjutsu",
                (int id,
                [FromServices] DAL<Samurai> dal) =>
                {
                    var sam = dal.ReadBy(s=>s.Id == id);
                    if (sam == null) return Results.NotFound();
                    sam.KenCollect.Clear();
                    dal.Update(sam);
                    return Results.NoContent();
                }).WithSummary("Removes all known Kenjutsu from Samurai.");
        }

        public static ICollection<SamuraiResponse>
            EntityListToResponseList(IEnumerable<Samurai> entities)
        {
            return entities.Select(a=>EntityToResponse(a)).ToList();
        }
        private static SamuraiResponse EntityToResponse(Samurai entity)
        {
            return new SamuraiResponse(
                entity.Id,
                entity.Name,
                entity.Clan
                );
        }

        private static SamuraiFullResponse EntityToFullResponse(Samurai sam)
        {
            string doj;
            var kenList = new List<KenjutsuResponse>();
            if (sam.Dojo == null) doj = "No Dojo.";
            else doj = sam.Dojo.Name;
            if (!sam.KenCollect.IsNullOrEmpty())
            {
                kenList = (List<KenjutsuResponse>)KenjutsuExtension
                    .EntityListToResponseList(sam.KenCollect);
            }
            return new SamuraiFullResponse(
                sam.Id, sam.Name, sam.Clan, doj, kenList);
        }

    }
}
