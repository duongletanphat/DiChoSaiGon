using System;
using System.Collections.Generic;

#nullable disable

namespace DiChoSaiGon.Models
{
    public partial class Customer
    {
        public Customer()
        {
            CustomerCustomerDemos = new HashSet<CustomerCustomerDemo>();
            Orders = new HashSet<Order>();
        }

        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Salt { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool? Active { get; set; }
        public string Avatar { get; set; }
        public int? Ward { get; set; }
        public int? LocationId { get; set; }
        public int? District { get; set; }

        public virtual ICollection<CustomerCustomerDemo> CustomerCustomerDemos { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
