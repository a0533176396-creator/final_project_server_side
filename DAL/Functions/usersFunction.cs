using DAL.Data;
using DAL.Models;

namespace DAL.Functions
{
    /// <summary>
    /// Function class for user-related operations.
    /// </summary>
    public static class usersFunction
    {
        static AppDbContext DB = new AppDbContext();

        //--------------------------קבלת כל המשתמשים----------------------------
        public static List<users> GetAllUsers()
        {
            return DB.Users.ToList();
        }

        //--------------------------קבלת משתמש על פי קוד המשתמש----------------------------
        public static users? GetUserById(int id)
        {
            users User = DB.Users.FirstOrDefault(p => p.Id == id)!;
            if (User != null)
                return User;
            return null;
        }

        //--------------------------------הוספת משתמש----------------------------------
        public static List<users> AddNewUser(users u)
        {
            DB.Users.Add(u);
            DB.SaveChanges();
            return GetAllUsers();
        }

        //----------------------------------עדכון משתמש----------------------------------
        public static List<users> UpdateUser(int idUser, users newUser)
        {
            users UserToUpdate = DB.Users.FirstOrDefault(p => p.Id == idUser)!;
            if (UserToUpdate != null)
            {
                UserToUpdate.First_name = newUser.First_name;
                UserToUpdate.Last_name = newUser.Last_name;
                UserToUpdate.Email = newUser.Email;
                UserToUpdate.Password = newUser.Password;
                UserToUpdate.Wont_help = newUser.Wont_help;
                DB.SaveChanges();
            }
            return GetAllUsers();
        }

        //--------------------------------מחיקת משתמש----------------------------------
        public static List<users> DeleteUser(int idUser)
        {
            users UserToDelete = DB.Users.FirstOrDefault(p => p.Id == idUser)!;
            if (UserToDelete != null)
            {
                DB.Users.Remove(UserToDelete);
                DB.SaveChanges();
            }
            return GetAllUsers();
        }
    }
}
