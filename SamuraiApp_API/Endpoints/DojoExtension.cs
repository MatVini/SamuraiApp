using Microsoft.AspNetCore.Mvc;
using SamuraiApp.Shared.Data.DB;
using SamuraiApp_Model;

namespace SamuraiApp_API.Endpoints
{
    public static class DojoExtension
    {
        public static void AddEndpointsDojo(this WebApplication app)
        {
            app.MapGet("/Dojo", ([FromServices] DAL<Dojo> dal) =>
            {
                return Results.Ok(dal.Read());
            });

            app.MapPost("/Dojo", ([FromServices] DAL<Dojo> dal,
                [FromBody] Dojo doj) =>
            {
                dal.Create(doj);
                return Results.Created();
            });

            app.MapDelete("/Dojo/{id}", ([FromServices] DAL<Dojo> dal, int id) =>
            {
                var doj = dal.ReadBy(d => d.Id == id);
                if (doj == null) return Results.NotFound();
                dal.Delete(doj);
                return Results.NoContent();
            });

            app.MapPut("/Dojo/{id}", ([FromServices] DAL<Dojo> dal,
                [FromBody] Dojo doj) =>
            {
                var dojToEdit = dal.ReadBy(d => d.Id == doj.Id);
                if (doj == null) return Results.NotFound();
                dojToEdit.Name = doj.Name;
                dojToEdit.Region = doj.Region;
                dal.Update(dojToEdit);
                return Results.Created();
            });
        }
    }
}
