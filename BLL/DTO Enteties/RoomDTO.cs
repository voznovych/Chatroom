namespace BLL.DTO_Enteties
{
    public class RoomDTO
    {
        public int Id { get; set; }

        public int CreatorId { get; set; }
        public UserDTO Creator { get; set; }

        public string Name { get; set; }

        public int GenreId { get; set; }
        public GenreDTO Genre { get; set; }

        public string Description { get; set; }
        public System.Drawing.Image Photo { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsPersonal { get; set; }
    }
}
