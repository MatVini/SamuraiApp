using SamuraiApp_Model;

namespace SamuraiApp_API.Responses
{
    public record DojoSamResponse(
        int id,
        string name,
        string region,
        ICollection<Samurai> samurai
    );
}
