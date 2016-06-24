using System.Configuration;
using ImageUploader.Models;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System;

namespace ImageUploader.DB
{
    public class DataAccess
    {
        public static string GetConnectionString()
        {           
            return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public void InsertPerson(Person person) 
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                if (person.ImageData != null)
                {
                    string query = "insert into Persons(Name,Surname,ImageSize,ImageName,ImageData) " +
                                   "values(@Name,@Surname,@ImageSize,@ImageName,@ImageData)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = person.Name;
                        cmd.Parameters.Add("@Surname", SqlDbType.NVarChar).Value = person.Surname;
                        cmd.Parameters.Add("@ImageSize", SqlDbType.Int).Value = person.ImageSize;
                        cmd.Parameters.Add("@ImageName", SqlDbType.NVarChar).Value = person.ImageName;
                        cmd.Parameters.Add("@ImageData", SqlDbType.Binary).Value = person.ImageData;
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                    else 
                    {
                        string query = "insert into Persons(Name,Surname,ImageSize,ImageName) " +
                                  "values(@Name,@Surname,@ImageSize,@ImageName)";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = person.Name;
                            cmd.Parameters.Add("@Surname", SqlDbType.NVarChar).Value = person.Surname;
                            cmd.Parameters.Add("@ImageSize", SqlDbType.Int).Value = person.ImageSize;
                            cmd.Parameters.Add("@ImageName", SqlDbType.NVarChar).Value = person.ImageName;                        
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
            }
        }

        public List<Person> GetPersonList()
        {
            List<Person> personsList = new List<Person>();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                string query = "select p.RecordID,p.Name,p.Surname,p.ImageSize,p.ImageName,p.ImageData from Persons p";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Person person = new Person();
                                person.RecordID = reader.GetInt32(0);
                                person.Name = reader.GetString(1);
                                person.Surname = reader.GetString(2);
                                person.ImageSize = reader.GetInt32(3);
                                person.ImageName = reader.GetString(4);
                                if (reader.GetValue(5) != DBNull.Value)
                                {
                                    person.ImageData = (byte[])reader.GetValue(5);
                                }                                                                                                          
                                personsList.Add(person);
                            }
                        }
                    }
                }
            }
            return personsList;
        }

        public void UpdatePerson(Person person)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                if (person.ImageData != null)
                {
                    string query = "update Persons set Name=@Name,Surname=@Surname,ImageSize=@ImageSize,ImageName=@ImageName,ImageData=@ImageData where RecordID=@RecordID;";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = person.Name;
                        cmd.Parameters.Add("@Surname", SqlDbType.NVarChar).Value = person.Surname;
                        cmd.Parameters.Add("@ImageSize", SqlDbType.Int).Value = person.ImageSize;
                        cmd.Parameters.Add("@ImageName", SqlDbType.NVarChar).Value = person.ImageName;
                        cmd.Parameters.Add("@ImageData", SqlDbType.Binary).Value = person.ImageData;
                        cmd.Parameters.Add("@RecordID", SqlDbType.Int).Value = person.RecordID;
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                    else 
                    {
                        string query = "update Persons set Name=@Name,Surname=@Surname,ImageSize=@ImageSize,ImageName=@ImageName where RecordID=@RecordID;";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = person.Name;
                            cmd.Parameters.Add("@Surname", SqlDbType.NVarChar).Value = person.Surname;
                            cmd.Parameters.Add("@ImageSize", SqlDbType.Int).Value = person.ImageSize;
                            cmd.Parameters.Add("@ImageName", SqlDbType.NVarChar).Value = person.ImageName;                         
                            cmd.Parameters.Add("@RecordID", SqlDbType.Int).Value = person.RecordID;
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
            }
        }

        public void DeletePerson(int itemID)
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                string query = "delete from Persons where RecordID=@RecordID;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {               
                    cmd.Parameters.Add("@RecordID", SqlDbType.Int).Value = itemID;
                    con.Open();
                    cmd.ExecuteNonQuery();
                }

            }
        }

        public Person GetSinglePerson(int itemID)
        {
            Person item = new Person();
            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                string query = "select p.RecordID,p.Name,p.Surname,p.ImageSize,p.ImageName,p.ImageData from Persons p where p.RecordID=@itemID;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@itemID", SqlDbType.Int).Value = itemID;
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {                   
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                item.RecordID = reader.GetInt32(0);
                                item.Name = reader.GetString(1);
                                item.Surname = reader.GetString(2);
                                item.ImageSize = reader.GetInt32(3);
                                item.ImageName = reader.GetString(4);
                                if (reader.GetValue(5) != DBNull.Value)
                                {
                                    item.ImageData = (byte[])reader.GetValue(5);
                                }
                                
                            }
                        }
                    }
                }
            }
            return item;
        }

    }
}