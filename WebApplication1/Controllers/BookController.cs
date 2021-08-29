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
    public class BookController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public BookController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string querry = @"
                select BookID as ""BookID"",
                        BookName as ""BookName""
                      
                from Book
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("AuthorAppCon");
            NpgsqlDataReader reader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(querry, myCon))
                {

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
                insert into Book(BookName)
                values (@BookName)
             
                
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
        public JsonResult Put(Models.Book book)
        {
            string querry = @"
                  update Book
                set     BookName = @BookName                    
               where  BookID = @BookID
                
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

                    command.Parameters.AddWithValue("@BookID", book.BookID);
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
                  delete from Book
                where  BookID = @BookID
                
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("AuthorAppCon");
            NpgsqlDataReader reader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(querry, myCon))
                {

                    command.Parameters.AddWithValue("@BookID", id);

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
