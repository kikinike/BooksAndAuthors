using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class BookAuthor
    {
        public int AuthorID { get; set; }
        public int BookID { get; set; }

        public int pk_author_book { get; set; }
    }
}
