using SamuraiApp.Shared.Model;

namespace SamuraiApp_API.Responses
{
    public record SamuraiFullResponse(
        int id,
        string name,
        string clan,
        string dojoName,
        ICollection<Kenjutsu> kenjutsu
        );
}
