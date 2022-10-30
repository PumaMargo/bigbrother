using System.ComponentModel.DataAnnotations;

namespace bigbrother_back.Models.Api
{
    public class TouchInPlaceRequest
    {
        public int PlaceId { get; set; }

        [MaxLength(256)]
        public string MarkerSignature { get; set; } = string.Empty;
    }
}
