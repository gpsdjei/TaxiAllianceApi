using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace TaxiAllianceApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class CabbieController : ControllerBase
    {
        [HttpGet("{whereColumnName}/{whereValue}/{orderByColumnName}")] //+
        public async Task<ActionResult<Cabbie>> GetCabbies(string whereColumnName, string whereValue, string orderByColumnName)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using DataBaseHelper db = new();
                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@WhereColumnName", SqlDbType.VarChar, 200).WithValue(whereColumnName),
                        new SqlParameter("@WhereValue", SqlDbType.VarChar, 200).WithValue(whereValue),
                        new SqlParameter("@OrderByColumnName", SqlDbType.VarChar, 200).WithValue(orderByColumnName)
                    };
                    List<Cabbie> result = db.ResultModels.FromSqlRaw("EXEC dbo.GetCabbies @WhereColumnName, @WhereValue, @OrderByColumnName", parameters).ToList().Select(r => r.ParseResult<Cabbie>()).ToList();

                    if (result.Count == 0)
                        throw new Exception("Сотрудник не найден");
                    else
                        return StatusCode(201, result);
                }
                catch (Exception ex)
                {
                    return StatusCode(400, ex.Message);
                }
            });
        }

        [HttpGet("{login}/{password}")]
        public async Task<ActionResult<Cabbie>> GetLoggedUser(string login, string password)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using DataBaseHelper db = new DataBaseHelper();
                    List<Cabbie> users = db.Cabbie.Where(u => (u.Email == login) && (u.Password == password)).ToList();

                    if (users.Count > 0)
                    {
                        SqlParameter parameter = new SqlParameter("@ID_Cabbie", SqlDbType.Int).WithValue(users[0].ID_Cabbie);
                        return StatusCode(201, db.ResultModels.FromSqlRaw("EXEC dbo.GetCabbie @ID_Cabbie", parameter).ToList()[0].ParseResult<Cabbie>());
                    }
                    else
                        throw new Exception("Пользователь не найден");
                }
                catch (Exception ex)
                {
                    return StatusCode(400, ex.Message);
                }
            });
        }


        [HttpGet("{id}")] //+
        public async Task<ActionResult<Cabbie>> GetCabbie(int id)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using DataBaseHelper db = new();
                    SqlParameter parameter = new SqlParameter("@ID_Cabbie", SqlDbType.Int).WithValue(id);
                    return StatusCode(201, db.ResultModels.FromSqlRaw("EXEC dbo.GetCabbie @ID_Cabbie", parameter).ToList()[0].ParseResult<Cabbie>());
                }
                catch (Exception ex)
                {
                    return StatusCode(400, ex.Message);
                }
            });
        }

        [HttpPut("{id}")] //+ 
        public async Task<IActionResult> PutCabbie(int id, Cabbie Cabbie)
        {
            if (id != Cabbie.ID_Cabbie)
                return BadRequest();

            return await Task.Run(() =>
            {
                using DataBaseHelper db = new();
                SqlParameter[] parameters =
                {
                    new SqlParameter("@ID_Cabbie", SqlDbType.Int).WithValue(Cabbie.ID_Cabbie),
                    new SqlParameter("@Name", SqlDbType.VarChar, 100).WithValue(Cabbie.Name),
                    new SqlParameter("@LastName", SqlDbType.VarChar).WithValue(Cabbie.LastName),
                    new SqlParameter("@MidlleName", SqlDbType.VarChar, 100).WithValue(Cabbie.MidlleName),
                    new SqlParameter("@Email", SqlDbType.DateTime).WithValue(Cabbie.Email),
                    new SqlParameter("@Password", SqlDbType.DateTime).WithValue(Cabbie.Password),
                    new SqlParameter("@Phone", SqlDbType.DateTime).WithValue(Cabbie.Phone),
                    new SqlParameter("@Role_ID", SqlDbType.Int).WithValue(Cabbie.Role.ID_Role),
                };
                try
                {
                    db.Database.ExecuteSqlRaw("EXEC dbo.Cabbie_update @ID_Cabbie, @Name, @LastName, @MidlleName, @Email, @Password, @Phone, @Role_ID", parameters);
                    SqlParameter parameter = new SqlParameter("@ID_Cabbie", SqlDbType.Int).WithValue(Cabbie.ID_Cabbie);
                    return StatusCode(201, db.ResultModels.FromSqlRaw("EXEC dbo.GetCabbie @ID_Cabbie", parameter).ToList()[0].ParseResult<Cabbie>());
                }
                catch (Exception ex)
                {
                    return StatusCode(400, ex.Message);
                }
            });
        }

        [HttpPost] //+ 
        public async Task<ActionResult<Cabbie>> PostCabbie(Cabbie Cabbie)
        {
            return await Task.Run(() =>
            {
                using DataBaseHelper db = new();
                SqlParameter[] parameters =
                {

                    new SqlParameter("@Name", SqlDbType.VarChar, 100).WithValue(Cabbie.Name),
                    new SqlParameter("@LastName", SqlDbType.VarChar).WithValue(Cabbie.LastName),
                    new SqlParameter("@MidlleName", SqlDbType.VarChar, 100).WithValue(Cabbie.MidlleName),
                    new SqlParameter("@Email", SqlDbType.DateTime).WithValue(Cabbie.Email),
                    new SqlParameter("@Password", SqlDbType.DateTime).WithValue(Cabbie.Password),
                    new SqlParameter("@Phone", SqlDbType.DateTime).WithValue(Cabbie.Phone),
                    new SqlParameter("@Role_ID", SqlDbType.Int).WithValue(Cabbie.Role.ID_Role),
                    new SqlParameter("@ID_Cabbie", SqlDbType.Int)
                    {
                       Direction = ParameterDirection.Output
                    }
                };
                try
                {
                    db.Database.ExecuteSqlRaw("EXEC dbo.Cabbie_insert @Name, @LastName, @MidlleName, @Email, @Password, @Phone, @Role_ID, @ID_Cabbie OUT", parameters);

                    SqlParameter parameter = new SqlParameter("@ID_Cabbie", SqlDbType.Int).WithValue((int)parameters[7].Value);
                    return StatusCode(201, db.ResultModels.FromSqlRaw("EXEC dbo.GetCabbie @ID_Cabbie", parameter).ToList()[0].ParseResult<Cabbie>());
                }
                catch (Exception ex)
                {
                    return StatusCode(400, ex.Message);
                }
            });
        }

        [HttpDelete("{id}")] //+
        public async Task<IActionResult> DeleteCabbie(int id)
        {
            return await Task.Run(() =>
            {
                using DataBaseHelper db = new();
                SqlParameter[] parameters =
                {
                    new SqlParameter("@ID_Cabbie", SqlDbType.Int).WithValue(id)
                };
                try
                {
                    db.Database.ExecuteSqlRaw("EXEC dbo.Cabbie_delete @ID_Cabbie", parameters);
                    return StatusCode(204, null);
                }
                catch (Exception ex)
                {
                    return StatusCode(400, ex.Message);
                }
            });
        }
    }
}
