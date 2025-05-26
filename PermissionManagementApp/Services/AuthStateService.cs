using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace PermissionManagementApp.Services
{
    public class AuthStateService
    {
        private readonly ProtectedSessionStorage storage;

        public bool IsLoggedIn { get; set; } = false;
        public string? UserName { get; set; } = null;
        public int? UserId { get; set; } = null;

        public event Action? OnAuthStateChanged;

        public AuthStateService(ProtectedSessionStorage storage)
        {
            this.storage = storage;
        }

        public async Task LoginAsync(string username, int userId)
        {
            IsLoggedIn = true;
            UserName = username;
            UserId = userId;

            await storage.SetAsync(nameof(UserName), username);
            await storage.SetAsync(nameof(UserId), userId);

            OnStateChanged();
        }

        public async Task Logout()
        {
            IsLoggedIn = false;
            UserName = null;
            UserId = null;

            await storage.DeleteAsync(nameof(UserName));
            await storage.DeleteAsync(nameof(UserId));

            OnStateChanged();
        }

        public async Task InitializeAuthState()
        {
            var userName = await storage.GetAsync<string>(nameof(UserName));
            var userId = await storage.GetAsync<int>(nameof(UserId));

            if(userId.Success && userName.Success)
            {
                IsLoggedIn = true;
                UserName = userName.Value;
                UserId = userId.Value;

                OnStateChanged();
            }
        }

        private void OnStateChanged()
        {
            OnAuthStateChanged?.Invoke();
        }
    }
}
