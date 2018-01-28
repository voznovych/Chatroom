namespace BLL.DTO_Enteties
{
    public class VisitInfoDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public UserDTO User { get; set; }

        public int RoomId { get; set; }
        public RoomDTO Room { get; set; }

        public System.DateTime DateOfLastVisit { get; set; }
    }
}
