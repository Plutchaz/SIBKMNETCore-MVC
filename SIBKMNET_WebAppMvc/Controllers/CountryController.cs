using Microsoft.AspNetCore.Mvc;
using SIBKMNET_WebAppMvc.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SIBKMNET_WebAppMvc.Controllers
{
    public class CountryController : Controller
    {

        SqlConnection sqlConnection;

        /*
         * Data Source - Server 
         * Initial Catalog - Database
         * User ID - Username
         * Password - Password
         * Connect Timeout
         */
        string connectionString = "Data Source=localhost;Initial Catalog=SIBKMNETConnection;User ID=sibkmnet;Password=1234567890;Connect Timeout=30;";

        public IActionResult Index()
        {
            string query = "SELECT * FROM Country";

            sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

            //buat masukkin data ke country, bikin listOfCountry dulu
            List<Country> Countries = new List<Country>();

            try
            {
                sqlConnection.Open();
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    if (sqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            //create model
                            Country country = new Country();
                            country.Id = Convert.ToInt32(sqlDataReader[0]);
                            country.Name = sqlDataReader[1].ToString();
                            Console.WriteLine(sqlDataReader[0] + " - " + sqlDataReader[1]);
                            Countries.Add(country);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No data rows");
                    }
                    sqlDataReader.Close();
                }
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return View(Countries);
        }

        //GET BY ID
        public IActionResult GetId(int? id) //tinggal dibikinin searchbarnya
        {
            //buat masukkin data ke country, bikin listOfCountry dulu
            List<Country> Countries = new List<Country>();

            string query = "SELECT * FROM Country WHERE id = @id";

            sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

            SqlParameter sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "@id";
            sqlParameter.Value = id;
            sqlCommand.Parameters.Add(sqlParameter);

            try
            {
                sqlConnection.Open();
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    if (sqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            //create model
                            Country country = new Country();
                            country.Id = Convert.ToInt32(sqlDataReader[0]);
                            country.Name = sqlDataReader[1].ToString();
                            Console.WriteLine(sqlDataReader[0] + " - " + sqlDataReader[1]);
                            Countries.Add(country);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No data rows");
                    }
                    sqlDataReader.Close();
                }
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return View(Countries);
        }

        //CREATE
        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST - ngirim data berupa data yang sesuai dengan di model
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Country country)
        {
            List<Country> Countries = new List<Country>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();

                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;

                SqlParameter sqlParameter = new SqlParameter();
                sqlParameter.ParameterName = "@name";
                sqlParameter.Value = country.Name;

                sqlCommand.Parameters.Add(sqlParameter);

                try
                {
                    //input yang bakal dijalanin seperti apa dalam command
                    sqlCommand.CommandText = "INSERT INTO Country(Name) VALUES (@name)";
                    sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                }
            }
            return View(Countries);
        }

        //UPDATE
        //GET
        //POST

        //DELETE
        //GET
        //POST
    }
}
