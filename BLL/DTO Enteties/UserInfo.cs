namespace BLL.DTO_Enteties
{
    public class UserInfo
    {
        public System.Drawing.Image Photo { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public System.DateTime? BirthDate { get; set; }
        public SexDTO Sex { get; set; }
        public CountryDTO Country { get; set; }
    }
}
