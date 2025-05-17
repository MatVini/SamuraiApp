using Microsoft.AspNetCore.Mvc;
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

            groupBuilder.MapPost("", ([FromServices] DAL<Samurai> dal,
                [FromServices] DAL<Kenjutsu> kenDal,
                [FromBody] SamuraiRequest sam) =>
            {
                dal.Create(
                    new Samurai(sam.name, sam.clan)
                    {
                        KenCollect = sam.Kenjutsu != null?
                        KenjutsuRequestConvert(sam.Kenjutsu, kenDal) :
                        new List<Kenjutsu>()
                    });
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
                samToEdit.Name = sam.name;
                samToEdit.Clan = sam.clan;
                dal.Update(samToEdit);
                return Results.Created();
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
    }
}
