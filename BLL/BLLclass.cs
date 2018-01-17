using AutoMapper;
using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLClass
    {
        private readonly DAL.Interfaces.IDAL _dal;
        private User AuthenticatedUser { get; set; }
        static BLLClass()
        {
            // Init AutoMapper
            Mapper.Initialize(cfg =>
            {
                // entity to dto
                cfg.CreateMap<User, UserDTO>()
                    .ForMember(dest => dest.Photo,
                        opts => opts.Ignore())
                    //opts => opts.MapFrom(src => Util.ByteArrayToImage(src.Photo)))
                    .ForMember(dest => dest.CreatedRooms,
                        opts => opts.Ignore())
                    .ForMember(dest => dest.Messages,
                        opts => opts.Ignore())
                    .ForMember(dest => dest.Rooms,
                        opts => opts.Ignore())
                    .ForMember(dest => dest.VisitInfos,
                        opts => opts.Ignore());

                cfg.CreateMap<Room, RoomDTO>()
                    .ForMember(dest => dest.Photo,
                            opts => opts.Ignore())
                    .ForMember(dest => dest.Messages,
                        opts => opts.Ignore())
                    .ForMember(dest => dest.Users,
                        opts => opts.Ignore())
                    .ForMember(dest => dest.VisitInfos,
                        opts => opts.Ignore());

                cfg.CreateMap<Sex, SexDTO>()
                    .ForMember(dest => dest.Users,
                        opts => opts.Ignore());

                cfg.CreateMap<Country, CountryDTO>()
                    .ForMember(dest => dest.Users,
                        opts => opts.Ignore());

                cfg.CreateMap<UserStatus, UserStatusDTO>()
                    .ForMember(dest => dest.Users,
                        opts => opts.Ignore());

                cfg.CreateMap<VisitInfo, VisitInfoDTO>();

                cfg.CreateMap<Message, MessageDTO>();

                cfg.CreateMap<Genre, GenreDTO>()
                    .ForMember(dest => dest.Rooms,
                        opts => opts.Ignore());

                // dto to entity
                cfg.CreateMap<UserDTO, User>()
                    .ForMember(dest => dest.Photo,
                        opts => opts.Ignore())
                    //opts => opts.MapFrom(src => Util.ImageToByteArray(src.Photo)))
                    .ForMember(dest => dest.CreatedRooms,
                        opts => opts.Ignore())
                    .ForMember(dest => dest.Messages,
                        opts => opts.Ignore())
                    .ForMember(dest => dest.Rooms,
                        opts => opts.Ignore())
                    .ForMember(dest => dest.VisitInfos,
                        opts => opts.Ignore());

                cfg.CreateMap<RoomDTO, Room>()
                    .ForMember(dest => dest.Photo,
                            opts => opts.Ignore())
                    .ForMember(dest => dest.Messages,
                        opts => opts.Ignore())
                    .ForMember(dest => dest.Users,
                        opts => opts.Ignore())
                    .ForMember(dest => dest.VisitInfos,
                        opts => opts.Ignore());

                cfg.CreateMap<Sex, SexDTO>()
                    .ForMember(dest => dest.Users,
                        opts => opts.Ignore());

                cfg.CreateMap<Country, CountryDTO>()
                    .ForMember(dest => dest.Users,
                        opts => opts.Ignore());

                cfg.CreateMap<UserStatus, UserStatusDTO>()
                    .ForMember(dest => dest.Users,
                        opts => opts.Ignore());

                cfg.CreateMap<VisitInfo, VisitInfoDTO>();

                cfg.CreateMap<Message, MessageDTO>();

                cfg.CreateMap<Genre, GenreDTO>()
                    .ForMember(dest => dest.Rooms,
                        opts => opts.Ignore());

            });
        }

        #region Get rooms feature
        public class RoomInfo
        {
            public MessageDTO LastMessage { get; set; }
            public RoomDTO Room { get; set; }
            public int AmountOfUnreadedMsgs { get; set; }

        }

        public bool RoomExists(int RoomId)
        {
            return _dal.Rooms.GetAll().SkipWhile( r => r.Id != RoomId).Count() != 0;
        }

        public bool UserExists(int UserId)
        {
            return _dal.Users.GetAll().SkipWhile(u => u.Id != UserId).Count() != 0;
        }

        public IEnumerable<RoomInfo> GetInfosAboutAllUserRooms()
        {
            //return _dal.Rooms.GetAll().Where(r => r.Users.Contains(AuthenticatedUser)).ToList();
            return _dal.Users.GetAll().First(u => u.Id == AuthenticatedUser.Id).
                Rooms.Select(r => new RoomInfo
                {
                    Room = Converter.ToRoomDTO(r),
                    AmountOfUnreadedMsgs = GetAmountOfUnreadedMessages(r.Id),
                    LastMessage = Converter.ToMessageDTO(_dal.Messages.GetAll().Where(m => m.RoomId == r.Id).LastOrDefault())
                });
        }

        public IEnumerable<RoomInfo> GetInfosAboutAllUserRooms(int UserId)
        {
            //return _dal.Rooms.GetAll().Where(r => r.Users.Contains(AuthenticatedUser));
            if(UserExists(UserId))
            {
                throw new Exception("User cannot be found!");
            }
            return _dal.Users.GetAll().First(u => u.Id == UserId).
                Rooms.Select(r => new RoomInfo
                {
                    Room = Converter.ToRoomDTO(r),
                    AmountOfUnreadedMsgs = GetAmountOfUnreadedMessages(r.Id),
                    LastMessage = Converter.ToMessageDTO(_dal.Messages.GetAll().Where(m => m.RoomId == r.Id).LastOrDefault())
                });
        }



        public IEnumerable<RoomDTO> GetAllUserRooms(int UserId)
        {
            //return _dal.Rooms.GetAll().Where(r => r.Users.Contains(AuthenticatedUser));
            if (UserExists(UserId))
            {
                throw new Exception("User cannot be found!");
            }
            return _dal.Users.GetAll().First(u => u.Id == UserId).Rooms.Select(r => Converter.ToRoomDTO(r));
        }

        public IEnumerable<RoomDTO> GetAllUserRooms()
        {
            //return _dal.Rooms.GetAll().Where(r => r.Users.Contains(AuthenticatedUser)).ToList();
            return _dal.Users.GetAll().First(u => u.Id == AuthenticatedUser.Id).Rooms.Select(r => Converter.ToRoomDTO(r));
        }

        public int GetAmountOfUnreadedMessages(int RoomId)
        {
            if (RoomExists(RoomId))
            {
                throw new Exception("Room cannot be found!");
            }
            var lastDateOfVisisit = _dal.VisitInfos.GetAll().First(inf => inf.RoomId == RoomId && inf.UserId == AuthenticatedUser.Id).LastDateOfVisit;
            return _dal.Messages.GetAll().Where(m => m.RoomId == RoomId && m.DateOfSend > lastDateOfVisisit).Count();
        }


        public int GetAmountOfUnreadedMessages(int RoomId, int UserId)
        {
            if (RoomExists(RoomId))
            {
                throw new Exception("Room cannot be found!");
            }
            if (UserExists(UserId))
            {
                throw new Exception("User cannot be found!");
            }
            var lastDateOfVisisit = _dal.VisitInfos.GetAll().First(inf => inf.RoomId == RoomId && inf.UserId == UserId).LastDateOfVisit;
            return _dal.Messages.GetAll().Where(m => m.RoomId == RoomId && m.DateOfSend > lastDateOfVisisit).Count();
        }

        public MessageDTO GetLastMessage(int RoomId)
        {
            if (RoomExists(RoomId))
            {
                throw new Exception("Room cannot be found!");
            }
            return Converter.ToMessageDTO(_dal.Messages.GetAll().LastOrDefault(m => m.RoomId == RoomId));
        }
        #endregion

        public BLLClass()
        {
            try
            {
                _dal = new DALClass();
            }
            catch (Exception)
            {
            }
        }
    }

    #region Data-Transfer-Object class or old name POCO = wrapper classe
    public class CountryDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        // collection of users
        public IEnumerable<UserDTO> Users { get; set; }
    }

    public class SexDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        // collection of users
        public IEnumerable<UserDTO> Users { get; set; }
    }

    public class GenreDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        // collection of rooms
        public IEnumerable<RoomDTO> Rooms { get; set; }
    }

    public class UserStatusDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        // collection of users
        public IEnumerable<UserDTO> Users { get; set; }
    }

    public class UserDTO
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public SexDTO Sex { get; set; }
        public int SexId { get; set; }
        //public string SexName { get; set; }

        public int? CountryId { get; set; }
        //public string CountryName { get; set; }
        public CountryDTO Country { get; set; }

        public Image Photo { get; set; }

        public int StatusId { get; set; }
        //public string StatusName { get; set; }
        public UserStatusDTO Status { get; set; }

        // collection of sended messages
        public IEnumerable<MessageDTO> Messages { get; set; }
        // collection of invited rooms
        public IEnumerable<RoomDTO> Rooms { get; set; }
        // collection of created rooms
        public IEnumerable<RoomDTO> CreatedRooms { get; set; }
        // collection of user visit infos
        public IEnumerable<VisitInfoDTO> VisitInfos { get; set; }
    }

    public class RoomDTO
    {
        public int Id { get; set; }

        public int CreatorId { get; set; }
        public UserDTO Creator { get; set; }

        public string Name { get; set; }

        public int GenreId { get; set; }
        //public string GenreName { get; set; }
        public GenreDTO Genre { get; set; }

        public string Description { get; set; }
        public Image Photo { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsPersonal { get; set; }

        // collection of sended messages
        public IEnumerable<MessageDTO> Messages { get; set; }
        // collection of invited users
        public IEnumerable<UserDTO> Users { get; set; }
        // collection of room visit infos
        public IEnumerable<VisitInfoDTO> VisitInfos { get; set; }
    }

    public class VisitInfoDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public UserDTO User { get; set; }

        public int RoomId { get; set; }
        public RoomDTO Room { get; set; }

        public DateTime DateOfLastVisit { get; set; }
    }

    public class MessageDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public UserDTO User { get; set; }

        public int RoomId { get; set; }
        public RoomDTO Room { get; set; }

        public string Text { get; set; }
        public DateTime DateOfSend { get; set; }
    }
    #endregion
}