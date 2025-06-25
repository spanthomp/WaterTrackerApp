using Microsoft.AspNetCore.Components;
using MudBlazor;
using WaterTrackerApp.Client.Dtos;

namespace WaterTrackerApp.Client.Pages.Dialogs
{
    public partial class WaterIntakeDialog
    {
        [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = default!;
        [Parameter] public WaterIntakeDto WaterIntake { get; set; } = new();
        [Parameter] public string ButtonText { get; set; } = "Save";
        [Parameter] public EventCallback<WaterIntakeDto> OnValidSubmit { get; set; }
        [Parameter] public EventCallback OnCancel { get; set; }

        private MudForm? form;

        private async Task Submit()
        {
            if (form != null)
            {
                await form.Validate();
                if (form.IsValid)
                {
                    MudDialog.Close(DialogResult.Ok(WaterIntake));
                }
            }
        }
        private void Cancel() => MudDialog.Cancel();

        private async Task HandleValidSubmit()
        {
            await OnValidSubmit.InvokeAsync(WaterIntake);
        }
    }
}
