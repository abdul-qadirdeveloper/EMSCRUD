using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ems.Data;

namespace Ems
{
    public partial class ExportEmsdbController : ExportController
    {
        private readonly EmsdbContext context;
        public ExportEmsdbController(EmsdbContext context)
        {
            this.context = context;
        }

        [HttpGet("/export/Emsdb/employees/csv")]
        [HttpGet("/export/Emsdb/employees/csv(fileName='{fileName}')")]
        public FileStreamResult ExportEmployeesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(context.Employees, Request.Query), fileName);
        }

        [HttpGet("/export/Emsdb/employees/excel")]
        [HttpGet("/export/Emsdb/employees/excel(fileName='{fileName}')")]
        public FileStreamResult ExportEmployeesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(context.Employees, Request.Query), fileName);
        }
    }
}
