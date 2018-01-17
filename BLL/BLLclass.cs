﻿using AutoMapper;
using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public enum LoginResult { Succes, InvalidLogin, InvalidPassword }
    public enum UserStatusType { Online, DoNotDisturb, Offline }

    public class BLLClass
    {
        private readonly DAL.DALClass _dal;
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

        private void UpdateUser()
        {
            _dal.Users.UpdateUser(AuthenticatedUser);
        }

        private UserStatus GetUserStatus(UserStatusType status)
        {
            var statuses = _dal.UserStatuses.GetAll();

            switch (status)
            {
                case UserStatusType.Online:
                    return statuses.First(s => s.Name == "Online");
                case UserStatusType.DoNotDisturb:
                    return statuses.First(s => s.Name == "Do not disturb");
                case UserStatusType.Offline:
                    return statuses.First(s => s.Name == "Offline");
                default:
                    throw new Exception("Status not found");
            }
        }

        private void SetStatus(UserStatusType status)
        {
            AuthenticatedUser.StatusId = GetUserStatus(status).Id;
            UpdateUser();
        }

        private bool IsLoginAlreadyExist(string login)
        {
            return _dal.Users.GetAll().FirstOrDefault(u => u.Login == login) != null;
        }
        private bool IsValidPassword(string login, string password)
        {
            return _dal.Users.GetAll().First(u => u.Login == login).Password == password;
        }
        private User GetUserByLoginAndPassword(string login, string password)
        {
            return _dal.Users.GetAll().First(u => u.Login == login && u.Password == password);
        }

        public LoginResult Login(string login, string password)
        {
            try
            {
                if (!IsLoginAlreadyExist(login))
                    return LoginResult.InvalidLogin;

                if (!IsValidPassword(login, password))
                    return LoginResult.InvalidPassword;

                AuthenticatedUser = GetUserByLoginAndPassword(login, password);
                SetStatus(UserStatusType.Online);

                return LoginResult.Succes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool SendMessage(int roomId, string text)
        {
            if (AuthenticatedUser == null)
                return false;

            _dal.Messages.Add(new Message()
            {
                UserId = AuthenticatedUser.Id,
                RoomId = roomId,
                Text = text,
                DateOfSend = DateTime.Now
            });

            return true;
        }

        public void Logout()
        {
            try
            {
                SetStatus(UserStatusType.Offline);
                AuthenticatedUser = null;
            }
            catch (Exception)
            {
                throw;
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