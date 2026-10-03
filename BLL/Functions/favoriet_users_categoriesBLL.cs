using AutoMapper;
using DAL.Functions;
using DAL.Models;
using DTO.Mapper;
using DTO.Models;

namespace BLL.Functions
{
    public static class favoriet_users_categoriesBLL
    {
        public static List<favoriet_users_categoriesDTO> GetAllFavoriteUserCategories()
        {
            List<favoriet_users_categories> allData = favoriet_users_categoriesFunction.GetAllFavoriteUserCategories();
            return allData.Select(AppMapper.FavorietUserCategoryToDto).ToList();
        }

        public static favoriet_users_categoriesDTO? GetFavoriteUserCategoryById(int id)
        {
            favoriet_users_categories? favorite = favoriet_users_categoriesFunction.GetFavoriteUserCategoryById(id);
            if (favorite == null)
                return null;
            return AppMapper.FavorietUserCategoryToDto(favorite);
        }

        public static List<favoriet_users_categoriesDTO> AddNewFavoriteUserCategory(favoriet_users_categoriesDTO newFavorite)
        {
            favoriet_users_categories newFavoriteTBL = AppMapper.DtoToFavorietUserCategory(newFavorite);
            List<favoriet_users_categories> allData = favoriet_users_categoriesFunction.AddNewFavoriteUserCategory(newFavoriteTBL);
            return allData.Select(AppMapper.FavorietUserCategoryToDto).ToList();
        }

        public static List<favoriet_users_categoriesDTO> UpdateFavoriteUserCategory(int idFavorite, favoriet_users_categoriesDTO newFavorite)
        {
            favoriet_users_categories newFavoriteTBL = AppMapper.DtoToFavorietUserCategory(newFavorite);
            List<favoriet_users_categories> allData = favoriet_users_categoriesFunction.UpdateFavoriteUserCategory(idFavorite, newFavoriteTBL);
            return allData.Select(AppMapper.FavorietUserCategoryToDto).ToList();
        }

        public static List<favoriet_users_categoriesDTO> DeleteFavoriteUserCategory(int idFavorite)
        {
            List<favoriet_users_categories> allData = favoriet_users_categoriesFunction.DeleteFavoriteUserCategory(idFavorite);
            return allData.Select(AppMapper.FavorietUserCategoryToDto).ToList();
        }

        public static List<categoriesDTO> GetFavoriteCategoriesByUserId(int userId)
        {
            // קבלת הרשימה שכבר עברה Materialize ב-DAL והמרתה ל-DTO
            List<categories> categoriesList = favoriet_users_categoriesFunction.GetFavoriteCategoriesByUserId(userId);

            return categoriesList?
                .Select(c => AppMapper.CategoryToDto(c))
                .ToList() ?? new List<categoriesDTO>();
        }

        //--------------------------------הוספת קטגוריה חדשה ושיוכה למשתמש----------------------------------
        public static List<categoriesDTO> CreateAndLinkNewFavoriteCategory(CreateAndLinkFavoriteCategoryDTO request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.CategoryName))
                throw new ArgumentException("Category name is required.");

            var newCatDto = new categoriesDTO
            {
                Name = request.CategoryName.Trim(),
                father_id = request.FatherId,
                Color = request.Color ?? string.Empty
            };

            // יצירת הקטגוריה החדשה במערכת (כולל הגרלת צבע אם צריכה)
            var allCategories = categoriesBLL.AddNewCategory(newCatDto);
            var createdCategory = allCategories
                .Where(c => c.Name == newCatDto.Name && c.father_id == newCatDto.father_id)
                .OrderByDescending(c => c.Id)
                .FirstOrDefault();

            if (createdCategory == null)
                throw new InvalidOperationException("Failed to create new category.");

            // שיוך למשתמש
            AddNewFavoriteUserCategory(new favoriet_users_categoriesDTO
            {
                user_id = request.UserId,
                category_id = createdCategory.Id
            });

            return GetFavoriteCategoriesByUserId(request.UserId);
        }
    }
}