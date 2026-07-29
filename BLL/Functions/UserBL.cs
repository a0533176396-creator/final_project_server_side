using AutoMapper;
using DAL.Functions;
using DAL.Models;
using DTO.Mapper;
using DTO.Models;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Generic;

namespace BLL.Functions
{
    /// <summary>
    /// Business Logic Layer class for User operations.
    /// </summary>
    public static class UsersBLL
    {
        private static readonly IMapper _Mapper;



        //-----------------------------------GetAllUsers-----------------------------------
        public static List<usersDTO> GetAllUsers()
        {
            List<Users> allData = usersFunction.GetAllUsers();
            return allData.Select(AppMapper.UserToDto).ToList();
        }

        //-----------------------------------GetUserById-----------------------------------
        public static usersDTO? GetUserById(string sub)
        {
            Users? user = usersFunction.GetUserById(sub);
            if (user == null)
                return null;
            return AppMapper.UserToDto(user);
        }

        //-----------------------------------AddNewUser-----------------------------------
        public static List<usersDTO> AddNewUser(usersDTO newUser)
        {
            Users newUserTBL = AppMapper.DtoToUser(newUser);
            List<Users> allData = usersFunction.AddNewUser(newUserTBL);
            return allData.Select(AppMapper.UserToDto).ToList();

        }

        //-----------------------------------UpdateUser-----------------------------------
        public static List<usersDTO> UpdateUser(int idUser, usersDTO newUser)
        {
            Users newUserTBL = AppMapper.DtoToUser(newUser);
            List<Users> allData = usersFunction.UpdateUser(idUser, newUserTBL);
            return allData.Select(AppMapper.UserToDto).ToList();
        }

        //-----------------------------------DeleteUser-----------------------------------
        public static List<usersDTO> DeleteUser(int idUser)
        {
            List<Users> allData = usersFunction.DeleteUser(idUser);
            return allData.Select(AppMapper.UserToDto).ToList();

        }
        //-----------------------------------ValidateUserFullNameAndPassword-----------------------------------
        public static bool ValidateUserFullNameAndPassword(string firstName, string lastName, string password)
        {
            return usersFunction.ValidateUserFullNameAndPassword(firstName, lastName, password);
        }
    }
}
