using bigbrother_back.Utility;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace bigbrother_back.Models.DataModel
{
    public enum AccountRole
    {
        Administrator,
        Manager,
        Customer
    }

    public enum AccountSex
    {
        Male,
        Female
    }

    public class Account
    {
        #region Properties

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


        [MaxLength(80)]
        public string Salt { get; set; } = string.Empty;

        [MaxLength(80)]
        public string HashType => "sha256";

        [MaxLength(256)]
        public string Hash { get; set; } = string.Empty;

        public Marker? Marker { get; set; }

        public List<Tag>? Tags { get; set; }

        #endregion

        #region Methods

        internal void BuildPasswordHash(string password)
        {
            var salt = Security.GenerateSalt();
            Debug.Assert(salt.Length == 80);

            Hash = Security.GetSHA256Hash($"{password}{salt}");
            Salt = salt;
        }

        #endregion
    }
}
