﻿namespace SamuraiApp_API.Requests
{
    public record SamuraiEditRequest (
        int id,
        string name,
        string clan
        );
}
