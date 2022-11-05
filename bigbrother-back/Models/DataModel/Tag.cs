using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace bigbrother_back.Models.DataModel
{
    public enum TagType
    {
        Desciption,
        Social
    }

    public class Tag
    {
        #region Properties

        public int Id { get; set; }

        public TagType TagType { get; set; } = TagType.Desciption;

        [MaxLength(256)]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public AccountSex? Sex { get; set; } = AccountSex.Male;

        public AgeRange? AgeRange { get; set; }

        public List<Account>? Accounts { get; set; }

        public List<Place>? Places { get; set; }

        #endregion
    }

    [Owned]
    public class AgeRange
    {
        #region Constants

        public const int MinAge = 0;
        public const int MaxAge = 150;

        #endregion
        
        #region Properties

        [Range(MinAge, MaxAge)]
        public int From { get; set; }

        [Range(MinAge, MaxAge)]
        public int To { get; set; }

        #endregion
    }
}
