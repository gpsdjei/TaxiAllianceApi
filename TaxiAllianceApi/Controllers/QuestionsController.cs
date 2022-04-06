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
    public class QuestionsController : ControllerBase
    {
        [HttpGet("{whereColumnName}/{whereValue}/{orderByColumnName}")] //+
        public async Task<ActionResult<Questions>> GetQuestionss(string whereColumnName, string whereValue, string orderByColumnName)
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
                    List<Questions> result = db.ResultModels.FromSqlRaw("EXEC dbo.GetQuestions @WhereColumnName, @WhereValue, @OrderByColumnName", parameters).ToList().Select(r => r.ParseResult<Questions>()).ToList();

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
        public async Task<ActionResult<Questions>> GetQuestions(int id)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using DataBaseHelper db = new();
                    SqlParameter parameter = new SqlParameter("@ID_Question", SqlDbType.Int).WithValue(id);
                    return StatusCode(201, db.ResultModels.FromSqlRaw("EXEC dbo.GetQuestion @ID_Question", parameter).ToList()[0].ParseResult<Questions>());
                }
                catch (Exception ex)
                {
                    return StatusCode(400, ex.Message);
                }
            });
        }

        [HttpPut("{id}")] //+ 
        public async Task<IActionResult> PutQuestions(int id, Questions Questions)
        {
            if (id != Questions.ID_Question)
                return BadRequest();

            return await Task.Run(() =>
            {
                using DataBaseHelper db = new();
                SqlParameter[] parameters =
                {
                    new SqlParameter("@ID_Questions", SqlDbType.Int).WithValue(Questions.ID_Question),
                    new SqlParameter("@Question_number", SqlDbType.Int).WithValue(Questions.Question_number),
                    new SqlParameter("@Question", SqlDbType.VarChar).WithValue(Questions.Question),
                    new SqlParameter("@Test_ID", SqlDbType.Int).WithValue(Questions.Test.ID_Test),
                };
                try
                {
                    db.Database.ExecuteSqlRaw("EXEC dbo.Question_update @ID_Question, @Question_number, @Question, @Test_ID", parameters);
                    SqlParameter parameter = new SqlParameter("@ID_Question", SqlDbType.Int).WithValue(Questions.ID_Question);
                    return StatusCode(201, db.ResultModels.FromSqlRaw("EXEC dbo.GetQuestion @ID_Question", parameter).ToList()[0].ParseResult<Questions>());
                }
                catch (Exception ex)
                {
                    return StatusCode(400, ex.Message);
                }
            });
        }

        [HttpPost] //+ 
        public async Task<ActionResult<Questions>> PostQuestions(Questions Questions)
        {
            return await Task.Run(() =>
            {
                using DataBaseHelper db = new();
                SqlParameter[] parameters =
                {

                    new SqlParameter("@Question_number", SqlDbType.Int).WithValue(Questions.Question_number),
                    new SqlParameter("@Question", SqlDbType.VarChar).WithValue(Questions.Question),
                    new SqlParameter("@Test_ID", SqlDbType.Int).WithValue(Questions.Test.ID_Test),
                    new SqlParameter("@ID_Question", SqlDbType.Int)
                    {
                       Direction = ParameterDirection.Output
                    }
                };
                try
                {
                    db.Database.ExecuteSqlRaw("EXEC dbo.Question_insert @Question_number, @Question, @Test_ID, @ID_Question OUT", parameters);

                    SqlParameter parameter = new SqlParameter("@ID_Question", SqlDbType.Int).WithValue((int)parameters[6].Value);
                    return StatusCode(201, db.ResultModels.FromSqlRaw("EXEC dbo.GetQuestion @ID_Question", parameter).ToList()[0].ParseResult<Questions>());
                }
                catch (Exception ex)
                {
                    return StatusCode(400, ex.Message);
                }
            });
        }

        [HttpDelete("{id}")] //+
        public async Task<IActionResult> DeleteQuestions(int id)
        {
            return await Task.Run(() =>
            {
                using DataBaseHelper db = new();
                SqlParameter[] parameters =
                {
                    new SqlParameter("@ID_Question", SqlDbType.Int).WithValue(id)
                };
                try
                {
                    db.Database.ExecuteSqlRaw("EXEC dbo.Question_delete @ID_Question", parameters);
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
