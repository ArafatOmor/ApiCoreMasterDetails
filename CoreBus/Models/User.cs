using System;
using System.Collections.Generic;

namespace CoreBus.Models;

public partial class User
{
    public int UserId { get; set; }

    public string EmailId { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string UserName { get; set; } = null!;
}
