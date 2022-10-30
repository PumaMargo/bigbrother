using System.ComponentModel.DataAnnotations;

namespace bigbrother_back.Models.DataModel
{
    public class Marker
    {
        #region Properties

        public int Id { get; set; }

        [MaxLength(256)]
        public string Signature { get; set; } = string.Empty;

        public Place? Place { get; set; }

        #endregion
    }
}
