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
    public class ScoreController : ControllerBase
    {

        [HttpGet("{whereColumnName}/{whereValue}/{ScoreByColumnName}")]
        public async Task<ActionResult<Score>> GetScores(string whereColumnName, string whereValue, string ScoreByColumnName)
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
                        new SqlParameter("@ScoreByColumnName", SqlDbType.VarChar, 200).WithValue(ScoreByColumnName)
                    };
                    List<Score> result = db.ResultModels.FromSqlRaw("EXEC dbo.GetScores @WhereColumnName, @WhereValue, @ScoreByColumnName", parameters).ToList().Select(r => r.ParseResult<Score>()).ToList();

                    if (result.Count == 0)
                        throw new Exception("Заказ не найден");
                    else
                        return StatusCode(201, result);
                }
                catch (Exception ex)
                {
                    return StatusCode(400, ex.Message);
                }
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Score>> GetScore(int id)
        {
            return await Task.Run(() =>

            {
                try
                {
                    using DataBaseHelper db = new();
                    SqlParameter parameter = new SqlParameter("@ID_Score", SqlDbType.Int).WithValue(id);
                    return StatusCode(201, db.ResultModels.FromSqlRaw("EXEC dbo.GetScore @ID_Score", parameter).ToList()[0].ParseResult<Score>());
                }
                catch (Exception ex)
                {
                    return StatusCode(400, ex.Message);
                }
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutScore(int id, Score Score)
        {
            if (id != Score.ID_Score)
                return BadRequest();

            return await Task.Run(() =>
            {
                using DataBaseHelper db = new();
                SqlParameter[] parameters =
                {
                    new SqlParameter("@ID_Score", SqlDbType.Int).WithValue(Score.ID_Score),
                    new SqlParameter("@Score_number", SqlDbType.Int).WithValue(Score.Score_number),
                    new SqlParameter("@Cabbie_ID", SqlDbType.Int).WithValue(Score.Cabbie.ID_Cabbie),
                    new SqlParameter("@Test_ID", SqlDbType.Int).WithValue(Score.Test.ID_Test),

                };
                try
                {
                    db.Database.ExecuteSqlRaw("EXEC dbo.Score_update @ID_Score, @Score_number, @Cabbie_ID, @Test_ID", parameters);
                    SqlParameter parameter = new SqlParameter("@ID_Score", SqlDbType.Int).WithValue(id);
                    return StatusCode(201, db.ResultModels.FromSqlRaw("EXEC dbo.GetScore @ID_Score", parameter).ToList()[0].ParseResult<Score>());
                }
                catch (Exception ex)
                {
                    return StatusCode(400, ex.Message);
                }
            });
        }

        [HttpPost]
        public async Task<ActionResult<Score>> PostScore(Score Score)
        {
            return await Task.Run(() =>
            {
                using DataBaseHelper db = new();
                SqlParameter[] parameters =
                {

                    new SqlParameter("@Score_number", SqlDbType.Int).WithValue(Score.Score_number),
                    new SqlParameter("@Cabbie_ID", SqlDbType.Int).WithValue(Score.Cabbie.ID_Cabbie),
                    new SqlParameter("@Test_ID", SqlDbType.Int).WithValue(Score.Test.ID_Test),
                    new SqlParameter("@ID_Score", SqlDbType.Int)
                    {
                       Direction = ParameterDirection.Output
                    }
                };
                try
                {
                    db.Database.ExecuteSqlRaw("EXEC dbo.Score_insert @Score_number, @Cabbie_ID, @Test_ID, @ID_Score OUT", parameters);
                    SqlParameter parameter = new SqlParameter("@ID_Score", SqlDbType.Int).WithValue((int)parameters[8].Value);
                    return StatusCode(201, db.ResultModels.FromSqlRaw("EXEC dbo.GetScore @ID_Score", parameter).ToList()[0].ParseResult<Score>());
                }
                catch (Exception ex)
                {
                    return StatusCode(400, ex.Message);
                }
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteZakaz(int id)
        {
            return await Task.Run(() =>
            {
                using DataBaseHelper db = new();
                SqlParameter[] parameters =
                {
                    new SqlParameter("@ID_Score", SqlDbType.Int).WithValue(id)
                };
                try
                {
                    db.Database.ExecuteSqlRaw("EXEC dbo.Score_delete @ID_Score", parameters);
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
