namespace GrantsPlatform.Models
{
    public class Grant
    {
        public int? Id { get; set; }

        public string Title { get; set; }

        public string SourceUrl { get; set; }

        public IList<int> ProjectDirections { get; set; }

        public int Amount { get; set; }

        public IList<int> LegalForms { get; set; }

        public int Age { get; set; }

        public IList<int> CuttingOffCriteria { get; set; }
    }
}
