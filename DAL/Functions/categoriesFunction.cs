using DAL.Data;
using DAL.Models;

namespace DAL.Functions
{
    /// <summary>
    /// Function class for category-related operations.
    /// </summary>
    public static class categoriesFunction
    {
        //--------------------------קבלת כל הקטגוריות----------------------------
        public static List<categories> GetAllCategories()
        {
            using (AppDbContext DB = new AppDbContext())
            {
                return DB.Categories.ToList();
            }
        }

        //--------------------------קבלת קטגוריה על פי קוד הקטגוריה----------------------------
        public static categories? GetCategoryById(int id)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                categories Category = DB.Categories.FirstOrDefault(p => p.Id == id)!;
                if (Category != null)
                    return Category;
                return null;
            }
        }

        //--------------------------------הוספת קטגוריה----------------------------------
        public static List<categories> AddNewCategory(categories c)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                if (c.father_id != null)
                {
                    c.Color = GetCategoryById((int)c.father_id)!.Color;
                }
                DB.Categories.Add(c);
                DB.SaveChanges();
                return GetAllCategories();
            }
        }

        //----------------------------------עדכון קטגוריה----------------------------------
        public static List<categories> UpdateCategory(int idCategory, categories newCategory)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                categories CategoryToUpdate = DB.Categories.FirstOrDefault(p => p.Id == idCategory)!;
                if (CategoryToUpdate != null)
                {
                    CategoryToUpdate.Name = newCategory.Name;
                    CategoryToUpdate.Color = newCategory.Color;
                    CategoryToUpdate.father_id = newCategory.father_id;
                    DB.SaveChanges();
                }
                return GetAllCategories();
            }
        }

        //--------------------------------מחיקת קטגוריה----------------------------------
        public static List<categories> DeleteCategory(int idCategory)
        {
            using (AppDbContext DB = new AppDbContext())
            {
                categories CategoryToDelete = DB.Categories.FirstOrDefault(p => p.Id == idCategory)!;
                if (CategoryToDelete != null)
                {
                    DB.Categories.Remove(CategoryToDelete);
                    DB.SaveChanges();
                }
                return GetAllCategories();
            }
        }
    }
}
