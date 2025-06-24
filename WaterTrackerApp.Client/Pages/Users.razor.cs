using WaterTrackerApp.Client.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace WaterTrackerApp.Client.Pages
{
    public partial class Users
    {
        private List<UserDto>? users;
        private bool showDialog = false;
        private UserDto editingUser = new();
        private string? notification;

        protected override async Task OnInitializedAsync()
        {
            await LoadUsers();
        }

        private async Task LoadUsers()
        {
            users = await UserService.GetAllUsersAsync();
        }

        private void ShowCreateDialog()
        {
            editingUser = new UserDto();
            showDialog = true;
        }

        private void ShowEditDialog(UserDto user)
        {
            editingUser = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                Surname = user.Surname,
                Email = user.Email
            };
            showDialog = true;
        }

        private void CloseDialog()
        {
            showDialog = false;
        }

        private async Task SaveUser()
        {
            if (editingUser.Id == 0)
            {
                var created = await UserService.CreateUserAsync(editingUser);
                if (created != null)
                {
                    users?.Add(created);
                    notification = "User created successfully!";
                }
            }
            else
            {
                var success = await UserService.UpdateUserAsync(editingUser.Id, editingUser);
                if (success)
                {
                    var user = users?.FirstOrDefault(u => u.Id == editingUser.Id);
                    if (user != null)
                    {
                        user.FirstName = editingUser.FirstName;
                        user.Surname = editingUser.Surname;
                        user.Email = editingUser.Email;
                    }
                    notification = "User updated successfully!";
                }
            }
            showDialog = false;
            StateHasChanged();
        }

        private async Task DeleteUser(int id)
        {
            var confirmed = await JS.InvokeAsync<bool>("confirm", "Are you sure you want to delete this user?");
            if (confirmed)
            {
                var success = await UserService.DeleteUserAsync(id);
                if (success)
                {
                    users = users?.Where(u => u.Id != id).ToList();
                    notification = "User deleted successfully!";
                    StateHasChanged();
                }
            }
        }

        [Inject]
        private IJSRuntime JS { get; set; } = default!;
    }
}
