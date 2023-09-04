using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADO_1.DAL.Entity;
using System.Windows;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ADO_1.DAL.DAO
{
    internal class ProductGroupDao
    {
        private readonly SqlConnection _connection;

        public ProductGroupDao(SqlConnection connection) 
        {
            this._connection = connection;
        }

        public List<Entity.ProductGroup> GetAll() 
        {
            using SqlCommand command = new();
            command.Connection = _connection;
            command.CommandText = "SELECT pg.* FROM ProductGroups pg WHERE pg.DeleteDt IS NULL";
            try
            {
                using SqlDataReader reader = command.ExecuteReader();
                var ProductGroups = new List<Entity.ProductGroup>();
                while (reader.Read())  
                {
                 
                    ProductGroups.Add(new()
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        Picture = reader.GetString(3),
                    });
                }
                return ProductGroups;
            }
            catch { throw; }

        }

        public void Add(Entity.ProductGroup productGroup)
        {
            using SqlCommand command = new();
            command.Connection = _connection;
            command.CommandText = "INSERT INTO ProductGroups ( Id, Name, Description, Picture ) VALUES(@id, @name, @description, @picture)";
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.UniqueIdentifier));
            command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 50));
            command.Parameters.Add(new SqlParameter("@description", SqlDbType.NText));
            command.Parameters.Add(new SqlParameter("@picture", SqlDbType.NVarChar, 50));

            command.Parameters[0].Value = productGroup.Id;
            command.Parameters[1].Value = productGroup.Name;
            command.Parameters[2].Value = productGroup.Description;
            command.Parameters[3].Value = productGroup.Picture;

            command.ExecuteNonQuery();
        }

        public void Delete(Entity.ProductGroup productGroup) 
        {
            using SqlCommand command = new();
            command.Connection = _connection;
            command.CommandText = $@"
                UPDATE
                    ProductGroups 
                SET 
                    DeleteDt = @datetime
                WHERE 
                    Id = @id ";
            command.Prepare();
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.UniqueIdentifier));
            command.Parameters.Add(new SqlParameter("@datetime", SqlDbType.DateTime));

            command.Parameters[0].Value = productGroup.Id;
            command.Parameters[1].Value = DateTime.Now;

            command.ExecuteNonQuery();
        }

        public void Update(Entity.ProductGroup productGroup)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = $"UPDATE ProductGroups SET Name = @name, Description = @description, Picture = @picture WHERE Id = @id ";
            cmd.Prepare();
            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.UniqueIdentifier));
            cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 50));
            cmd.Parameters.Add(new SqlParameter("@description", SqlDbType.NText));
            cmd.Parameters.Add(new SqlParameter("@picture", SqlDbType.NVarChar, 50));
            cmd.Parameters[0].Value = productGroup.Id;
            cmd.Parameters[1].Value = productGroup.Name;
            cmd.Parameters[2].Value = productGroup.Description;
            cmd.Parameters[3].Value = productGroup.Picture;
            cmd.ExecuteNonQuery();
        }

        public List<Entity.ProductGroup> getDeleted()
        {
            SqlCommand command = new SqlCommand();
            command.Connection = _connection;
            command.CommandText = "SELECT * FROM ProductGroups WHERE DeleteDT IS NOT NULL";
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                var ProductGroups = new List<Entity.ProductGroup>();
                while (reader.Read())
                {
                    ProductGroups.Add(new DAL.Entity.ProductGroup()
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        Picture = reader.GetString(3)
                    });
                }
                reader.Close();
                return ProductGroups;
            }
            catch { throw; }
        }

        public void Restore(Entity.ProductGroup productGroup)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = $"UPDATE ProductGroups SET DeleteDT = NULL WHERE Id = @id ";
            cmd.Prepare();
            cmd.Parameters.Add(new SqlParameter("id", SqlDbType.UniqueIdentifier));
            cmd.Parameters[0].Value = productGroup.Id;
            cmd.ExecuteNonQuery();
        }
    }
}
