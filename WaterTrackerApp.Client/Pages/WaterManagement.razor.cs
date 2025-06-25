using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using WaterTrackerApp.Client.Dtos;
using WaterTrackerApp.Client.Services;

namespace WaterTrackerApp.Client.Pages
{
    public partial class WaterManagement
    {
        [Inject] private WaterIntakeService WaterIntakeService { get; set; } = default!;
        [Inject] private UserService UserService { get; set; } = default!;
        [Parameter] public int UserId { get; set; }
        private List<WaterIntakeDto>? WaterIntake;
        public string FirstName { get; set; } = string.Empty;
        public string? Surname { get; set; }

        public string UserName => string.IsNullOrWhiteSpace(Surname) ? FirstName : $"{FirstName} {Surname}";

        //table
        private bool _loading;

        protected override async Task OnInitializedAsync()
        {
            _loading = true;
            var user = await UserService.GetUserByIdAsync(UserId);
            if (user != null)
            {
                FirstName = user.FirstName;
                Surname = user.Surname;
            }
            WaterIntake = await WaterIntakeService.GetByUserIdAsync(UserId); 
            _loading = false;
        }
        private async Task ShowCreateDialog()
        {
            var parameters = new DialogParameters<Dialogs.WaterIntakeDialog>
            {
                { x => x.WaterIntake, new WaterIntakeDto { UserId = UserId, IntakeDate = DateTime.Today } },
                { x => x.ButtonText, "Save" }
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
            var dialog = await DialogService.ShowAsync<Dialogs.WaterIntakeDialog>("Add Water Intake", parameters, options);
            var result = await dialog.Result;

            if (!result.Canceled && result.Data is WaterIntakeDto created)
            {
                var newRecord = await WaterIntakeService.CreateAsync(created);
                if (newRecord != null && newRecord.Id != 0)
                {
                    WaterIntake?.Add(newRecord);
                    Snackbar.Add("Water intake record created successfully!", Severity.Success);
                    StateHasChanged();
                }
                else
                {
                    Snackbar.Add("Failed to create water intake record.", Severity.Error);
                }
            }
        }
        private async Task ShowEditDialog(WaterIntakeDto record)
        {
            var parameters = new DialogParameters<Dialogs.WaterIntakeDialog>
            {
                { x => x.WaterIntake, new WaterIntakeDto
                    {
                        Id = record.Id,
                        UserId = record.UserId,
                        IntakeDate = record.IntakeDate,
                        ConsumedWater = record.ConsumedWater
                    }
                },
                { x => x.ButtonText, "Save" }
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
            var dialog = await DialogService.ShowAsync<Dialogs.WaterIntakeDialog>("Edit Water Intake", parameters, options);
            var result = await dialog.Result;

            if (!result.Canceled && result.Data is WaterIntakeDto updated)
            {
                var success = await WaterIntakeService.UpdateAsync(updated.Id, updated);
                if (success)
                {
                    var existing = WaterIntake?.FirstOrDefault(r => r.Id == updated.Id);
                    if (existing != null)
                    {
                        existing.IntakeDate = updated.IntakeDate;
                        existing.ConsumedWater = updated.ConsumedWater;
                        Snackbar.Add("Water intake record updated successfully!", Severity.Success);
                        StateHasChanged();
                    }
                }
                else
                {
                    Snackbar.Add("Failed to update water intake record.", Severity.Error);
                }
            }
        }
        private async Task DeleteRecord(int id)
        {
            bool? confirmed = await DialogService.ShowMessageBox(
                "Confirm Delete",
                "Are you sure you want to delete this record?",
                yesText: "Yes", cancelText: "No");

            if (confirmed == true)
            {
                var success = await WaterIntakeService.DeleteAsync(id);
                if (success)
                {
                    WaterIntake = WaterIntake?.Where(r => r.Id != id).ToList();
                    Snackbar.Add("Water intake record deleted successfully!", Severity.Success);
                    StateHasChanged();
                }
            }
        }
    }
}
