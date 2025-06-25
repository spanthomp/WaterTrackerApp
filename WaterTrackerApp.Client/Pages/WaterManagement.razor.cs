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
        private string UserName = string.Empty;
        private int TotalWaterConsumed;
        private bool _loading;

        protected override async Task OnInitializedAsync()
        {
            _loading = true;
            var user = await UserService.GetUserByIdAsync(UserId);
            if (user != null)
            {
                UserName = string.IsNullOrWhiteSpace(user.Surname) ? user.FirstName : $"{user.FirstName} {user.Surname}";
            }

            WaterIntake = await WaterIntakeService.GetByUserIdAsync(UserId);
            await RefreshTotal();

            _loading = false;
        }
        private async Task RefreshTotal()
        {
            TotalWaterConsumed = await WaterIntakeService.GetTotalConsumedAsync(UserId);
            StateHasChanged();
        }

        private async Task<WaterIntakeDto?> ShowWaterDialog(string title, WaterIntakeDto intake, string buttonText)
        {
            var parameters = new DialogParameters<Dialogs.WaterIntakeDialog>
            {
                { x => x.WaterIntake, intake },
                { x => x.ButtonText, buttonText }
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
            var dialog = await DialogService.ShowAsync<Dialogs.WaterIntakeDialog>(title, parameters, options);
            var result = await dialog.Result;

            return !result.Canceled && result.Data is WaterIntakeDto dto ? dto : null;
        }

        private async Task ShowCreateDialog()
        {
            var dto = await ShowWaterDialog("Add Water Intake", new WaterIntakeDto { UserId = UserId, IntakeDate = DateTime.Today }, "Save");
            if (dto == null) return;

            var newRecord = await WaterIntakeService.CreateAsync(dto);
            if (newRecord != null && newRecord.Id != 0)
            {
                WaterIntake?.Add(newRecord);
                await RefreshTotal();
                Snackbar.Add("Water intake record created successfully!", Severity.Success);
            }
            else
            {
                Snackbar.Add("Failed to create water intake record.", Severity.Error);
            }
        }

        private async Task ShowEditDialog(WaterIntakeDto record)
        {
            var dto = await ShowWaterDialog("Edit Water Intake", new WaterIntakeDto
            {
                Id = record.Id,
                UserId = record.UserId,
                IntakeDate = record.IntakeDate,
                ConsumedWater = record.ConsumedWater
            }, "Save");

            if (dto == null) return;

            var success = await WaterIntakeService.UpdateAsync(dto.Id, dto);
            if (success)
            {
                var existing = WaterIntake?.FirstOrDefault(r => r.Id == dto.Id);
                if (existing != null)
                {
                    existing.IntakeDate = dto.IntakeDate;
                    existing.ConsumedWater = dto.ConsumedWater;
                }

                await RefreshTotal();
                Snackbar.Add("Water intake record updated successfully!", Severity.Success);
            }
            else
            {
                Snackbar.Add("Failed to update water intake record.", Severity.Error);
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
                    await RefreshTotal();
                    Snackbar.Add("Water intake record deleted successfully!", Severity.Success);
                }
            }
        }
    }
}
