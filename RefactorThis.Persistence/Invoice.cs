using System.Collections.Generic;

namespace RefactorThis.Persistence
{
	public class Invoice
	{
		private readonly InvoiceRepository _repository;
		public decimal Amount { get; set; }
		public decimal AmountPaid { get; set; }
		public List<Payment> Payments { get; set; }

		public Invoice( InvoiceRepository repository )
		{
			_repository = repository;
		}

		public void Save( )
		{
			_repository.SaveInvoice( this );
		}

		
	}
}