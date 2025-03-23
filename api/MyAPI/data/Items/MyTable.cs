using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyAPI.data.Items; 

public class MyTable
{
    public long Id;
    public List<MyTableItem> Items { get; } = [];
    public string Title { get; set; } = "";
}
