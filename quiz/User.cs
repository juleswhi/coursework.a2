using System.Runtime.CompilerServices;

namespace quiz; 
record Name(string FirstName, string Surname);
internal class User : IEquatable<User>
{
    public string Username { get; set; } = String.Empty;
    public string? Password { get; set; }
    public string? Email { get; set; }

    public DateTime DateOfBirth { get; set; }
    public Name? Name { get; set; }

    public User(string username, string password)
    {
        Username = username;
        password.Hash(out string hashed);
        Password = hashed;
    }

    public bool Equals(User? other) => this.Username == other?.Username;
    public static bool operator ==(User a, User b) => a.Equals(b);
    public static bool operator !=(User a, User b) => !a.Equals(b);
    public override bool Equals(object? obj) => Equals(obj as User);
    public override int GetHashCode() => base.GetHashCode();

}
