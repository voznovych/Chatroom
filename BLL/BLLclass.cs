﻿using AutoMapper;
using DAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using BLL.DBO_Enteties;
using DAL.Interfaces;

namespace BLL
{
    public enum RegistrationResult
    {
        Success,
        LoginIsAlreadyExist,
        LoginIsInvalidOrEmpty,
        NameIsInvalidOrEmpty,
        SurnameIsInvalidOrEmpty,
        PasswordIsInvalidOrEmpty,
        BirthDateIsInvalidOrNotSelected,
        SexIsInvalidOrNotSelected,
    }
    
    public class BLLClass
    {
        const int HUNDRED_YEARS_BEFORE = -100;
        private const string STATUS_ONLINE = "Online";
        private const string STATUS_OFFLINE = "Offline";
        private const string STATUS_DND = "Do not disturb";

        private readonly IDAL _dal;
        private string LoginPattern { get; set; } = @"^[a-zA-Z]\w{5,19}$";
        private string PasswordPattern { get; set; } = @"\w{6,25}";

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

        public RegistrationResult SignUp(SignUpUserData data)
        {
            if (!IsValidLogin(data.Login))
            {
                return RegistrationResult.LoginIsInvalidOrEmpty;
            }
            else if (IsLoginExist(data.Login))
            {
                return RegistrationResult.LoginIsAlreadyExist;
            }
            else if (!IsValidPassword(data.Password))
            {
                return RegistrationResult.PasswordIsInvalidOrEmpty;
            }
            else if (!IsValidName(data.Name))
            {
                return RegistrationResult.NameIsInvalidOrEmpty;
            }
            else if (!IsValidName(data.Surname))
            {
                return RegistrationResult.SurnameIsInvalidOrEmpty;
            }
            else if(!data.BirthDate.HasValue || !IsValidBirthDate(data.BirthDate.Value))
            {
                return RegistrationResult.BirthDateIsInvalidOrNotSelected;
            }
            else if(!IsValidSex(data.Sex))
            {
                return RegistrationResult.SexIsInvalidOrNotSelected;
            }

            User registeredUser = CreateUser(data.Login, data.Password, data.Name, data.Surname, data.BirthDate.Value, 
                                                data.Sex.Id, data.Country?.Id);
            AddUser(registeredUser);

            return RegistrationResult.Success;
        }
        private bool IsValidLogin(string login)
        {
            return !String.IsNullOrEmpty(login) && Regex.IsMatch(login, LoginPattern);
        }
        private bool IsValidName(string name)
        {
            return !String.IsNullOrEmpty(name);
        }
        private bool IsValidSurname(string surname)
        {
            return !String.IsNullOrEmpty(surname);
        }
        private bool IsValidPassword(string password)
        {
            return !String.IsNullOrEmpty(password) && Regex.IsMatch(password, PasswordPattern);
        }
        private bool IsValidBirthDate(DateTime birthDate)
        {
            return birthDate < DateTime.UtcNow && birthDate > DateTime.UtcNow.AddYears(HUNDRED_YEARS_BEFORE);
        }
        private bool IsValidSex(SexDTO sex)
        {
            if (sex == null)
            {
                return false;
            }

            return IsSexExist(sex.Id);
        }
        private bool IsSexExist(int id)
        {
            return _dal.Sexes.GetAll().FirstOrDefault(s => s.Id == id) != null;

        }
        private bool IsLoginExist(string login)
        {
            return _dal.Users.GetAll().FirstOrDefault(u => u.Login == login) != null;
        }

        private User CreateUser(string login, string password, string name, string surname,
                                    DateTime? birthDate, int sexId, int? countryId)
        {
            User user = new User()
            {
                Login = login,
                Name = name,
                Surname = surname,
                SexId = sexId,
                DateOfBirth = birthDate,
            };
            if(countryId.HasValue)
            {
                user.CountryId = countryId.Value;
            }
            user.StatusId = GetUserStatusId(STATUS_OFFLINE);
            user.Password = Util.GetHashString(password);

            return user;
        }
        private int GetUserStatusId(string status)
        {
            return _dal.UserStatuses.GetAll().FirstOrDefault(s => s.Name == status).Id;
        }
        private void AddUser(User user)
        {
            _dal.Users.AddUser(user);
        }
        public IEnumerable<SexDTO> GetAllSexes()
        {
            return _dal.Sexes.GetAll().ToList().ConvertAll(Converter.ToSexDTO);
        }
        public IEnumerable<CountryDTO> GetAllCountries()
        {
            return _dal.Countries.GetAll().OrderBy(c=>c.Name).ToList().ConvertAll(Converter.ToCountryDTO);
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