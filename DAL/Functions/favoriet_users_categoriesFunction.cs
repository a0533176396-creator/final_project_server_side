using DAL.Data;
using DAL.Models;

namespace DAL.Functions
{
    /// <summary>
    /// Function class for favorite user categories-related operations.
    /// </summary>
    public static class favoriet_users_categoriesFunction
    {
        //--------------------------קבלת כל הקטגוריות המועדפות----------------------------
        public static List<favoriet_users_categories> GetAllFavoriteUserCategories()
        {
            using (AppDbContext DB = new AppDbContext())
            {
                return DB.FavoriteUserCategories.ToList();
            }
        }

        //--------------------------קבלת קטגוריה מועדפת על פי קוד----------------------------
        public static favoriet_users_categories? GetFavoriteUserCategoryById(int id)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                favoriet_users_categories FavoriteUserCategory = DB.FavoriteUserCategories.FirstOrDefault(p => p.Id == id)!;
                if (FavoriteUserCategory != null)
                    return FavoriteUserCategory;
                return null;
            }
        }

        //--------------------------------הוספת קטגוריה מועדפת----------------------------------
        public static List<favoriet_users_categories> AddNewFavoriteUserCategory(favoriet_users_categories f)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                DB.FavoriteUserCategories.Add(f);
                DB.SaveChanges();
                return GetAllFavoriteUserCategories();
            }
        }

        //----------------------------------עדכון קטגוריה מועדפת----------------------------------
        public static List<favoriet_users_categories> UpdateFavoriteUserCategory(int idFavorite, favoriet_users_categories newFavorite)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                favoriet_users_categories FavoriteToUpdate = DB.FavoriteUserCategories.FirstOrDefault(p => p.Id == idFavorite)!;
                if (FavoriteToUpdate != null)
                {
                    FavoriteToUpdate.user_id = newFavorite.user_id;
                    FavoriteToUpdate.category_id = newFavorite.category_id;
                    DB.SaveChanges();
                }
                return GetAllFavoriteUserCategories();
            }
        }

        //--------------------------------מחיקת קטגוריה מועדפת----------------------------------
        public static List<favoriet_users_categories> DeleteFavoriteUserCategory(int idFavorite)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                favoriet_users_categories FavoriteToDelete = DB.FavoriteUserCategories.FirstOrDefault(p => p.Id == idFavorite)!;
                if (FavoriteToDelete != null)
                {
                    DB.FavoriteUserCategories.Remove(FavoriteToDelete);
                    DB.SaveChanges();
                }
                return GetAllFavoriteUserCategories();
            }
        }

        // בתוך פרויקט DAL
        public static IQueryable<categories> GetFavoriteCategoriesQueryByUserId(int userId)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                return DB.FavoriteUserCategories
                    .Where(fuc => fuc.user_id == userId)
                    .Select(fuc => fuc.Category).AsQueryable();
            }
        }
    }
}
