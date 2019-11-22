using Microsoft.Azure.Cosmos.Table;

namespace Teknologisk.WebApp.Models
{
    public class Customer : TableEntity
    {
        public Customer()
        {
            
        }

        public Customer(string area, string customerNo)
        {
            this.PartitionKey = area;
            this.RowKey = customerNo;
        }

        public string Email;
        public string LastName;
        public string FirstName;
    }
}
