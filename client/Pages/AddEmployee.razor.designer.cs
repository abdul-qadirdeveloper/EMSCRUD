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
    public partial class AddEmployeeComponent : ComponentBase
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

        Ems.Models.Emsdb.Employee _employee;
        protected Ems.Models.Emsdb.Employee employee
        {
            get
            {
                return _employee;
            }
            set
            {
                if (!object.Equals(_employee, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "employee", NewValue = value, OldValue = _employee };
                    _employee = value;
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
            employee = new Ems.Models.Emsdb.Employee(){};
        }

        protected async System.Threading.Tasks.Task Form0Submit(Ems.Models.Emsdb.Employee args)
        {
            try
            {
                var emsdbCreateEmployeeResult = await Emsdb.CreateEmployee(employee:employee);
                DialogService.Close(employee);
            }
            catch (System.Exception emsdbCreateEmployeeException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to create new Employee!" });
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
