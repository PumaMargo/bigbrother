namespace bigbrother_back.Models.DataModel
{
    public class Visit
    {
        #region Properties

        public int Id { get; set; }

        public int PlaceId { get; set; }

        public int AccountId { get; set; }

        public DateTime VisitDate { get; set; }

        #endregion
    }
}
