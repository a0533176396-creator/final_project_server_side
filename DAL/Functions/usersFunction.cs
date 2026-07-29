using DAL.Data;
using DAL.Models;

namespace DAL.Functions
{
    /// <summary>
    /// Function class for user-related operations.
    /// </summary>
    public static class usersFunction
    {
        //--------------------------קבלת כל המשתמשים----------------------------
        public static List<Users> GetAllUsers()
        {
            using (AppDbContext DB = new AppDbContext())
            {
                return DB.Users.ToList();
            }
        }

        //--------------------------קבלת משתמש על פי קוד המשתמש----------------------------
        public static Users? GetUserById(string sub)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                Users User = DB.Users.FirstOrDefault(p => p.sub == sub)!;
                if (User != null)
                    return User;
                return null;
            }
        }

        //--------------------------------הוספת משתמש----------------------------------
        public static List<Users> AddNewUser(Users u)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                DB.Users.Add(u);
                DB.SaveChanges();
                return GetAllUsers();
            }
        }

        //----------------------------------עדכון משתמש----------------------------------
        public static List<Users> UpdateUser(int idUser, Users newUser)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                Users UserToUpdate = DB.Users.FirstOrDefault(p => p.Id == idUser)!;
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
        }

        //--------------------------------מחיקת משתמש----------------------------------
        public static List<Users> DeleteUser(int idUser)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                Users UserToDelete = DB.Users.FirstOrDefault(p => p.Id == idUser)!;
                if (UserToDelete != null)
                {
                    DB.Users.Remove(UserToDelete);
                    DB.SaveChanges();
                }
                return GetAllUsers();
            }
        }

        //-------------------------------- Validate User Full Name and Password ----------------------------------
        public static bool ValidateUserFullNameAndPassword(string firstName, string lastName, string password)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                // Search for a user with matching first name and last name
                Users? User = DB.Users.FirstOrDefault(p =>
                  p.First_name == firstName &&
                 p.Last_name == lastName);

                // If user found and password matches, return true
                if (User != null && User.Password == password)
                    return true;

                return false;
            }
        }
    }
}
