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
    public class TestController : ControllerBase
    {
        [HttpGet("{whereColumnName}/{whereValue}/{orderByColumnName}")] //+
        public async Task<ActionResult<Test>> GetTests(string whereColumnName, string whereValue, string orderByColumnName)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using DataBaseHelper db = new();
                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@WhereColumnName", SqlDbType.VarChar, 50).WithValue(whereColumnName),
                        new SqlParameter("@WhereValue", SqlDbType.VarChar, 50).WithValue(whereValue),
                        new SqlParameter("@OrderByColumnName", SqlDbType.VarChar, 50).WithValue(orderByColumnName)
                    };
                    List<Test> result = db.ResultModels.FromSqlRaw("EXEC dbo.GetTests @WhereColumnName, @WhereValue, @OrderByColumnName", parameters).ToList().Select(r => r.ParseResult<Test>()).ToList();

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
        public async Task<ActionResult<Test>> GetTest(int id)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using DataBaseHelper db = new();
                    SqlParameter parameter = new SqlParameter("@ID_Test", SqlDbType.Int).WithValue(id);
                    return StatusCode(201, db.ResultModels.FromSqlRaw("EXEC dbo.GetTest @ID_Test", parameter).ToList()[0].ParseResult<Test>());
                }
                catch (Exception ex)
                {
                    return StatusCode(400, ex.Message);
                }
            });
        }

        [HttpPut("{id}")] //+ 
        public async Task<IActionResult> PutTest(int id, Test Test)
        {
            if (id != Test.ID_Test)
                return BadRequest();

            return await Task.Run(() =>
            {
                using DataBaseHelper db = new();
                SqlParameter[] parameters =
                {
                    new SqlParameter("@ID_Test", SqlDbType.Int).WithValue(Test.ID_Test),
                    new SqlParameter("@Test_number", SqlDbType.Int).WithValue(Test.Test_number),
                    new SqlParameter("@HR_Department_ID", SqlDbType.Int).WithValue(Test.HR_Department.ID_HR_Department),
                };
                try
                {
                    db.Database.ExecuteSqlRaw("EXEC dbo.Test_update @ID_Test, @Test_number, @HR_Department_ID", parameters);
                    SqlParameter parameter = new SqlParameter("@ID_Test", SqlDbType.Int).WithValue(Test.ID_Test);
                    return StatusCode(201, db.ResultModels.FromSqlRaw("EXEC dbo.GetTest @ID_Test", parameter).ToList()[0].ParseResult<Test>());
                }
                catch (Exception ex)
                {
                    return StatusCode(400, ex.Message);
                }
            });
        }

        [HttpPost] //+ 
        public async Task<ActionResult<Test>> PostTest(Test Test)
        {
            return await Task.Run(() =>
            {
                using DataBaseHelper db = new();
                SqlParameter[] parameters =
                {

                    new SqlParameter("@Test_number", SqlDbType.Int).WithValue(Test.Test_number),
                    new SqlParameter("@HR_Department_ID", SqlDbType.Int).WithValue(Test.HR_Department.ID_HR_Department),
                    new SqlParameter("@ID_Test", SqlDbType.Int)
                    {
                       Direction = ParameterDirection.Output
                    }
                };
                try
                {
                    db.Database.ExecuteSqlRaw("EXEC dbo.Test_insert @Test_number, @HR_Department_ID, @ID_Test OUT", parameters);

                    SqlParameter parameter = new SqlParameter("@ID_Test", SqlDbType.Int).WithValue((int)parameters[6].Value);
                    return StatusCode(201, db.ResultModels.FromSqlRaw("EXEC dbo.GetTest @ID_Test", parameter).ToList()[0].ParseResult<Test>());
                }
                catch (Exception ex)
                {
                    return StatusCode(400, ex.Message);
                }
            });
        }

        [HttpDelete("{id}")] //+
        public async Task<IActionResult> DeleteTest(int id)
        {
            return await Task.Run(() =>
            {
                using DataBaseHelper db = new();
                SqlParameter[] parameters =
                {
                    new SqlParameter("@ID_Test", SqlDbType.Int).WithValue(id)
                };
                try
                {
                    db.Database.ExecuteSqlRaw("EXEC dbo.Test_delete @ID_Test", parameters);
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
