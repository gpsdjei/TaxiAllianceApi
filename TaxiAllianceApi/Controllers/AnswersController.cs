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
    public class AnswersController : ControllerBase
    {
        [HttpGet("{whereColumnName}/{whereValue}/{orderByColumnName}")] //+
        public async Task<ActionResult<Answers>> GetAnswers(string whereColumnName, string whereValue, string orderByColumnName)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using DataBaseHelper db = new();
                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@WhereColumnName", SqlDbType.VarChar, 100).WithValue(whereColumnName),
                        new SqlParameter("@WhereValue", SqlDbType.VarChar, 100).WithValue(whereValue),
                        new SqlParameter("@OrderByColumnName", SqlDbType.VarChar, 100).WithValue(orderByColumnName)
                    };
                    List<Answers> result = db.ResultModels.FromSqlRaw("EXEC dbo.GetAnswers @WhereColumnName, @WhereValue, @OrderByColumnName", parameters).ToList().Select(r => r.ParseResult<Answers>()).ToList();

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
        public async Task<ActionResult<Answers>> GetAnswer(int id)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using DataBaseHelper db = new();
                    SqlParameter parameter = new SqlParameter("@ID_Answer", SqlDbType.Int).WithValue(id);
                    return StatusCode(201, db.ResultModels.FromSqlRaw("EXEC dbo.GetAnswer @ID_Answer", parameter).ToList()[0].ParseResult<Answers>());
                }
                catch (Exception ex)
                {
                    return StatusCode(400, ex.Message);
                }
            });
        }

        [HttpPut("{id}")] //+ 
        public async Task<IActionResult> PutAnswers(int id, Answers Answers)
        {
            if (id != Answers.ID_Answer)
                return BadRequest();

            return await Task.Run(() =>
            {
                using DataBaseHelper db = new();
                SqlParameter[] parameters =
                {
                    new SqlParameter("@ID_Answer", SqlDbType.Int).WithValue(Answers.ID_Answer),
                    new SqlParameter("@Answer", SqlDbType.VarChar, 100).WithValue(Answers.Answer),
                    new SqlParameter("@Right_answer", SqlDbType.Bit).WithValue(Answers.Right_answer),
                    new SqlParameter("@Question_ID", SqlDbType.Int).WithValue(Answers.Questions.ID_Question),
                };
                try
                {
                    db.Database.ExecuteSqlRaw("EXEC dbo.Answer_update @ID_Answer, @Answer, @Right_answer, @Question_ID", parameters);
                    SqlParameter parameter = new SqlParameter("@ID_Answer", SqlDbType.Int).WithValue(Answers.ID_Answer);
                    return StatusCode(201, db.ResultModels.FromSqlRaw("EXEC dbo.GetAnswer @ID_Answer", parameter).ToList()[0].ParseResult<Answers>());
                }
                catch (Exception ex)
                {
                    return StatusCode(400, ex.Message);
                }
            });
        }

        [HttpPost] //+ 
        public async Task<ActionResult<Answers>> PostAnswers(Answers Answers)
        {
            return await Task.Run(() =>
            {
                using DataBaseHelper db = new();
                SqlParameter[] parameters =
                {

                    new SqlParameter("@Answer", SqlDbType.VarChar, 100).WithValue(Answers.Answer),
                    new SqlParameter("@Right_answer", SqlDbType.Bit).WithValue(Answers.Right_answer),
                    new SqlParameter("@Question_ID", SqlDbType.Int).WithValue(Answers.Questions.ID_Question),
                    new SqlParameter("@ID_Answer", SqlDbType.Int)
                    {
                       Direction = ParameterDirection.Output
                    }
                };
                try
                {
                    db.Database.ExecuteSqlRaw("EXEC dbo.Answer_insert @Answer, @Right_answer, @Question_ID, @ID_Answer OUT", parameters);

                    SqlParameter parameter = new SqlParameter("@ID_Answer", SqlDbType.Int).WithValue((int)parameters[6].Value);
                    return StatusCode(201, db.ResultModels.FromSqlRaw("EXEC dbo.GetAnswer @ID_Answer", parameter).ToList()[0].ParseResult<Answers>());
                }
                catch (Exception ex)
                {
                    return StatusCode(400, ex.Message);
                }
            });
        }

        [HttpDelete("{id}")] //+
        public async Task<IActionResult> DeleteAnswers(int id)
        {
            return await Task.Run(() =>
            {
                using DataBaseHelper db = new();
                SqlParameter[] parameters =
                {
                    new SqlParameter("@ID_Answer", SqlDbType.Int).WithValue(id)
                };
                try
                {
                    db.Database.ExecuteSqlRaw("EXEC dbo.Answer_delete @ID_Answer", parameters);
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
