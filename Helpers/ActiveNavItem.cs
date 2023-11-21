using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Smart_Library.Helpers.TagHelpers
{
    [HtmlTargetElement(Attributes = "asp-active")]
    public class ActiveTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ActiveTagHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        [HtmlAttributeName("asp-area")]
        public string? Area { get; set; }
        [HtmlAttributeName("asp-controller")]
        public string? Controller { get; set; }

        [HtmlAttributeName("asp-action")]
        public string? Action { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var currentArea = _httpContextAccessor.HttpContext?.GetRouteValue("area")?.ToString();
            var currentController = _httpContextAccessor.HttpContext?.GetRouteValue("controller")?.ToString();
            var currentAction = _httpContextAccessor.HttpContext?.GetRouteValue("action")?.ToString();
            if (string.Equals(currentArea, Area, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(currentController, Controller, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(currentAction, Action, StringComparison.OrdinalIgnoreCase))
            {
                var existingClasses = output.Attributes["class"]?.Value?.ToString();
                var newClasses = string.IsNullOrEmpty(existingClasses)
                    ? "active"
                    : $"{existingClasses} active";
                output.Attributes.SetAttribute("class", newClasses);
                return;
            }
            if (string.Equals(currentArea, Area, StringComparison.OrdinalIgnoreCase) &&
                     string.Equals(currentController, Controller, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(Action))
            {
                var existingClasses = output.Attributes["class"]?.Value?.ToString();
                var newClasses = string.IsNullOrEmpty(existingClasses)
                    ? "active"
                    : $"{existingClasses} active";
                output.Attributes.SetAttribute("class", newClasses);
                return;
            }
            if (string.Equals(currentAction, Action, StringComparison.OrdinalIgnoreCase) &&
                     string.Equals(currentController, Controller, StringComparison.OrdinalIgnoreCase))
            {
                var existingClasses = output.Attributes["class"]?.Value?.ToString();
                var newClasses = string.IsNullOrEmpty(existingClasses)
                    ? "active"
                    : $"{existingClasses} active";
                output.Attributes.SetAttribute("class", newClasses);
                return;
            }
        }
    }
    [HtmlTargetElement(Attributes = "asp-active-submenu")]
    public class ActiveSubmenuTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ActiveSubmenuTagHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        [HtmlAttributeName("asp-area")]
        public string? Area { get; set; }
        [HtmlAttributeName("asp-controller")]
        public string? Controller { get; set; }

        [HtmlAttributeName("asp-action")]
        public string? Action { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var currentArea = _httpContextAccessor.HttpContext?.GetRouteValue("area")?.ToString();
            var currentController = _httpContextAccessor.HttpContext?.GetRouteValue("controller")?.ToString();
            var currentAction = _httpContextAccessor.HttpContext?.GetRouteValue("action")?.ToString();
            if (string.Equals(currentArea, Area, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(currentController, Controller, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(currentAction, Action, StringComparison.OrdinalIgnoreCase))
            {
                var existingClasses = output.Attributes["class"]?.Value?.ToString();
                var newClasses = string.IsNullOrEmpty(existingClasses)
                    ? "active"
                    : $"{existingClasses} active";
                output.Attributes.SetAttribute("class", newClasses);
                return;
            }

            if (string.Equals(currentAction, Action, StringComparison.OrdinalIgnoreCase) &&
                     string.Equals(currentController, Controller, StringComparison.OrdinalIgnoreCase))
            {
                var existingClasses = output.Attributes["class"]?.Value?.ToString();
                var newClasses = string.IsNullOrEmpty(existingClasses)
                    ? "active"
                    : $"{existingClasses} active";
                output.Attributes.SetAttribute("class", newClasses);
                return;
            }
        }
    }
}