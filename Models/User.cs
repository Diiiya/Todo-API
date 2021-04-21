using System;

namespace TodoApi.Models
{
    // public record User
    // {
    //     public Guid Id { get; init; }
    //     public string Username { get; init; }
    //     public string Email { get; init; }
    //     public string Password { get; init; }
    //     public DateTimeOffset CreatedDate { get; init; }
    //     public bool Deleted { get; init; }
    // }
    public record User(Guid Id, string Username, string Email, string Password, DateTimeOffset CreatedDate, bool Deleted);
}