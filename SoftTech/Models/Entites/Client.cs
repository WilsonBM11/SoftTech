using System;
using System.Collections.Generic;

namespace SoftTech.Models.Entites;

public partial class Client
{
    public string id { get; set; } = null!;

    public string name { get; set; } = null!;

    public string? address { get; set; }

    public string? phone_number { get; set; }

    public string email { get; set; } = null!;

    public string? profile_picture { get; set; }
}
