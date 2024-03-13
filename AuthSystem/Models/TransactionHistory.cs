using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace InterviewTest_Clicknext.Models
{
    public class TransactionHistory
    {
        [Key]
        public int Id { get; set; }
        public string TransactionUniqueReference { get; set; }
        
        [DisplayName("Amount")]
        [Range(1, 1000, ErrorMessage = "Enter a number greater than 1")]
        [DisplayFormat(DataFormatString = "{0:#,##0.00}")]
        public double TransactionAmount { get; set; }
        public TranStatus TransactionStatus { get; set; }
        public bool IsSuccessful => TransactionStatus.Equals(TranStatus.Success);
        
        [DisplayName("Source Account")]
        public string TransactionSourceAccount { get; set; }
        
        [DisplayName("Destination Account")]
        public string TransactionDestinationAccount { get; set; }
        public string TransactionParticulars { get; set; }
        [DisplayName("Action")]
        public TranType TransactionType { get; set; }
        [DisplayName("Date time")]
        public DateTime TransactionDate { get; set; }
        [DisplayName("From")]
        public string? TransactionSourceAccountName { get; set; }
        [DisplayName("User")]
        public string? TransactionDestinationAccountName { get; set; }
        [DisplayName("Remain")]
        [DisplayFormat(DataFormatString = "{0:#,##0.00}")]
        public double TransactionSourceAccountRemain { get; set; }
        [DisplayName("Remain")]
        [DisplayFormat(DataFormatString = "{0:#,##0.00}")]
        public double TransactionDestinationAccountRemain { get; set; }

        public TransactionHistory()
        {
            TransactionUniqueReference = $"{Guid.NewGuid().ToString().Replace("-", "").Substring(1, 17)}";
        }


    }

    public enum TranStatus
    {
        Failed,
        Success,
        Error
    }

    public enum TranType
    {
        Deposit,
        Withdrawal,
        Transfer
    }
}
