namespace InvoiceManagement.Data
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public double InvoiceAmount { get; set; }
        public string InvoiceMonth { get; set; }
        public string InvoiceOwner { get; set; }
    }
}
