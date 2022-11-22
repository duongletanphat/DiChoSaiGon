using System;
using System.Collections.Generic;

#nullable disable

namespace DiChoSaiGon.Models
{
    public partial class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int UnitPrice { get; set; }
        public short Quantity { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? Amount { get; set; }
        public int? TotalMoney { get; set; }
        public int? Discount { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
