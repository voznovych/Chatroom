namespace BLL.DTO_Enteties
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public System.DateTime? DateOfBirth { get; set; }

        public SexDTO Sex { get; set; }
        public int SexId { get; set; }

        public int? CountryId { get; set; }
        public CountryDTO Country { get; set; }

        public System.Drawing.Image Photo { get; set; }

        public int StatusId { get; set; }
        public UserStatusDTO Status { get; set; }
    }
}
