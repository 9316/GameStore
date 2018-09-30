using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.WEB.Models
{
    public class PagingInfo
    {
        public int TotalItems { get; set; } // საქონლის რაოდენობა

        public int ItemsPerPage { get; set; } // საქონლის რაოდენობა ერთ გვერდზე

        public int CurrentPage { get; set; } // მიმდინრე გვერდის ნომერი

        public int TotalPages // გევრდების საერთო რაოდენობა 
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
        }
    }
}