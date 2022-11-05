using bigbrother_back.Models.DataModel;
using System.ComponentModel.DataAnnotations;

namespace bigbrother_back.Models.Api
{
    public class CreateTagRequest
    {
        public TagType TagType { get; set; } = TagType.Desciption;

        [MaxLength(256)]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public AccountSex? Sex { get; set; } = AccountSex.Male;

        [Range(0, 150)]
        public AgeRange? AgeRange { get; set; }
    }

    public class EditTagRequest : CreateTagRequest
    {
        public int Id { get; set; }
    }

    public class TagResponce
    {
        public int Id { get; set; }

        public TagType TagType { get; set; } = TagType.Desciption;

        [MaxLength(256)]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public AccountSex? Sex { get; set; } = AccountSex.Male;

        public AgeRange? AgeRange { get; set; }
    }
}
