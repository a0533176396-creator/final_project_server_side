using System;

namespace DTO.Models
{
    /// <summary>
    /// DTO עבור UserInsight - תובנות שה-AI למד על המשתמש
    /// משמש להעברת נתונים בין שכבות ללא חשיפת פרטי הטבלה
    /// </summary>
    public class UserInsightDTO
    {
  public int Id { get; set; }
        public int UserId { get; set; }
      public string InsightText { get; set; }
        public string? Category { get; set; }
        public int ConfidenceLevel { get; set; }
        public DateTime DiscoveredAt { get; set; }
   public DateTime? LastUpdatedAt { get; set; }
        public int ConfirmationCount { get; set; }
      public int NegationCount { get; set; }
    }
}
