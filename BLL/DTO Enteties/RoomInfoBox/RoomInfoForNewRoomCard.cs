using BLL.DTO_Enteties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class RoomInfoForNewRoomCard
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Image Photo { get; set; }
        public string Description { get; set; }
        public GenreDTO Genre { get; set; }
        public int AmountOfMembers { get; set; }
    }
}
