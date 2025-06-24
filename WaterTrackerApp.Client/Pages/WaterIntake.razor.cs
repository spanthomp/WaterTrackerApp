using WaterTrackerApp.Client.Dtos;
using WaterTrackerApp.Infrastructure.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace WaterTrackerApp.Client.Pages
{
    public partial class WaterIntake
    {
        [Parameter] public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; }
        public string FullName => string.IsNullOrWhiteSpace(LastName) ? FirstName : $"{FirstName} {LastName}";

        private List<WaterIntakeDto>? records;
        private bool showDialog = false;
        private WaterIntakeDto editingRecord = new();
        private string? notification;

        protected override async Task OnInitializedAsync()
        {
            await LoadRecords();
        }

        private async Task LoadRecords()
        {
            records = await WaterIntakeService.GetByUserIdAsync(UserId);
        }

        private void ShowCreateDialog()
        {
            editingRecord = new WaterIntakeDto { UserId = UserId, IntakeDate = DateTime.Today };
            showDialog = true;
        }

        private void ShowEditDialog(WaterIntakeDto record)
        {
            editingRecord = new WaterIntakeDto
            {
                Id = record.Id,
                UserId = record.UserId,
                IntakeDate = record.IntakeDate,
                ConsumedWater = record.ConsumedWater
            };
            showDialog = true;
        }

        private void CloseDialog()
        {
            showDialog = false;
        }

        private async Task SaveRecord()
        {
            if (editingRecord.Id == 0)
            {
                var created = await WaterIntakeService.CreateAsync(editingRecord);
                if (created != null)
                {
                    records?.Add(created);
                    notification = "Water intake record created successfully!";
                }
            }
            else
            {
                var success = await WaterIntakeService.UpdateAsync(editingRecord.Id, editingRecord);
                if (success)
                {
                    var rec = records?.FirstOrDefault(r => r.Id == editingRecord.Id);
                    if (rec != null)
                    {
                        rec.IntakeDate = editingRecord.IntakeDate;
                        rec.ConsumedWater = editingRecord.ConsumedWater;
                    }
                    notification = "Water intake record updated successfully!";
                }
            }
            showDialog = false;
            StateHasChanged();
        }

        private async Task DeleteRecord(int id)
        {
            var confirmed = await JS.InvokeAsync<bool>("confirm", "Are you sure you want to delete this record?");
            if (confirmed)
            {
                var success = await WaterIntakeService.DeleteAsync(id);
                if (success)
                {
                    records = records?.Where(r => r.Id != id).ToList();
                    notification = "Water intake record deleted successfully!";
                    StateHasChanged();
                }
            }
        }

        [Inject]
        private IJSRuntime JS { get; set; } = default!;
    }
}
