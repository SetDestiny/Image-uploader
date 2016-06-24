using System.Data.SQLite;
using System.Configuration;
using ImageUploader.Models;
using System.Collections.Generic;
using System;
using System.IO;

namespace ImageUploader.DB
{
    public class SQLiteDA
    {
        public static string GetConnectionString()
        {            
            return ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;
        }

        public void InsertPerson(Person person)
        {
            using (SQLiteConnection con = new SQLiteConnection(GetConnectionString()))
            {
                if (person.ImageData != null)
                {
                    string query = "insert into Persons(Name,Surname,ImageSize,ImageName,ImageData) " +
                                   "values(@Name,@Surname,@ImageSize,@ImageName,@ImageData);";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, con))
                    {
                        cmd.Parameters.Add(new SQLiteParameter("@Name", person.Name));
                        cmd.Parameters.Add(new SQLiteParameter("@Surname", person.Surname));
                        cmd.Parameters.Add(new SQLiteParameter("@ImageSize", person.ImageSize));
                        cmd.Parameters.Add(new SQLiteParameter("@ImageName", person.ImageName));
                        cmd.Parameters.Add(new SQLiteParameter("@ImageData", person.ImageData));
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    string query = "insert into Persons(Name,Surname,ImageSize,ImageName) " +
                              "values(@Name,@Surname,@ImageSize,@ImageName)";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, con))
                    {
                        cmd.Parameters.Add(new SQLiteParameter("@Name", person.Name));
                        cmd.Parameters.Add(new SQLiteParameter("@Surname", person.Surname));
                        cmd.Parameters.Add(new SQLiteParameter("@ImageSize", person.ImageSize));
                        cmd.Parameters.Add(new SQLiteParameter("@ImageName", person.ImageName));
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public List<Person> GetPersonList()
        {
            List<Person> personsList = new List<Person>();
            using (SQLiteConnection con = new SQLiteConnection(GetConnectionString()))
            {
                string query = "select p.RecordID,p.Name,p.Surname,p.ImageSize,p.ImageName,p.ImageData from Persons p";

                using (SQLiteCommand cmd = new SQLiteCommand(query, con))
                {
                    con.Open();
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
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
            using (SQLiteConnection con = new SQLiteConnection(GetConnectionString()))
            {
                if (person.ImageData != null)
                {
                    string query = "update Persons set Name=@Name,Surname=@Surname,ImageSize=@ImageSize,ImageName=@ImageName,ImageData=@ImageData where RecordID=@RecordID;";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, con))
                    {
                        cmd.Parameters.Add(new SQLiteParameter("@Name", person.Name));
                        cmd.Parameters.Add(new SQLiteParameter("@Surname", person.Surname));
                        cmd.Parameters.Add(new SQLiteParameter("@ImageSize", person.ImageSize));
                        cmd.Parameters.Add(new SQLiteParameter("@ImageName", person.ImageName));
                        cmd.Parameters.Add(new SQLiteParameter("@ImageData", person.ImageData));
                        cmd.Parameters.Add(new SQLiteParameter("@RecordID", person.RecordID));
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    string query = "update Persons set Name=@Name,Surname=@Surname,ImageSize=@ImageSize,ImageName=@ImageName where RecordID=@RecordID;";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, con))
                    {
                        cmd.Parameters.Add(new SQLiteParameter("@Name", person.Name));
                        cmd.Parameters.Add(new SQLiteParameter("@Surname", person.Surname));
                        cmd.Parameters.Add(new SQLiteParameter("@ImageSize", person.ImageSize));
                        cmd.Parameters.Add(new SQLiteParameter("@ImageName", person.ImageName));
                        cmd.Parameters.Add(new SQLiteParameter("@RecordID", person.RecordID));
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void DeletePerson(int itemID)
        {
            using (SQLiteConnection con = new SQLiteConnection(GetConnectionString()))
            {
                string query = "delete from Persons where RecordID=@RecordID;";

                using (SQLiteCommand cmd = new SQLiteCommand(query, con))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@RecordID", itemID));
                    con.Open();
                    cmd.ExecuteNonQuery();
                }

            }
        }

        public Person GetSinglePerson(int itemID)
        {
            Person item = new Person();
            using (SQLiteConnection con = new SQLiteConnection(GetConnectionString()))
            {
                string query = "select p.RecordID,p.Name,p.Surname,p.ImageSize,p.ImageName,p.ImageData from Persons p where p.RecordID=@itemID;";

                using (SQLiteCommand cmd = new SQLiteCommand(query, con))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@itemID", itemID));
                    con.Open();
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
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