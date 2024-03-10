using System;
using System.Collections.Generic;

namespace WemaCustomer.Application.Data.Models
{
    public partial class State
    {
        public State()
        {
            LocalGovernments = new HashSet<LocalGovernment>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<LocalGovernment> LocalGovernments { get; set; }
    }
}
