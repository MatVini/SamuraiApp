using SamuraiApp_Model;

namespace SamuraiApp_API.Responses
{
    public record KenjutsuSamResponse(
        int id,
        string style,
        ICollection<Samurai> samurai
    );
}
