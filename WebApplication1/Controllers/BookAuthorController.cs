using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookAuthorController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public BookAuthorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

   

       [HttpGet]
        public JsonResult Get(String surename)
        {
            string querry =
            @"select Author.authorName as ""Author"", 
            count(*) AS ""Number of written books""
            from Author
            Join author_book ON Author.authorID = author_book.authorID
            JOIN Book ON Book.bookID = author_book.bookID
            where Author.authorName = 'Voronin'
            GROUP BY  Author.authorName"
           ;

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("AuthorAppCon");
            NpgsqlDataReader reader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(querry, myCon))
                {
                   // command.Parameters.AddWithValue("@authorName",id);
                    reader = command.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    myCon.Close();

                }


                return new JsonResult(table);

            }
        }

        [HttpPost]
        public JsonResult Post(Models.Book book)
        {
            string querry = @"
                insert into author_book(bookID,authorID)
                values (@bookID, @authorID)
             
                
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("AuthorAppCon");
            NpgsqlDataReader reader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(querry, myCon))
                {

                    command.Parameters.AddWithValue("@BookName", book.BookName);

                    reader = command.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    myCon.Close();

                }


                return new JsonResult("Added successfully");

            }
        }
        [HttpPut]
        public JsonResult Put(Models.BookAuthor book)
        {
            string querry = @"
                  update author_book
                set     bookID = @bookID ,
                           authorID = @authorID
               where  pk_author_book = @pk_author_book
                
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("AuthorAppCon");
            NpgsqlDataReader reader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(querry, myCon))
                {

                    command.Parameters.AddWithValue("@authorID", book.AuthorID);

                    command.Parameters.AddWithValue("@bookID", book.BookID);
                    reader = command.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    myCon.Close();

                }


                return new JsonResult("Updated successfully");

            }
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string querry = @"
                  delete from author_book
                where  pk_author_book = @pk_author_book
                
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("AuthorAppCon");
            NpgsqlDataReader reader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(querry, myCon))
                {

                    command.Parameters.AddWithValue("@pk_author_book", id);

                    reader = command.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    myCon.Close();

                }


                return new JsonResult("Deleted successfully");

            }
        }
    }
}
