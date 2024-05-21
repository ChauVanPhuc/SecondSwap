using System;
using System.Collections.Generic;

namespace Second_Swap.Models;

public partial class Feedback
{
    public int Id { get; set; }

    public string? Video { get; set; }

    public string? Comment { get; set; }

    public int? Quality { get; set; }

    public int? OrderId { get; set; }

    public int? Seller { get; set; }

    public virtual Order? Order { get; set; }

    public virtual User? SellerNavigation { get; set; }
}
