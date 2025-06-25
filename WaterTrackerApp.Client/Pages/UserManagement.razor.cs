using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using WaterTrackerApp.Client.Dtos;
using WaterTrackerApp.Client.Pages.Dialogs;
using WaterTrackerApp.Client.Services;

namespace WaterTrackerApp.Client.Pages
{
    public partial class UserManagement
    {
        [Inject] private UserService UserService { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        private List<UserDto>? Users;

        //table
        private bool _loading;

        protected override async Task OnInitializedAsync()
        {
            _loading = true;
            Users = await UserService.GetAllUsersAsync();
            _loading = false;
        }

        private void NavToWaterIntake(int userId)
        {
            NavigationManager.NavigateTo($"/waterintake/{userId}");
        }
        private async Task ShowCreateDialog()
        {
            var parameters = new DialogParameters<Dialogs.UserDialog>
            {
                { x => x.User, new UserDto() },
                { x => x.ButtonText, "Save" }
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
            var dialog = await DialogService.ShowAsync<Dialogs.UserDialog>("Add User", parameters, options);
            var result = await dialog.Result;

            if (!result.Canceled && result.Data is UserDto created)
            {
                var newUser = await UserService.CreateUserAsync(created);
                if (newUser != null && newUser.Id != 0)
                {
                    Users?.Add(newUser);
                    Snackbar.Add("User created successfully!", Severity.Success);
                    StateHasChanged();
                }
                else
                {
                    Snackbar.Add("Failed to create user.", Severity.Error);
                }
            }
        }

        private async Task ShowEditDialog(UserDto user)
        {
            var parameters = new DialogParameters<Dialogs.UserDialog>
            {
                { x => x.User, new UserDto
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        Surname = user.Surname,
                        Email = user.Email
                    }
                },
                { x => x.ButtonText, "Save" }
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
            var dialog = await DialogService.ShowAsync<Dialogs.UserDialog>("Edit User", parameters, options);
            var result = await dialog.Result;

            if (!result.Canceled && result.Data is UserDto updated)
            {
                var success = await UserService.UpdateUserAsync(updated.Id, updated);
                if (success)
                {
                    var existing = Users?.FirstOrDefault(u => u.Id == updated.Id);
                    if (existing != null)
                    {
                        existing.FirstName = updated.FirstName;
                        existing.Surname = updated.Surname;
                        existing.Email = updated.Email;
                        Snackbar.Add("User updated successfully!", Severity.Success);
                        StateHasChanged();
                    }
                }
                else
                {
                    Snackbar.Add("Failed to update user.", Severity.Error);
                }
            }
        }
        private async Task DeleteUser(int id)
        {
            bool? confirmed = await DialogService.ShowMessageBox(
                "Confirm Delete",
                "Are you sure you want to delete this user?",
                yesText: "Yes", cancelText: "No");

            if (confirmed == true)
            {
                var success = await UserService.DeleteUserAsync(id);
                if (success)
                {
                    Users = Users?.Where(u => u.Id != id).ToList();
                    Snackbar.Add("User deleted successfully!", Severity.Success);
                    StateHasChanged();
                }
            }
        }
    }
}
