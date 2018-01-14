using DAL;
using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    static public class Converter
    {
        static public IEnumerable<T> ToIEnumerable<T>(IQueryable<T> value)
        {
            return value?.ToList();
        }

        static public Country ToCountry(CountryDTO value)
        {
            if (value == null)
                return null;

            var res = Mapper.Map<CountryDTO, Country>(value);
            //res.Users = value.Users.ToList().ConvertAll(Mapper.Map<UserDTO, User>);
            res.Users = value.Users?.ToList().ConvertAll(ToUser);

            return res;
        }
        static public CountryDTO ToCountryDTO(Country value)
        {
            if (value == null)
                return null;

            var res = Mapper.Map<Country, CountryDTO>(value);
            //res.Users = value.Users.ToList().ConvertAll(Mapper.Map<User, UserDTO>);
            res.Users = value.Users?.ToList().ConvertAll(ToUserDTO);

            return res;
        }

        static public Sex ToSex(SexDTO value)
        {
            if (value == null)
                return null;

            var res = Mapper.Map<SexDTO, Sex>(value);
            //res.Users = value.Users.ToList().ConvertAll(Mapper.Map<UserDTO, User>);
            res.Users = value.Users?.ToList().ConvertAll(ToUser);

            return res;
        }
        static public SexDTO ToSexDTO(Sex value)
        {
            if (value == null)
                return null;

            var res = Mapper.Map<Sex, SexDTO>(value);
            //res.Users = value.Users.ToList().ConvertAll(Mapper.Map<User, UserDTO>);
            res.Users = value.Users?.ToList().ConvertAll(ToUserDTO);

            return res;
        }

        static public UserStatus ToUserStatus(UserStatusDTO value)
        {
            if (value == null)
                return null;

            var res = Mapper.Map<UserStatusDTO, UserStatus>(value);
            //res.Users = value.Users.ToList().ConvertAll(Mapper.Map<UserDTO, User>);
            res.Users = value.Users?.ToList().ConvertAll(ToUser);

            return res;
        }
        static public UserStatusDTO ToUserStatusDTO(UserStatus value)
        {
            if (value == null)
                return null;

            var res = Mapper.Map<UserStatus, UserStatusDTO>(value);
            //res.Users = value.Users.ToList().ConvertAll(Mapper.Map<User, UserDTO>);
            res.Users = value.Users?.ToList().ConvertAll(ToUserDTO);

            return res;
        }

        static public Genre ToGenre(GenreDTO value)
        {
            if (value == null)
                return null;

            var res = Mapper.Map<GenreDTO, Genre>(value);
            //res.Rooms = value.Rooms.ToList().ConvertAll(Mapper.Map<RoomDTO, Room>);
            res.Rooms = value.Rooms?.ToList().ConvertAll(ToRoom);

            return res;
        }
        static public GenreDTO ToGenreDTO(Genre value)
        {
            if (value == null)
                return null;

            var res = Mapper.Map<Genre, GenreDTO>(value);
            //res.Rooms = value.Rooms.ToList().ConvertAll(Mapper.Map<Room, RoomDTO>);
            res.Rooms = value.Rooms?.ToList().ConvertAll(ToRoomDTO);

            return res;
        }

        static public User ToUser(UserDTO value)
        {
            if (value == null)
                return null;

            var res = Mapper.Map<UserDTO, User>(value);
            res.Photo = Util.ImageToByteArray(value.Photo);
            //res.CreatedRooms = value.CreatedRooms.ToList().ConvertAll(Mapper.Map<RoomDTO, Room>);
            //res.Messages = value.Messages.ToList().ConvertAll(Mapper.Map<MessageDTO, Message>);
            //res.Rooms = value.Rooms.ToList().ConvertAll(Mapper.Map<RoomDTO, Room>);
            //res.VisitInfos = value.VisitInfos.ToList().ConvertAll(Mapper.Map<VisitInfoDTO, VisitInfo>);
            res.CreatedRooms = value.CreatedRooms?.ToList().ConvertAll(ToRoom);
            res.Messages = value.Messages?.ToList().ConvertAll(ToMessage);
            res.Rooms = value.Rooms?.ToList().ConvertAll(ToRoom);
            res.VisitInfos = value.VisitInfos?.ToList().ConvertAll(ToVisiInfo);

            return res;
        }
        static public UserDTO ToUserDTO(User value)
        {
            if (value == null)
                return null;

            var res = Mapper.Map<User, UserDTO>(value);
            res.Photo = Util.ByteArrayToImage(value.Photo);
            //res.CreatedRooms = value.CreatedRooms.ToList().ConvertAll(Mapper.Map<Room, RoomDTO>);
            //res.Messages = value.Messages.ToList().ConvertAll(Mapper.Map<Message, MessageDTO>);
            //res.Rooms = value.Rooms.ToList().ConvertAll(Mapper.Map<Room, RoomDTO>);
            //res.VisitInfos = value.VisitInfos.ToList().ConvertAll(Mapper.Map<VisitInfo, VisitInfoDTO>);
            res.CreatedRooms = value.CreatedRooms?.ToList().ConvertAll(ToRoomDTO);
            res.Messages = value.Messages?.ToList().ConvertAll(ToMessageDTO);
            res.Rooms = value.Rooms?.ToList().ConvertAll(ToRoomDTO);
            res.VisitInfos = value.VisitInfos?.ToList().ConvertAll(ToVisiInfoDTO);

            return res;
        }

        static public Room ToRoom(RoomDTO value)
        {
            if (value == null)
                return null;

            var res = Mapper.Map<RoomDTO, Room>(value);
            res.Photo = Util.ImageToByteArray(value.Photo);
            //res.Messages = value.Messages.ToList().ConvertAll(Mapper.Map<MessageDTO, Message>);
            //res.Users = value.Users.ToList().ConvertAll(Mapper.Map<UserDTO, User>);
            //res.VisitInfos = value.VisitInfos.ToList().ConvertAll(Mapper.Map<VisitInfoDTO, VisitInfo>);
            res.Messages = value.Messages?.ToList().ConvertAll(ToMessage);
            res.Users = value.Users?.ToList().ConvertAll(ToUser);
            res.VisitInfos = value.VisitInfos?.ToList().ConvertAll(ToVisiInfo);

            return res;
        }
        static public RoomDTO ToRoomDTO(Room value)
        {
            if (value == null)
                return null;

            var res = Mapper.Map<Room, RoomDTO>(value);
            res.Photo = Util.ByteArrayToImage(value.Photo);
            //res.Messages = value.Messages.ToList().ConvertAll(Mapper.Map<Message, MessageDTO>);
            //res.Users = value.Users.ToList().ConvertAll(Mapper.Map<User, UserDTO>);
            //res.VisitInfos = value.VisitInfos.ToList().ConvertAll(Mapper.Map<VisitInfo, VisitInfoDTO>);
            res.Messages = value.Messages?.ToList().ConvertAll(ToMessageDTO);
            res.Users = value.Users?.ToList().ConvertAll(ToUserDTO);
            res.VisitInfos = value.VisitInfos?.ToList().ConvertAll(ToVisiInfoDTO);

            return res;
        }

        static public VisitInfo ToVisiInfo(VisitInfoDTO value)
        {
            if (value == null)
                return null;

            return Mapper.Map<VisitInfoDTO, VisitInfo>(value);            
        }
        static public VisitInfoDTO ToVisiInfoDTO(VisitInfo value)
        {
            if (value == null)
                return null;

            return Mapper.Map<VisitInfo, VisitInfoDTO>(value);
        }

        static public Message ToMessage(MessageDTO value)
        {
            if (value == null)
                return null;

            return Mapper.Map<MessageDTO, Message>(value);
        }
        static public MessageDTO ToMessageDTO(Message value)
        {
            if (value == null)
                return null;

            return Mapper.Map<Message, MessageDTO>(value);
        }
    }
}
