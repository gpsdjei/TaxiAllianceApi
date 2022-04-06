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
    public class HR_DepartamentController : ControllerBase
    {
        [HttpGet("{whereColumnName}/{whereValue}/{orderByColumnName}")] //+
        public async Task<ActionResult<HR_Department>> GetHR_Departments(string whereColumnName, string whereValue, string orderByColumnName)
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
                    List<HR_Department> result = db.ResultModels.FromSqlRaw("EXEC dbo.GetHrDepartments @WhereColumnName, @WhereValue, @OrderByColumnName", parameters).ToList().Select(r => r.ParseResult<HR_Department>()).ToList();

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
        public async Task<ActionResult<HR_Department>> GetLoggedUser(string login, string password)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using DataBaseHelper db = new DataBaseHelper();
                    List<HR_Department> users = db.HR_Department.Where(u => (u.Email == login) && (u.Password == password)).ToList();

                    if (users.Count > 0)
                    {
                        SqlParameter parameter = new SqlParameter("@ID_HR_Department", SqlDbType.Int).WithValue(users[0].ID_HR_Department);
                        return StatusCode(201, db.ResultModels.FromSqlRaw("EXEC dbo.GetHRDepartment @ID_HR_Department", parameter).ToList()[0].ParseResult<HR_Department>());
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


        //[HttpGet("{id}")] //+
        //public async Task<ActionResult<HR_Department>> GetHR_Department(int id)
        //{
        //    return await Task.Run(() =>
        //    {
        //        try
        //        {
        //            using DataBaseHelper db = new();
        //            SqlParameter parameter = new SqlParameter("@ID_HR_Department", SqlDbType.Int).WithValue(id);
        //            return StatusCode(201, db.ResultModels.FromSqlRaw("EXEC dbo.GetHrDepartment @ID_HR_Department", parameter).ToList()[0].ParseResult<HR_Department>());
        //        }
        //        catch (Exception ex)
        //        {
        //            return StatusCode(400, ex.Message);
        //        }
        //    });
        //}

        [HttpPut("{id}")] //+ 
        public async Task<IActionResult> PutHR_Department(int id, HR_Department HR_Department)
        {
            if (id != HR_Department.ID_HR_Department)
                return BadRequest();

            return await Task.Run(() =>
            {
                using DataBaseHelper db = new();
                SqlParameter[] parameters =
                {
                    new SqlParameter("@ID_HR_Department", SqlDbType.Int).WithValue(HR_Department.ID_HR_Department),
                    new SqlParameter("@Name_HR_Departmenta", SqlDbType.VarChar, 100).WithValue(HR_Department.Email),
                    new SqlParameter("@LastName_HR_Departmenta", SqlDbType.VarChar).WithValue(HR_Department.Password),
                    new SqlParameter("@Role_ID", SqlDbType.Int).WithValue(HR_Department.Role.ID_Role),
                };
                try
                {
                    db.Database.ExecuteSqlRaw("EXEC dbo.HrDepartment_update @ID_HR_Department, @Email, @Password, @Role_ID", parameters);
                    SqlParameter parameter = new SqlParameter("@ID_HR_Department", SqlDbType.Int).WithValue(HR_Department.ID_HR_Department);
                    return StatusCode(201, db.ResultModels.FromSqlRaw("EXEC dbo.GetHrDepartment @ID_HR_Department", parameter).ToList()[0].ParseResult<HR_Department>());
                }
                catch (Exception ex)
                {
                    return StatusCode(400, ex.Message);
                }
            });
        }

        [HttpPost] //+ 
        public async Task<ActionResult<HR_Department>> PostHR_Department(HR_Department HR_Department)
        {
            return await Task.Run(() =>
            {
                using DataBaseHelper db = new();
                SqlParameter[] parameters =
                {

                    new SqlParameter("@Name_HR_Departmenta", SqlDbType.VarChar, 100).WithValue(HR_Department.Email),
                    new SqlParameter("@LastName_HR_Departmenta", SqlDbType.VarChar).WithValue(HR_Department.Password),
                    new SqlParameter("@Role_ID", SqlDbType.Int).WithValue(HR_Department.Role.ID_Role),
                    new SqlParameter("@ID_HR_Department", SqlDbType.Int)
                    {
                       Direction = ParameterDirection.Output
                    }
                };
                try
                {
                    db.Database.ExecuteSqlRaw("EXEC dbo.HrDepartment_insert  @Email, @Password, @Role_ID @ID_HR_Department OUT", parameters);

                    SqlParameter parameter = new SqlParameter("@ID_HR_Department", SqlDbType.Int).WithValue((int)parameters[6].Value);
                    return StatusCode(201, db.ResultModels.FromSqlRaw("EXEC dbo.GetHrDepartment @ID_HR_Department", parameter).ToList()[0].ParseResult<HR_Department>());
                }
                catch (Exception ex)
                {
                    return StatusCode(400, ex.Message);
                }
            });
        }

        [HttpDelete("{id}")] //+
        public async Task<IActionResult> DeleteHR_Department(int id)
        {
            return await Task.Run(() =>
            {
                using DataBaseHelper db = new();
                SqlParameter[] parameters =
                {
                    new SqlParameter("@ID_HR_Department", SqlDbType.Int).WithValue(id)
                };
                try
                {
                    db.Database.ExecuteSqlRaw("EXEC dbo.HrDepartment_delete @ID_HR_Department", parameters);
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
