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
    public class LecturesController : ControllerBase
    {
        [HttpGet("{whereColumnName}/{whereValue}/{orderByColumnName}")] //+
        public async Task<ActionResult<Lectures>> GetLectures(string whereColumnName, string whereValue, string orderByColumnName)
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
                    List<Lectures> result = db.ResultModels.FromSqlRaw("EXEC dbo.GetLectures @WhereColumnName, @WhereValue, @OrderByColumnName", parameters).ToList().Select(r => r.ParseResult<Lectures>()).ToList();

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

        [HttpGet("{id}")] //+
        public async Task<ActionResult<Lectures>> GetLectures(int id)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using DataBaseHelper db = new();
                    SqlParameter parameter = new SqlParameter("@ID_Lecture", SqlDbType.Int).WithValue(id);
                    return StatusCode(201, db.ResultModels.FromSqlRaw("EXEC dbo.GetLecture @ID_Lecture", parameter).ToList()[0].ParseResult<Lectures>());
                }
                catch (Exception ex)
                {
                    return StatusCode(400, ex.Message);
                }
            });
        }

        [HttpPut("{id}")] //+ 
        public async Task<IActionResult> PutLectures(int id, Lectures Lectures)
        {
            if (id != Lectures.ID_Lecture)
                return BadRequest();

            return await Task.Run(() =>
            {
                using DataBaseHelper db = new();
                SqlParameter[] parameters =
                {
                    new SqlParameter("@ID_Lecture", SqlDbType.Int).WithValue(Lectures.ID_Lecture),
                    new SqlParameter("@Name_Lecture", SqlDbType.VarChar, 100).WithValue(Lectures.Name_Lecture),
                    new SqlParameter("@Lecture", SqlDbType.VarChar).WithValue(Lectures.Lecture),
                    new SqlParameter("@Test_ID", SqlDbType.Int).WithValue(Lectures.Test.ID_Test),
                };
                try
                {
                    db.Database.ExecuteSqlRaw("EXEC dbo.Lecture_update @ID_Lecture, @Name_Lecture, @Lecture, @Test_ID", parameters);
                    SqlParameter parameter = new SqlParameter("@ID_Lecture", SqlDbType.Int).WithValue(Lectures.ID_Lecture);
                    return StatusCode(201, db.ResultModels.FromSqlRaw("EXEC dbo.GetLecture @ID_Lecture", parameter).ToList()[0].ParseResult<Lectures>());
                }
                catch (Exception ex)
                {
                    return StatusCode(400, ex.Message);
                }
            });
        }

        [HttpPost] //+ 
        public async Task<ActionResult<Lectures>> PostLectures(Lectures Lectures)
        {
            return await Task.Run(() =>
            {
                using DataBaseHelper db = new();
                SqlParameter[] parameters =
                {

                    new SqlParameter("@Name_Lecture", SqlDbType.VarChar, 100).WithValue(Lectures.Name_Lecture),
                    new SqlParameter("@Lecture", SqlDbType.VarChar).WithValue(Lectures.Lecture),
                    new SqlParameter("@Test_ID", SqlDbType.Int).WithValue(Lectures.Test.ID_Test),
                    new SqlParameter("@ID_Lecture", SqlDbType.Int)
                    {
                       Direction = ParameterDirection.Output
                    }
                };
                try
                {
                    db.Database.ExecuteSqlRaw("EXEC dbo.Lecture_insert @Name_Lecture, @Lecture, @Test_ID, @ID_Lecture OUT", parameters);

                    SqlParameter parameter = new SqlParameter("@ID_Lecture", SqlDbType.Int).WithValue((int)parameters[6].Value);
                    return StatusCode(201, db.ResultModels.FromSqlRaw("EXEC dbo.GetLecture @ID_Lecture", parameter).ToList()[0].ParseResult<Lectures>());
                }
                catch (Exception ex)
                {
                    return StatusCode(400, ex.Message);
                }
            });
        }

        [HttpDelete("{id}")] //+
        public async Task<IActionResult> DeleteLectures(int id)
        {
            return await Task.Run(() =>
            {
                using DataBaseHelper db = new();
                SqlParameter[] parameters =
                {
                    new SqlParameter("@ID_Lecture", SqlDbType.Int).WithValue(id)
                };
                try
                {
                    db.Database.ExecuteSqlRaw("EXEC dbo.Lecture_delete @ID_Lecture", parameters);
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
