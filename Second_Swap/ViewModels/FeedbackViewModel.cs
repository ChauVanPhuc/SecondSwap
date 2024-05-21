using System.ComponentModel.DataAnnotations;

namespace Second_Swap.ViewModels
{
    public class FeedbackViewModel
    {
        public int Id { get; set; }

        public string? Video { get; set; }

        [Required(ErrorMessage = "Please, Enter Comment")]
        [Display(Name = "Comment")]
        public string Comment { get; set; }

        [Required(ErrorMessage = "Please, Enter Quality")]
        [Display(Name = "Quality")]
        public int? Quality { get; set; }

        public int? OrderId { get; set; }

        public int? Seller { get; set; }

        public IFormFile FileVideo { get; set; }
    }
}
