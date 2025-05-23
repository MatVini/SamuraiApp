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
            });

            groupBuilder.MapGet("/{id}", (int id,
                [FromServices] DAL<Samurai> dal) =>
            {
                var sam = dal.ReadBy(s=>s.Id == id);
                if (sam == null) Results.NotFound();
                return Results.Ok(EntityToResponse(sam));
            });

            groupBuilder.MapGet("/{id}/full", (int id,
                [FromServices] DAL<Samurai> dal) =>
            {
                var sam = dal.ReadBy(s=>s.Id == id);
                if (sam == null) Results.NotFound();
                return Results.Ok(EntityToFullResponse(sam));
            });

            groupBuilder.MapPost("", ([FromServices] DAL<Samurai> dal,
                [FromServices] DAL<Kenjutsu> kenDal,
                [FromBody] SamuraiRequest sam) =>
            {
                dal.Create(new Samurai(sam.name, sam.clan));
                return Results.Created();
            });
                
            groupBuilder.MapDelete("/{id}",
                ([FromServices] DAL<Samurai> dal, int id) =>
            {
                var sam = dal.ReadBy(s=>s.Id == id);
                if (sam == null) return Results.NotFound();
                dal.Delete(sam);
                return Results.NoContent();
            });

            groupBuilder.MapPut("/{id}", ([FromServices] DAL<Samurai> dal,
                [FromBody] SamuraiEditRequest sam) =>
            {
                if (sam == null) return Results.NotFound();
                var samToEdit = dal.ReadBy(s=>s.Id == sam.id);
                if (samToEdit == null) return Results.NotFound();
                samToEdit.Name = sam.name;
                samToEdit.Clan = sam.clan;
                dal.Update(samToEdit);
                return Results.Created();
            });

            groupBuilder.MapPost("/{id}/dojo",
                ([FromServices] DAL<Samurai> dal,
                [FromServices] DAL<Dojo> dojDal,
                [FromBody] SamuraiDojKenRequest samDK,
                int id) =>
            {
                var sam = dal.ReadBy(s=>s.Id == id);
                var doj = dojDal.ReadBy(d=>d.Id == samDK.id);
                if (sam == null || doj == null)
                    return Results.NotFound();
                sam.Dojo = doj;
                dal.Update(sam);
                return Results.NoContent();
            });

            groupBuilder.MapDelete("/{id}/dojo",
                ([FromServices] DAL<Samurai> dal, int id) =>
                {
                    var sam = dal.ReadBy(s => s.Id == id);
                    if (sam == null) return Results.NotFound();
                    sam.Dojo = null;
                    dal.Update(sam);
                    return Results.NoContent();
                });

            groupBuilder.MapPost("/{id}/kenjutsu",
                ([FromServices] DAL<Samurai> dal,
                [FromServices] DAL<Kenjutsu> kenDal,
                [FromBody] SamuraiDojKenRequest samDK,
                int id) =>
            {
                var sam = dal.ReadBy(s=>s.Id == id);
                var ken = kenDal.ReadBy(k=>k.Id == samDK.id);
                if (sam == null || ken == null)
                    return Results.NotFound();
                sam.KenCollect.Add(ken);
                dal.Update(sam);
                return Results.NoContent();
            });

            groupBuilder.MapDelete("/{id}/kenjutsu",
                ([FromServices] DAL<Samurai> dal, int id) =>
                {
                    var sam = dal.ReadBy(s=>s.Id == id);
                    if (sam == null) return Results.NotFound();
                    sam.KenCollect.Clear();
                    dal.Update(sam);
                    return Results.NoContent();
                });
        }

        private static List<Kenjutsu> KenjutsuRequestConvert
            (ICollection<KenjutsuRequest> kenList,
            DAL<Kenjutsu> kenDal)
        {
            var kenjutsuList = new List<Kenjutsu>();
            foreach (var item in kenList)
            {
                var ken = RequestToEntity(item);
                var foundKen = kenDal.ReadBy(
                    k=>k.Style.ToUpper()
                    .Equals(ken.Style.ToUpper()));
                if (foundKen != null) kenjutsuList.Add(foundKen);
                else kenjutsuList.Add(ken);
            }
            return kenjutsuList;
        }

        private static Kenjutsu RequestToEntity(KenjutsuRequest k)
        {
            return new Kenjutsu(k.style);
        }

        private static ICollection<SamuraiResponse>
            EntityListToResponseList(IEnumerable<Samurai> entities)
        {
            return entities.Select(a=>EntityToResponse(a)).ToList();
        }
        private static SamuraiResponse EntityToResponse(Samurai entity)
        {
            return new SamuraiResponse(entity.Id, entity.Name, entity.Clan);
        }

        private static SamuraiFullResponse EntityToFullResponse(Samurai sam)
        {
            string doj;
            if (sam.Dojo == null) doj = "No Dojo.";
            else doj = sam.Dojo.Name;
            return new SamuraiFullResponse(
                sam.Id, sam.Name, sam.Clan, doj, sam.KenCollect);
        }

    }
}
