namespace SamuraiApp_API.Requests
{
    public record DojoEditRequest(
        int id,
        string name,
        string region
        );
}
