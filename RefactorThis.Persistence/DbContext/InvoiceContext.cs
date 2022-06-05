using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace RefactorThis.Persistence
{
    public class InvoiceContext : DbContext
    {
        public InvoiceContext() : base("name=InvoiceContext")
        {

        }

        public DbSet<Invoice> Invoices { get; set; }
    }
}
