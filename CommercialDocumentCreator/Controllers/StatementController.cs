using CommercialDocumentCreator.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommercialDocumentCreator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatementController : ControllerBase
    {

        private readonly StatementHelper _statementHelper;
        public StatementController(StatementHelper statementHelper)
        {
            this._statementHelper = statementHelper;
        }



        #region GET STATEMENT

        [HttpGet("/api/get/statement/{start}/{end}")]
        public async Task<IActionResult> GetStatement([FromRoute] DateTime start, [FromRoute] DateTime end)
        {
            var details = await this._statementHelper.Statement(start, end);

            if (details is null)
            {
                return NotFound();
            }


            return Ok(details);
        }

        #endregion


    }
}
