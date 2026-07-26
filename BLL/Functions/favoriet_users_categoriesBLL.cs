using AutoMapper;
using DAL.Functions;
using DAL.Models;
using DTO.Mapper;
using DTO.Models;
using Microsoft.Extensions.Logging.Abstractions;

namespace BLL.Functions
{
    /// <summary>
    /// Business Logic Layer function class for favorite user categories-related operations.
    /// </summary>
    public static class favoriet_users_categoriesBLL
    {
        //-----------------------------------GetAllFavoriteUserCategories-----------------------------------
        public static List<favoriet_users_categoriesDTO> GetAllFavoriteUserCategories()
        {
            List<favoriet_users_categories> allData = favoriet_users_categoriesFunction.GetAllFavoriteUserCategories();
            return allData.Select(AppMapper.FavorietUserCategoryToDto).ToList();
        }

        //-----------------------------------GetFavoriteUserCategoryById-----------------------------------
        public static favoriet_users_categoriesDTO? GetFavoriteUserCategoryById(int id)
        {
            favoriet_users_categories? favorite = favoriet_users_categoriesFunction.GetFavoriteUserCategoryById(id);
            if (favorite == null)
                return null;
            return AppMapper.FavorietUserCategoryToDto(favorite);
        }

        //-----------------------------------AddNewFavoriteUserCategory-----------------------------------
        public static List<favoriet_users_categoriesDTO> AddNewFavoriteUserCategory(favoriet_users_categoriesDTO newFavorite)
        {
            favoriet_users_categories newFavoriteTBL = AppMapper.DtoToFavorietUserCategory(newFavorite);
            List<favoriet_users_categories> allData = favoriet_users_categoriesFunction.AddNewFavoriteUserCategory(newFavoriteTBL);
            return allData.Select(AppMapper.FavorietUserCategoryToDto).ToList();
        }

        //-----------------------------------UpdateFavoriteUserCategory-----------------------------------
        public static List<favoriet_users_categoriesDTO> UpdateFavoriteUserCategory(int idFavorite, favoriet_users_categoriesDTO newFavorite)
        {
            favoriet_users_categories newFavoriteTBL = AppMapper.DtoToFavorietUserCategory(newFavorite);
            List<favoriet_users_categories> allData = favoriet_users_categoriesFunction.UpdateFavoriteUserCategory(idFavorite, newFavoriteTBL);
            return allData.Select(AppMapper.FavorietUserCategoryToDto).ToList();
        }

        //-----------------------------------DeleteFavoriteUserCategory-----------------------------------
        public static List<favoriet_users_categoriesDTO> DeleteFavoriteUserCategory(int idFavorite)
        {
            List<favoriet_users_categories> allData = favoriet_users_categoriesFunction.DeleteFavoriteUserCategory(idFavorite);
            return allData.Select(AppMapper.FavorietUserCategoryToDto).ToList();
        }
        public static List<categoriesDTO> GetFavoriteCategoriesByUserId(int userId)
        {
            return favoriet_users_categoriesFunction.GetFavoriteCategoriesQueryByUserId(userId)
                                        .CategoryToDtoList()
                                        .ToList();
        }



    }
}
