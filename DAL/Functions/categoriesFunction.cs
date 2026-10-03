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
                else
                {
                    // הגדרת צבע ייחודי לקטגוריית אב
                    var existingColors = DB.Categories
                        .Where(cat => !string.IsNullOrEmpty(cat.Color))
                        .Select(cat => cat.Color.ToUpper())
                        .ToHashSet();

                    // פלטת צבעים חזקים ובולטים (ללא צבעים בהירים/לבן/ורוד בהיר)
                    string[] vibrantPalette = new string[]
                    {
                        "#E53935", "#D81B60", "#8E24AA", "#5E35B1", "#3949AB",
                        "#1E88E5", "#039BE5", "#00ACC1", "#00897B", "#43A047",
                        "#7CB342", "#C0CA33", "#FDD835", "#FB8C00", "#F4511E",
                        "#6D4C41", "#546E7A", "#2E7D32", "#1565C0", "#6A1B9A",
                        "#AD1457", "#C62828", "#00695C", "#283593", "#EF6C00"
                    };

                    // חיוץ צבע מהפלטה שאינו קיים עדיין ב-DB
                    string chosenColor = vibrantPalette.FirstOrDefault(color => !existingColors.Contains(color.ToUpper()));

                    if (chosenColor == null)
                    {
                        // אם כל הצבעים בפלטה כבר תפוסים, נגריל צבע חזק אקראי שאינו בשימוש
                        var random = Random.Shared;
                        do
                        {
                            // ייצור צבעי RGB עם ערכים שאינם גבוהים מדי למניעת צבעים בהירים/חלשים
                            int r = random.Next(30, 200);
                            int g = random.Next(30, 200);
                            int b = random.Next(30, 200);
                            chosenColor = $"#{r:X2}{g:X2}{b:X2}";
                        } while (existingColors.Contains(chosenColor.ToUpper()));
                    }

                    c.Color = chosenColor;
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
