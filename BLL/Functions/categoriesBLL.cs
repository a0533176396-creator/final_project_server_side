using AutoMapper;
using DAL.Functions;
using DAL.Models;
using DTO.Mapper;
using DTO.Models;
using Microsoft.Extensions.Logging.Abstractions;

namespace BLL.Functions
{
    /// <summary>
    /// Business Logic Layer function class for category-related operations.
    /// </summary>
    public static class categoriesBLL
    {
        //-----------------------------------GetAllCategories-----------------------------------
        public static List<categoriesDTO> GetAllCategories()
        {
            List<categories> allData = categoriesFunction.GetAllCategories();
            return allData.Select(AppMapper.CategoryToDto).ToList();
        }

        //-----------------------------------GetCategoryById-----------------------------------
        public static categoriesDTO? GetCategoryById(int id)
        {
            categories? category = categoriesFunction.GetCategoryById(id);
            if (category == null)
            return null;
            return AppMapper.CategoryToDto(category);
        }

        //-----------------------------------AddNewCategory-----------------------------------
        public static List<categoriesDTO> AddNewCategory(categoriesDTO newCategory)
        {
            categories newCategoryTBL = AppMapper.DtoToCategory(newCategory);
            List<categories> allData = categoriesFunction.AddNewCategory(newCategoryTBL);
            return allData.Select(AppMapper.CategoryToDto).ToList();
        }

        //-----------------------------------UpdateCategory-----------------------------------
        public static List<categoriesDTO> UpdateCategory(int idCategory, categoriesDTO newCategory)
        {
            categories newCategoryTBL = AppMapper.DtoToCategory(newCategory);
            List<categories> allData = categoriesFunction.UpdateCategory(idCategory, newCategoryTBL);
            return allData.Select(AppMapper.CategoryToDto).ToList();
        }

        //-----------------------------------DeleteCategory-----------------------------------
        public static List<categoriesDTO> DeleteCategory(int idCategory)
        {
            List<categories> allData = categoriesFunction.DeleteCategory(idCategory);
            return allData.Select(AppMapper.CategoryToDto).ToList();
        }
    }
}
