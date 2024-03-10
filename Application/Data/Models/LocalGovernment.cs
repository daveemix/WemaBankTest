using System;
using System.Collections.Generic;

namespace WemaCustomer.Application.Data.Models
{
    public partial class LocalGovernment
    {
        public int Id { get; set; }
        public int StateId { get; set; }
        public string Name { get; set; } = null!;

        public virtual State State { get; set; } = null!;
    }
}
