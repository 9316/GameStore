using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GameStore.Domain.Entities
{
    public class Game
    {
        [HiddenInput(DisplayValue=false)]
        public int GameId { get; set; }

        [Display(Name="Name")]
        [Required(ErrorMessage="Enter name of the game")]
        public string Name { get; set; }
        
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage="Enter description for the game")]
        [Display(Name="Description")]
        public string Description { get; set; }

        [Display(Name="Category")]
        [Required(ErrorMessage = "Enter category for the game")]
        public string Category { get; set; }

        [Display(Name="Price ($)")]
        [Required]
        [Range(0.01,double.MaxValue, ErrorMessage = "Enter a positive value for the price")]
        public decimal Price { get; set; }

        public byte[] ImageData { get; set; }

        public string ImageMimeType { get; set; }
    }
}
