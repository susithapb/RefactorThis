using RefactorThis.Persistence.Models;
using System.Collections.Generic;

namespace RefactorThis.Persistence
{
	public class Invoice : InvoiceEntity
	{
		private readonly InvoiceRepository _repository;		

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