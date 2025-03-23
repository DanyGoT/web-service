using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAPI.data.Items;

public class MyTableItem
{
    public long Id { get; set; }
    public string? Value { get; set; }
}
