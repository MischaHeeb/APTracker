using Hangfire.Dashboard;

namespace APTrackerAPI.Filters
{
    public class HangfireAuthFilter : IDashboardAuthorizationFilter
    {
        private readonly bool _inDevelopment;

        public HangfireAuthFilter(bool inDevelopment = false)
        {
            _inDevelopment = inDevelopment;
        }

        public bool Authorize(DashboardContext context)
        {
            if (_inDevelopment)
            {
                return true;
            }
        
            var httpContext = context.GetHttpContext();
            return httpContext.User.Identity?.IsAuthenticated == true && httpContext.User.IsInRole("Admin");
        }
    }
}

