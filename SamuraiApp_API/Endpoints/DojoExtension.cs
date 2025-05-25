using Microsoft.AspNetCore.Mvc;
using SamuraiApp.Shared.Data.DB;
using SamuraiApp_API.Requests;
using SamuraiApp_API.Responses;
using SamuraiApp_Model;

namespace SamuraiApp_API.Endpoints
{
    public static class DojoExtension
    {
        public static void AddEndpointsDojo(this WebApplication app)
        {
            var groupBuilder = app.MapGroup("Dojo")
                .RequireAuthorization()
                .WithTags("Dojo");

            groupBuilder.MapGet("", ([FromServices] DAL<Dojo> dal) =>
            {
                var dojList = dal.Read();
                if (dojList == null) return Results.NotFound();
                var dojResponseList = EntityListToResponseList(dojList);
                return Results.Ok(dojResponseList);
            }).WithSummary("Lists all Dojo and their regions.");

            groupBuilder.MapGet("/{id}",
                (int id,
                [FromServices] DAL<Dojo> dal) =>
            {
                var doj = dal.ReadBy(d=>d.Id == id);
                if (doj == null) return Results.NotFound();
                return Results.Ok(EntityToSamResponse(doj));
            }).WithSummary("Shows a Dojo with its region " +
            "and all enrolled Samurai.");

            groupBuilder.MapPost("",
                ([FromServices] DAL<Dojo> dal,
                [FromBody] DojoRequest doj) =>
            {
                dal.Create(new Dojo(doj.name, doj.region));
                return Results.Created();
            }).WithSummary("Creates a Dojo in a given region.");

            groupBuilder.MapPost("/{id}/samurai",
                (int id,
                [FromServices] DAL<Dojo> dal,
                [FromServices] DAL<Samurai> samDal,
                [FromBody] SamDojKenListRequest sdk) =>
            {
                var doj = dal.ReadBy(d=>d.Id == id);
                if (doj == null) return Results.NotFound();
                foreach (var sId in sdk.ids)
                {
                    var sam = samDal.ReadBy(s=>s.Id == sId);
                    if (sam == null) continue;
                    if (doj.SamCollect.Contains(sam)) continue;
                    doj.SamCollect.Add(sam);
                }
                dal.Update(doj);
                return Results.NoContent();
            }).WithSummary("Enrolls multiple Samurai to Dojo " +
            "given their IDs.");

            groupBuilder.MapDelete("/{id}",
                (int id,
                [FromServices] DAL<Dojo> dal) =>
            {
                var doj = dal.ReadBy(d=>d.Id == id);
                if (doj == null) return Results.NotFound();
                dal.Delete(doj);
                return Results.NoContent();
            }).WithSummary("Deletes a Dojo.");

            groupBuilder.MapDelete("/{id}/samurai",
                (int id,
                [FromServices] DAL<Dojo> dal) =>
            {
                var doj = dal.ReadBy(d=>d.Id == id);
                if (doj == null) return Results.NotFound();
                doj.SamCollect.Clear();
                dal.Update(doj);
                return Results.NoContent();
            }).WithSummary("Removes all Samurai from a Dojo.");

            groupBuilder.MapPut("/{id}",
                ([FromServices] DAL<Dojo> dal,
                [FromBody] DojoEditRequest doj) =>
            {
                if (doj == null) return Results.NotFound();
                var dojToEdit = dal.ReadBy(d=>d.Id == doj.id);
                if (dojToEdit == null) return Results.NotFound();
                dojToEdit.Name = doj.name;
                dojToEdit.Region = doj.region;
                dal.Update(dojToEdit);
                return Results.NoContent();
            }).WithSummary("Updates a Dojo's name and region " +
            "given its ID.");
        }

        private static ICollection<DojoResponse>
            EntityListToResponseList(IEnumerable<Dojo> entities)
        {
            return entities.Select(a=>EntityToResponse(a)).ToList();
        }
        private static DojoResponse EntityToResponse(Dojo entity)
        {
            return new DojoResponse(
                entity.Id,
                entity.Name,
                entity.Region
            );
        }
        private static DojoSamResponse EntityToSamResponse(Dojo doj)
        {
            return new DojoSamResponse(
                doj.Id,
                doj.Name,
                doj.Region,
                doj.SamCollect
                );
        }
    }
}
