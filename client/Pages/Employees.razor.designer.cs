using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using Ems.Models.Emsdb;
using Ems.Client.Pages;

namespace Ems.Pages
{
    public partial class EmployeesComponent : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected EmsdbService Emsdb { get; set; }
        protected RadzenDataGrid<Ems.Models.Emsdb.Employee> grid0;

        string _search;
        protected string search
        {
            get
            {
                return _search;
            }
            set
            {
                if (!object.Equals(_search, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "search", NewValue = value, OldValue = _search };
                    _search = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<Ems.Models.Emsdb.Employee> _getEmployeesResult;
        protected IEnumerable<Ems.Models.Emsdb.Employee> getEmployeesResult
        {
            get
            {
                return _getEmployeesResult;
            }
            set
            {
                if (!object.Equals(_getEmployeesResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getEmployeesResult", NewValue = value, OldValue = _getEmployeesResult };
                    _getEmployeesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        int _getEmployeesCount;
        protected int getEmployeesCount
        {
            get
            {
                return _getEmployeesCount;
            }
            set
            {
                if (!object.Equals(_getEmployeesCount, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getEmployeesCount", NewValue = value, OldValue = _getEmployeesCount };
                    _getEmployeesCount = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            await Load();
        }
        protected async System.Threading.Tasks.Task Load()
        {
            if (string.IsNullOrEmpty(search)) {
                search = "";
            }
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddEmployee>("Add Employee", null);
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid0LoadData(LoadDataArgs args)
        {
            try
            {
                var emsdbGetEmployeesResult = await Emsdb.GetEmployees(filter:$@"(contains(FirstName,""{search}"") or contains(LastName,""{search}"") or contains(Email,""{search}"") or contains(Department,""{search}"")) and {(string.IsNullOrEmpty(args.Filter)? "true" : args.Filter)}", orderby:$"{args.OrderBy}", top:args.Top, skip:args.Skip, count:args.Top != null && args.Skip != null);
                getEmployeesResult = emsdbGetEmployeesResult.Value.AsODataEnumerable();

                getEmployeesCount = emsdbGetEmployeesResult.Count;
            }
            catch (System.Exception emsdbGetEmployeesException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to load Employees" });
            }
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(Ems.Models.Emsdb.Employee args)
        {
            var dialogResult = await DialogService.OpenAsync<EditEmployee>("Edit Employee", new Dictionary<string, object>() { {"Id", args.Id} });
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var emsdbDeleteEmployeeResult = await Emsdb.DeleteEmployee(id:data.Id);
                    if (emsdbDeleteEmployeeResult != null && emsdbDeleteEmployeeResult.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        await grid0.Reload();
                    }

                    if (emsdbDeleteEmployeeResult != null && emsdbDeleteEmployeeResult.StatusCode != System.Net.HttpStatusCode.NoContent)
                    {
                        NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete Employee" });
                    }
                }
            }
            catch (System.Exception emsdbDeleteEmployeeException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete Employee" });
            }
        }
    }
}
