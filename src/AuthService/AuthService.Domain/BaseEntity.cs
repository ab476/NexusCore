using System;
using System.Collections.Generic;
using System.Text;

namespace NC.AuthService.Domain;

public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public Guid ConcurrencyStamp { get; set; }
}
