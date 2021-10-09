using Microsoft.AspNetCore.Mvc;

namespace tech.haamu.Movie
{
    public static class ControllerBaseExtension
    {
        public static string GetUserId(this ControllerBase controller)
        {
            return controller.User.FindFirst("userId")?.Value;
        }
    }
}
