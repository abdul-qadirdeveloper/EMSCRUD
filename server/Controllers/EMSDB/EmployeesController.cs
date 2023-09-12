using System;
using System.Net;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;




namespace Ems.Controllers.Emsdb
{
  using Models;
  using Data;
  using Models.Emsdb;

  [Route("odata/EMSDB/Employees")]
  public partial class EmployeesController : ODataController
  {
    private Ems.Data.EmsdbContext context;

    public EmployeesController(Ems.Data.EmsdbContext context)
    {
      this.context = context;
    }
    // GET /odata/Emsdb/Employees
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    [HttpGet]
    public IEnumerable<Models.Emsdb.Employee> GetEmployees()
    {
      var items = this.context.Employees.AsQueryable<Models.Emsdb.Employee>();
      this.OnEmployeesRead(ref items);

      return items;
    }

    partial void OnEmployeesRead(ref IQueryable<Models.Emsdb.Employee> items);

    partial void OnEmployeeGet(ref SingleResult<Models.Emsdb.Employee> item);

    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    [HttpGet("/odata/EMSDB/Employees(Id={Id})")]
    public SingleResult<Employee> GetEmployee(int key)
    {
        var items = this.context.Employees.Where(i=>i.Id == key);
        var result = SingleResult.Create(items);

        OnEmployeeGet(ref result);

        return result;
    }
    partial void OnEmployeeDeleted(Models.Emsdb.Employee item);
    partial void OnAfterEmployeeDeleted(Models.Emsdb.Employee item);

    [HttpDelete("/odata/EMSDB/Employees(Id={Id})")]
    public IActionResult DeleteEmployee(int key)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var item = this.context.Employees
                .Where(i => i.Id == key)
                .FirstOrDefault();

            if (item == null)
            {
                return BadRequest();
            }

            this.OnEmployeeDeleted(item);
            this.context.Employees.Remove(item);
            this.context.SaveChanges();
            this.OnAfterEmployeeDeleted(item);

            return new NoContentResult();
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    partial void OnEmployeeUpdated(Models.Emsdb.Employee item);
    partial void OnAfterEmployeeUpdated(Models.Emsdb.Employee item);

    [HttpPut("/odata/EMSDB/Employees(Id={Id})")]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    public IActionResult PutEmployee(int key, [FromBody]Models.Emsdb.Employee newItem)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (newItem == null || (newItem.Id != key))
            {
                return BadRequest();
            }

            this.OnEmployeeUpdated(newItem);
            this.context.Employees.Update(newItem);
            this.context.SaveChanges();

            var itemToReturn = this.context.Employees.Where(i => i.Id == key);
            this.OnAfterEmployeeUpdated(newItem);
            return new ObjectResult(SingleResult.Create(itemToReturn));
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    [HttpPatch("/odata/EMSDB/Employees(Id={Id})")]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    public IActionResult PatchEmployee(int key, [FromBody]Delta<Models.Emsdb.Employee> patch)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = this.context.Employees.Where(i => i.Id == key).FirstOrDefault();

            if (item == null)
            {
                return BadRequest();
            }

            patch.Patch(item);

            this.OnEmployeeUpdated(item);
            this.context.Employees.Update(item);
            this.context.SaveChanges();

            var itemToReturn = this.context.Employees.Where(i => i.Id == key);
            return new ObjectResult(SingleResult.Create(itemToReturn));
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }

    partial void OnEmployeeCreated(Models.Emsdb.Employee item);
    partial void OnAfterEmployeeCreated(Models.Emsdb.Employee item);

    [HttpPost]
    [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
    public IActionResult Post([FromBody] Models.Emsdb.Employee item)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (item == null)
            {
                return BadRequest();
            }

            this.OnEmployeeCreated(item);
            this.context.Employees.Add(item);
            this.context.SaveChanges();

        
            this.OnAfterEmployeeCreated(item);
            
            return Created($"odata/Emsdb/Employees/{item.Id}", item);
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return BadRequest(ModelState);
        }
    }
  }
}
