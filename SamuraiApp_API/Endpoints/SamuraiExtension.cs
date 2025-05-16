using Microsoft.AspNetCore.Mvc;
using SamuraiApp.Shared.Data.DB;
using SamuraiApp_Model;

namespace SamuraiApp_API.Endpoints
{
    public static class SamuraiExtension
    {
        public static void AddEndpointsSamurai(this WebApplication app)
        {
            app.MapGet("/Samurai", ([FromServices] DAL<Samurai> dal) =>
            {
                return Results.Ok(dal.Read());
            });

            app.MapPost("/Samurai", ([FromServices] DAL<Samurai> dal,
                [FromBody] Samurai sam) =>
            {
                dal.Create(sam);
                return Results.Created();
            });

            app.MapDelete("/Samurai/{id}", ([FromServices] DAL<Samurai> dal, int id) =>
            {
                var sam = dal.ReadBy(s => s.Id == id);
                if (sam == null) return Results.NotFound();
                dal.Delete(sam);
                return Results.NoContent();
            });

            app.MapPut("/Samurai/{id}", ([FromServices] DAL<Samurai> dal,
                [FromBody] Samurai sam) =>
            {
                var samToEdit = dal.ReadBy(s => s.Id == sam.Id);
                if (sam == null) return Results.NotFound();
                samToEdit.Name = sam.Name;
                samToEdit.Dojo = sam.Dojo;
                dal.Update(samToEdit);
                return Results.Created();
            });
        }
    }
}
