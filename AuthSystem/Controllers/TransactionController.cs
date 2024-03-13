using InterviewTest_Clicknext.Areas.Identity.Data;
using InterviewTest_Clicknext.Data;
using InterviewTest_Clicknext.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace InterviewTest_Clicknext.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TransactionDbContext _context;

        public TransactionController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, TransactionDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        

        // GET: Transaction/AddOrEdit
        public IActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View();
            else
                return View(_context.TransactionsHistorys.Find(id));
        }

        public IActionResult Deposit()
        {

            return View(new TransactionHistory());
        }

        public IActionResult Transfer()
        {
            return View(new TransactionHistory());
        }
        public IActionResult Withdrawal()
        {
            return View(new TransactionHistory());
        }

        // GET: Transfer Transaction
        public async Task<IActionResult> TransferTransaction()
        {
            var ds = (from history in _context.TransactionsHistorys
                      join accSource in _context.Users on history.TransactionSourceAccount equals accSource.Id
                      join accDestination in _context.Users on history.TransactionDestinationAccount equals accDestination.Id
                      where history.TransactionSourceAccount == _userManager.GetUserId(User) // get transaction Source Transfer
                      //&& history.TransactionType == TranType.Transfer // get transaction Source Receive
                      select new TransactionHistory()
                      {
                          TransactionSourceAccount = accSource.AccountNumberGenerated,
                          TransactionSourceAccountName = accSource.FirstName,
                          TransactionDestinationAccount = accDestination.AccountNumberGenerated,
                          TransactionDestinationAccountName = accDestination.FirstName,
                          TransactionType = history.TransactionType,
                          TransactionDate = history.TransactionDate,
                          TransactionAmount = history.TransactionAmount,
                          TransactionSourceAccountRemain = history.TransactionSourceAccountRemain
                      }).ToList();

            return View(ds.ToList());
        }

        // GET: Receive Transaction
        public async Task<IActionResult> ReceiveTransaction()
        {
            var ds = (from history in _context.TransactionsHistorys
                      join accSource in _context.Users on history.TransactionSourceAccount equals accSource.Id
                      join accDestination in _context.Users on history.TransactionDestinationAccount equals accDestination.Id
                      where history.TransactionDestinationAccount == _userManager.GetUserId(User)
                      && history.TransactionType == TranType.Transfer // get transaction Source Receive
                      select new TransactionHistory()
                      {
                          TransactionSourceAccount = accSource.AccountNumberGenerated,
                          TransactionSourceAccountName = accSource.FirstName,
                          TransactionDestinationAccount = accDestination.AccountNumberGenerated,
                          TransactionDestinationAccountName = accDestination.FirstName,
                          TransactionType = history.TransactionType,
                          TransactionDate = history.TransactionDate,
                          TransactionAmount = history.TransactionAmount,
                          TransactionDestinationAccountRemain = history.TransactionDestinationAccountRemain,
                      }).ToList();

            return View(ds.ToList());
        }

        // POST: Transaction/Deposit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deposit([FromForm] TransactionHistory transactionHistory)
        {


            if (transactionHistory.TransactionAmount > 0)
            {
                // Source account
                ApplicationUser sourceAccount = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
                sourceAccount.CurrentAccountBalance += transactionHistory.TransactionAmount; // ลบ CurrentAccountBalance ออกจากผู้ฝาก

                //// Destination account 
                //var listUsers = _userManager.Users.Where(user => user.AccountNumberGenerated == transactionHistory.TransactionDestinationAccount).ToList();
                //if (listUsers.Count == 0) {
                //    ModelState.AddModelError("TransactionDestinationAccount", "No Account!");
                //    return View();
                //}
                   
                //ApplicationUser destinationAccount = _userManager.Users.Where(user => user.AccountNumberGenerated == transactionHistory.TransactionDestinationAccount).ToList()[0];
                //destinationAccount.CurrentAccountBalance += transactionHistory.TransactionAmount; // เพิ่ม CurrentAccountBalance ให้ผู้รับ

                transactionHistory.TransactionDate = DateTime.Now;
                transactionHistory.TransactionSourceAccount = sourceAccount.Id; // ผู้ฝากเงิน
                transactionHistory.TransactionDestinationAccount = sourceAccount.Id; // ปลายทาง
                transactionHistory.TransactionStatus = TranStatus.Success;
                transactionHistory.TransactionType = TranType.Deposit;
                transactionHistory.TransactionSourceAccountRemain = sourceAccount.CurrentAccountBalance; // ยอดคงเหลือ Source account
                transactionHistory.TransactionParticulars = $"NEW Transaction FROM SOURCE {JsonConvert.SerializeObject(transactionHistory.TransactionSourceAccount)} TO DESTINATION => {JsonConvert.SerializeObject(transactionHistory.TransactionDestinationAccount)} ON {transactionHistory.TransactionDate} TRAN_TYPE =>  {transactionHistory.TransactionType} TRAN_STATUS => {transactionHistory.TransactionStatus}";
                _context.Update(transactionHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(TransferTransaction));
            }
            else
            {
                return View();
            }
        }

        // POST: Transaction/Transfer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Transfer([FromForm] TransactionHistory transactionHistory)
        {


            if (transactionHistory.TransactionDestinationAccount != null)
            {
                // Source account
                ApplicationUser sourceAccount = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
                sourceAccount.CurrentAccountBalance -= transactionHistory.TransactionAmount;// ลบ CurrentAccountBalance ให้ผู้โอน


                // Destination account 
                // เช็ค account ที่มีอยู่ในระบบ
                var listUsers = _userManager.Users.Where(user => user.AccountNumberGenerated == transactionHistory.TransactionDestinationAccount).ToList();
                if (listUsers.Count == 0)
                {
                    ModelState.AddModelError("TransactionDestinationAccount", "No Account!");
                    return View();
                }

                // เช็ค account เดียวกันกับผู้โอน
                ApplicationUser destinationAccount = _userManager.Users.Where(user => user.AccountNumberGenerated == transactionHistory.TransactionDestinationAccount).ToList()[0];
                if (sourceAccount.AccountNumberGenerated == destinationAccount.AccountNumberGenerated)
                {
                    ModelState.AddModelError("TransactionDestinationAccount", "Unable to transfer!");
                    return View();
                }
                destinationAccount.CurrentAccountBalance += transactionHistory.TransactionAmount; // เพิ่ม CurrentAccountBalance ให้ผู้รับ

                transactionHistory.TransactionDate = DateTime.Now;
                transactionHistory.TransactionSourceAccount = sourceAccount.Id; // ผู้โอน
                transactionHistory.TransactionDestinationAccount = destinationAccount.Id; // ผู้รับ
                transactionHistory.TransactionStatus = TranStatus.Success;
                transactionHistory.TransactionType = TranType.Transfer;
                transactionHistory.TransactionSourceAccountRemain = sourceAccount.CurrentAccountBalance; // ยอดคงเหลือ Source account
                transactionHistory.TransactionDestinationAccountRemain = destinationAccount.CurrentAccountBalance; // ยอดคงเหลือ Destination account
                transactionHistory.TransactionParticulars = $"NEW Transaction FROM SOURCE {JsonConvert.SerializeObject(transactionHistory.TransactionSourceAccount)} TO DESTINATION => {JsonConvert.SerializeObject(transactionHistory.TransactionDestinationAccount)} ON {transactionHistory.TransactionDate} TRAN_TYPE =>  {transactionHistory.TransactionType} TRAN_STATUS => {transactionHistory.TransactionStatus}";
                _context.Update(transactionHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(TransferTransaction));
            }
            else
            { 
                return View(); 
            }
        }

        // POST: Transaction/Withdrawal
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Withdrawal([FromForm] TransactionHistory transactionHistory)
        {
            if (transactionHistory.TransactionAmount > 0)
            {
                // Source account
                ApplicationUser sourceAccount = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
                
                sourceAccount.CurrentAccountBalance -= transactionHistory.TransactionAmount;// ลบ CurrentAccountBalance ผู้ถอน

                transactionHistory.TransactionDate = DateTime.Now;
                transactionHistory.TransactionSourceAccount = sourceAccount.Id; // SourceAccount
                transactionHistory.TransactionDestinationAccount = sourceAccount.Id; // DestinationAccount

                transactionHistory.TransactionStatus = TranStatus.Success;
                transactionHistory.TransactionType = TranType.Withdrawal;
                transactionHistory.TransactionSourceAccountRemain = sourceAccount.CurrentAccountBalance; // ยอดคงเหลือ Source account
                transactionHistory.TransactionParticulars = $"NEW Transaction FROM SOURCE {JsonConvert.SerializeObject(transactionHistory.TransactionSourceAccount)} TO DESTINATION => {JsonConvert.SerializeObject(transactionHistory.TransactionDestinationAccount)} ON {transactionHistory.TransactionDate} TRAN_TYPE =>  {transactionHistory.TransactionType} TRAN_STATUS => {transactionHistory.TransactionStatus}";
                _context.Update(transactionHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(TransferTransaction));
            }
            else {
                return View();
            }

        }

        // POST: Transaction/AddOrEdit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddOrEdit([Bind("TransactionId,AccountNumber,BeneficiaryName,BankName,SWIFTCode,Amount,Date")] Transaction transaction)
        //{
        //    // Server Side Validation
        //    if (ModelState.IsValid)
        //    {
        //        if (transaction.TransactionId == 0)
        //        {
        //            transaction.Date = DateTime.Now;
        //            _context.Add(transaction);
        //        }
        //        else
        //            _context.Update(transaction);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(transaction);
        //}




        //// POST: Transaction/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var transaction = await _context.Transactions.FindAsync(id);
        //    _context.Transactions.Remove(transaction);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
    }
}
