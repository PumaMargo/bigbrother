using bigbrother_back.Models.DataModel;
using System.ComponentModel.DataAnnotations;

namespace bigbrother_back.Models.Api
{
    public class CreateAccountRequest
    {
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
    }

    public class EditAccountRequest
    {
        public int Id { get; set; }

        public AccountRole Role { get; set; } = AccountRole.Customer;

        [MaxLength(256)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(256)]
        public string SecondName { get; set; } = string.Empty;

        public AccountSex? Sex { get; set; } = AccountSex.Male;

        public DateOnly? BirthDate { get; set; }
    }

    public class AccountResponce
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
