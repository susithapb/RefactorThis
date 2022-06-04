namespace RefactorThis.Persistence {
	public class InvoiceRepository : IInvoiceRepository
	{
		private Invoice _invoice;
		private readonly InvoiceContext _invoiceContext;

		public Invoice GetInvoice( string reference )
		{
			return _invoice;
		}

		public void SaveInvoice( Invoice invoice )
		{
			_invoiceContext.Invoices.Add(invoice);
			_invoiceContext.SaveChanges();
		}

		public void Add( Invoice invoice )
		{
			_invoice = invoice;
		}
	}
}