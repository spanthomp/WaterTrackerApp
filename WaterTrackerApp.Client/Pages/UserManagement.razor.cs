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
        private async Task<UserDto?> ShowUserDialog(string title, UserDto user, string buttonText)
        {
            var parameters = new DialogParameters<UserDialog>
            {
                { x => x.User, user },
                { x => x.ButtonText, buttonText }
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
            var dialog = await DialogService.ShowAsync<UserDialog>(title, parameters, options);
            var result = await dialog.Result;

            return !result.Canceled && result.Data is UserDto dto ? dto : null;
        }

        private async Task ShowCreateDialog()
        {
            var newUser = await ShowUserDialog("Add User", new UserDto(), "Save");
            if (newUser == null) return;

            var createdUser = await UserService.CreateUserAsync(newUser);
            if (createdUser != null && createdUser.Id != 0)
            {
                Users?.Add(createdUser);
                Snackbar.Add("User created successfully!", Severity.Success);
                StateHasChanged();
            }
            else
            {
                Snackbar.Add("Failed to create user.", Severity.Error);
            }
        }

        private async Task ShowEditDialog(UserDto user)
        {
            var updatedUser = await ShowUserDialog("Edit User", new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                Surname = user.Surname,
                Email = user.Email
            }, "Save");

            if (updatedUser == null) return;

            var success = await UserService.UpdateUserAsync(updatedUser.Id, updatedUser);
            if (success)
            {
                var existing = Users?.FirstOrDefault(u => u.Id == updatedUser.Id);
                if (existing != null)
                {
                    existing.FirstName = updatedUser.FirstName;
                    existing.Surname = updatedUser.Surname;
                    existing.Email = updatedUser.Email;
                }

                Snackbar.Add("User updated successfully!", Severity.Success);
                StateHasChanged();
            }
            else
            {
                Snackbar.Add("Failed to update user.", Severity.Error);
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