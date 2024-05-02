namespace CoreBus.DTOs
{
    public class Common
    {
        public int PassangerId { get; set; }

        public string? Name { get; set; }

        public bool IsPaid { get; set; }

        public DateTime JournyDate { get; set; }

        public IFormFile? ImageFile { get; set; }
        public string? ImageName { get; set; }
        public string? BusTypes { get; set; }
    }
}
