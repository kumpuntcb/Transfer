using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using Transfer.Models;

namespace Transfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Money_transctionsController : ControllerBase
    {
        private money_transctionsContext money_transctionsContext;
        public Money_transctionsController(money_transctionsContext transctionsContext)
        {
            money_transctionsContext = transctionsContext;
        }
        //GET API
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var db = money_transctionsContext;
                var data = db.transactions.ToList();

                if (data == null) { return NotFound(); }
                return Ok(data);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
            
        }


        [Route("savedata")]
        [HttpPost]
        public async Task<IActionResult> Post(Transfer.Models.transaction transaction)
        {
            if (transaction == null)
            {
                return BadRequest("Invalid transaction data");
            }

            try
            {
                var db = money_transctionsContext;
                var borrowData = db.transactions.Where(x => x.borrower_name == transaction.borrower_name
                                                            && x.lender_name == transaction.lender_name
                                                            && x.transaction_type == "Borrow").Sum(x => (int?)x.amount) ?? 0;

                var returnData = db.transactions.Where(x => x.borrower_name == transaction.borrower_name
                                                            && x.lender_name == transaction.lender_name
                                                            && x.transaction_type == "Return").Sum(x => (int?)x.amount) ?? 0;

                var remainingDebt = borrowData - returnData;
                if (transaction.transaction_type == "Borrow" || (borrowData == 0 && returnData == 0))
                {
                    money_transctionsContext.transactions.Add(transaction);
                    await money_transctionsContext.SaveChangesAsync();
                    return Ok("Transaction saved successfully");
                }
                else
                {
                    if (remainingDebt <= 0)
                    {
                        return BadRequest("You have already returned all the borrowed money. There's no more debt to be returned.");
                    }
                    else
                    {
                        money_transctionsContext.transactions.Add(transaction);
                        await money_transctionsContext.SaveChangesAsync();
                        return Ok("Transaction saved successfully");
                    }
                }
                

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

    }
}