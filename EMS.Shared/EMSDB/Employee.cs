using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ems.Models.Emsdb
{
  [Table("Employee", Schema = "dbo")]
  public partial class Employee
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id
    {
      get;
      set;
    }
    public string FirstName
    {
      get;
      set;
    }
    public string LastName
    {
      get;
      set;
    }
    public string Email
    {
      get;
      set;
    }
    public DateTime DateOfBirth
    {
      get;
      set;
    }
    public string Department
    {
      get;
      set;
    }
  }
}
