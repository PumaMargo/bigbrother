using bigbrother_back.Models.DataModel;
using System.ComponentModel.DataAnnotations;

namespace bigbrother_back.Models.Api
{
    public class CreateMarkerRequest
    {
        [MaxLength(256)]
        public string Signature { get; set; } = string.Empty;
    }

    public class MarkerResponce
    {
        public int Id { get; set; }

        public int? PlaceId { get; set; }
    }
}
