using Microsoft.AspNetCore.Mvc;
using SamuraiApp.Shared.Data.DB;
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
            });

            groupBuilder.MapGet("/{id}", (int id,
                [FromServices] DAL<Dojo> dal) =>
            {
                var doj = dal.ReadBy(d=>d.Id == id);
                if (doj == null) Results.NotFound();
                return Results.Ok(EntityToResponse(doj));
            });

            groupBuilder.MapPost("", ([FromServices] DAL<Dojo> dal,
                [FromBody] Dojo doj) =>
            {
                dal.Create(new Dojo(doj.Name, doj.Region));
                return Results.Created();
            });

            groupBuilder.MapDelete("/{id}",
                ([FromServices] DAL<Dojo> dal, int id) =>
            {
                var doj = dal.ReadBy(d=>d.Id == id);
                if (doj == null) return Results.NotFound();
                dal.Delete(doj);
                return Results.NoContent();
            });

            groupBuilder.MapPut("/{id}", ([FromServices] DAL<Dojo> dal,
                [FromBody] Dojo doj) =>
            {
                if (doj == null) return Results.NotFound();
                var dojToEdit = dal.ReadBy(d=>d.Id == doj.Id);
                dojToEdit.Name = doj.Name;
                dojToEdit.Region = doj.Region;
                dal.Update(dojToEdit);
                return Results.Created();
            });
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
    }
}
