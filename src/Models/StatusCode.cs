using System.ComponentModel;

namespace GacsApp.Models;

public enum StatusCode
{
    [Description("Not Set")]
    None = 0,

    [Description("Active")]
    Active = 1,

    [Description("Inactive")]
    Inactive = 2,

    [Description("Waiting for Approval")]
    Pending = 3
}