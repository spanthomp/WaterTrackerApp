using Microsoft.AspNetCore.Components;
using MudBlazor;
using WaterTrackerApp.Client.Dtos;
using WaterTrackerApp.Domain.Entities;

namespace WaterTrackerApp.Client.Pages.Dialogs
{
    public partial class UserDialog
    {
        [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = default!;
        [Parameter] public UserDto User { get; set; } = new();
        [Parameter] public string ButtonText { get; set; } = "Save";
        [Parameter] public EventCallback<UserDto> OnValidSubmit { get; set; }

        private MudForm? form;

        private async Task Submit()
        {
            if (form != null)
            {
                await form.Validate();
                if (form.IsValid)
                {
                    MudDialog.Close(DialogResult.Ok(User));
                }
            }
        }
        private void Cancel() => MudDialog.Cancel();

        private async Task HandleValidSubmit()
        {
            await OnValidSubmit.InvokeAsync(User);
        }
    }
}
