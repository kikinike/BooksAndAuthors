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
    public class AuthorController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string querry = @"
                select authorId as ""authorID""

                        authorName as ""authorName""
                from Author
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
        public JsonResult Post(Models.Author auth)
        {
            string querry = @"
                insert into author(authorName)
                values (@authorName)
             
                
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("AuthorAppCon");
            NpgsqlDataReader reader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(querry, myCon))
                {

                    command.Parameters.AddWithValue("@authorName", auth.AuthorName);
                   
                    reader = command.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    myCon.Close();

                }


                return new JsonResult("Added successfully");

            }
        }
        [HttpPut]
        public JsonResult Put(Models.Author auth)
        {
            string querry = @"
                  update Author
                set     authorName = @authorName                   
               where  authorID = @authorID
                
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("AuthorAppCon");
            NpgsqlDataReader reader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(querry, myCon))
                {

                    command.Parameters.AddWithValue("@authorName", auth.AuthorName);
                    
                    command.Parameters.AddWithValue("@authorID", auth.AuthorID);
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
                  delete from Author
                where  authorID = @authorID
                
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("AuthorAppCon");
            NpgsqlDataReader reader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(querry, myCon))
                {

                    command.Parameters.AddWithValue("@authorID", id);

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
