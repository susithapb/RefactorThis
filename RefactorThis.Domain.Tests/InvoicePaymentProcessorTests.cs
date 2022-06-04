using System;
using System.Collections.Generic;
using NUnit.Framework;
using RefactorThis.Persistence;

namespace RefactorThis.Domain.Tests
{
	[TestFixture]
	public class InvoicePaymentProcessorTests
	{
		private InvoiceRepository _repo;
		private InvoicePaymentProcessor _paymentProcessor;
		private int _amount;
		private Payment _payment;
		private string _failureMessage = "";

		[SetUp]
		public void SetUp()
		{
			_repo = new InvoiceRepository();
		    _paymentProcessor = new InvoicePaymentProcessor(_repo);

			_payment = new Payment()
			{
				Amount = _amount
			};
		}

		[Test]
		public void ProcessPayment_Should_ThrowException_When_NoInoiceFoundForPaymentReference( )
		{	

			try
			{
				var result = _paymentProcessor.ProcessPayment( _payment );
			}
			catch ( InvalidOperationException e )
			{
				_failureMessage = e.Message;
			}

			Assert.AreEqual( "There is no invoice matching this payment", _failureMessage );
		}

		[Test]
		public void ProcessPayment_Should_ReturnFailureMessage_When_NoPaymentNeeded( )
		{
			var invoice = new Invoice( _repo )
			{
				Amount = 0,
				AmountPaid = 0,
				Payments = null
			};

			_repo.Add( invoice );			

		    var result = _paymentProcessor.ProcessPayment( _payment );

			Assert.AreEqual( "no payment needed", result );
		}

		[Test]
		public void ProcessPayment_Should_ReturnFailureMessage_When_InvoiceAlreadyFullyPaid( )
		{
			var invoice = new Invoice( _repo )
			{
				Amount = 10,
				AmountPaid = 10,
				Payments = new List<Payment>
				{
					new Payment
					{
						Amount = 10
					}
				}
			};
			_repo.Add( invoice );

			var result = _paymentProcessor.ProcessPayment( _payment );

			Assert.AreEqual( "invoice was already fully paid", result );
		}

		[Test]
		public void ProcessPayment_Should_ReturnFailureMessage_When_PartialPaymentExistsAndAmountPaidExceedsAmountDue( )
		{
			var invoice = new Invoice( _repo )
			{
				Amount = 10,
				AmountPaid = 5,
				Payments = new List<Payment>
				{
					new Payment
					{
						Amount = 5
					}
				}
			};
			_repo.Add( invoice );

			_amount = 6;

			var result = _paymentProcessor.ProcessPayment( _payment );

			Assert.AreEqual( "the payment is greater than the partial amount remaining", result );
		}

		[Test]
		public void ProcessPayment_Should_ReturnFailureMessage_When_NoPartialPaymentExistsAndAmountPaidExceedsInvoiceAmount( )
		{
			var invoice = new Invoice( _repo )
			{
				Amount = 5,
				AmountPaid = 0,
				Payments = new List<Payment>( )
			};
			_repo.Add( invoice );

			_amount = 6;
			

			var result = _paymentProcessor.ProcessPayment( _payment );

			Assert.AreEqual( "the payment is greater than the invoice amount", result );
		}

		[Test]
		public void ProcessPayment_Should_ReturnFullyPaidMessage_When_PartialPaymentExistsAndAmountPaidEqualsAmountDue( )
		{
			var invoice = new Invoice( _repo )
			{
				Amount = 10,
				AmountPaid = 5,
				Payments = new List<Payment>
				{
					new Payment
					{
						Amount = 5
					}
				}
			};
			_repo.Add( invoice );

			_amount = 5;			

			var result = _paymentProcessor.ProcessPayment( _payment );

			Assert.AreEqual( "final partial payment received, invoice is now fully paid", result );
		}

		[Test]
		public void ProcessPayment_Should_ReturnFullyPaidMessage_When_NoPartialPaymentExistsAndAmountPaidEqualsInvoiceAmount( )
		{
			var invoice = new Invoice( _repo )
			{
				Amount = 10,
				AmountPaid = 0,
				Payments = new List<Payment>( ) { new Payment( ) { Amount = 10 } }
			};
			_repo.Add( invoice );

			_amount = 10;			

			var result = _paymentProcessor.ProcessPayment( _payment );

			Assert.AreEqual( "invoice was already fully paid", result );
		}

		[Test]
		public void ProcessPayment_Should_ReturnPartiallyPaidMessage_When_PartialPaymentExistsAndAmountPaidIsLessThanAmountDue( )
		{
			var invoice = new Invoice( _repo )
			{
				Amount = 10,
				AmountPaid = 5,
				Payments = new List<Payment>
				{
					new Payment
					{
						Amount = 5
					}
				}
			};
			_repo.Add( invoice );

			_amount = 1;			

			var result = _paymentProcessor.ProcessPayment( _payment );

			Assert.AreEqual( "another partial payment received, still not fully paid", result );
		}

		[Test]
		public void ProcessPayment_Should_ReturnPartiallyPaidMessage_When_NoPartialPaymentExistsAndAmountPaidIsLessThanInvoiceAmount( )
		{
			var invoice = new Invoice( _repo )
			{
				Amount = 10,
				AmountPaid = 0,
				Payments = new List<Payment>( )
			};
			_repo.Add( invoice );

			_amount = 1;			

			var result = _paymentProcessor.ProcessPayment( _payment );

			Assert.AreEqual( "invoice is now partially paid", result );
		}
	}
}