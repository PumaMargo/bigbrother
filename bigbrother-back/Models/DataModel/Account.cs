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

    public class Account
    {
        #region Properties

        public int Id { get; set; }

        [MaxLength(256)]
        public string Login { get; set; } = string.Empty;

        public AccountRole Role { get; set; } = AccountRole.Customer;

        [MaxLength(256)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(256)]
        public string SecondName { get; set; } = string.Empty;

        [MaxLength(80)]
        public string Salt { get; set; } = string.Empty;

        [MaxLength(256)]
        public string Hash { get; set; } = string.Empty;

        [MaxLength(80)]
        public string HashType => "sha256";

        public Marker? Marker { get; set; }

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
