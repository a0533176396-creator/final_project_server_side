using System;

namespace DTO.Models
{
    public class CreateAndLinkFavoriteCategoryDTO
    {
        public int UserId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int? FatherId { get; set; }
        public string? Color { get; set; }
    }
}
