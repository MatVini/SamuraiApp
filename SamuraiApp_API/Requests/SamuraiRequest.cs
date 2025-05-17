using SamuraiApp.Shared.Model;

namespace SamuraiApp_API.Requests
{
    public record SamuraiRequest (
        string name,
        string clan,
        ICollection<KenjutsuRequest> Kenjutsu = null
        );
}
