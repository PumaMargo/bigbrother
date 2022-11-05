using System.ComponentModel.DataAnnotations;

namespace bigbrother_back.Models.Api
{
    public class CreatePlaceRequest
    {
        [MaxLength(256)]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }

    public class EditPlaceRequest : CreatePlaceRequest
    {
        public int Id { get; set; }
    }

    public class PlaceResponce
    {
        public int Id { get; set; }

        [MaxLength(256)]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
