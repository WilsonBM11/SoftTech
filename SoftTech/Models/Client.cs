using System;
using System.Collections.Generic;

namespace SoftTech.Models;

public partial class Client
{
    public int id { get; set; }

    public string client_name { get; set; } = null!;

    public string dir { get; set; } = null!;

    public string tel { get; set; } = null!;

    public string email { get; set; } = null!;

    public string? profile_picture { get; set; }
}
