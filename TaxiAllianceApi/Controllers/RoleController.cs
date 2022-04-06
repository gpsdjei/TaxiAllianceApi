using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TaxiAllianceApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        [HttpGet] //+
        public async Task<ActionResult<Role>> GetRoles()
        {
            return await Task.Run(() =>
            {
                try
                {
                    using DataBaseHelper db = new();
                    return StatusCode(201, db.Role.ToList());
                }
                catch (Exception ex)
                {
                    return StatusCode(400, ex.Message);
                }
            });
        }
    }
}
