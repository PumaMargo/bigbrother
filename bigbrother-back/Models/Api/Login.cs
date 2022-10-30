using bigbrother_back.Models.DataModel;
using System.ComponentModel.DataAnnotations;

namespace bigbrother_back.Models.Api
{
    public class SaltRequest
    {
        [EmailAddress]
        [MaxLength(256)]
        public string Login { get; set; } = string.Empty;
    }

    public class SaltResponce
    {
        [MaxLength(80)]
        public string HashType { get; set; } = string.Empty;

        [MaxLength(80)]
        public string Salt { get; set; } = string.Empty;
    }

    public class LoginRequest
    {
        [EmailAddress]
        [MaxLength(256)]
        public string Login { get; set; } = string.Empty;

        [MaxLength(256)]
        public string Hash { get; set; } = string.Empty;
    }

    public class LoginResponce
    {
        public int Id { get; set; }

        public AccountRole Role { get; set; } = AccountRole.Customer;

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime RefreshValidTo { get; set; } = DateTime.UtcNow;

        public string AccessToken { get; set; } = string.Empty;

        public DateTime AccessValidTo { get; set; } = DateTime.UtcNow;
    }

    public class RefreshRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class RefreshResponce
    {
        public string RefreshToken { get; set; } = string.Empty;

        public DateTime RefreshValidTo { get; set; } = DateTime.UtcNow;

        public string AccessToken { get; set; } = string.Empty;

        public DateTime AccessValidTo { get; set; } = DateTime.UtcNow;

    }

    public class MeResponce
    {
        public int Id { get; set; }

        [EmailAddress]
        [MaxLength(256)]
        public string Login { get; set; } = string.Empty;

        public AccountRole Role { get; set; } = AccountRole.Customer;

        [MaxLength(256)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(256)]
        public string SecondName { get; set; } = string.Empty;

        public AccountSex? Sex { get; set; } = AccountSex.Male;

        public DateOnly? BirthDate { get; set; }

        public int? MarkerId { get; set; }
    }

}