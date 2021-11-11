using Microsoft.AspNetCore.Html;

namespace nhsuk.base_application.ViewModels
{
    public sealed class AntbitsIntroViewModel
    {
        public HtmlString Intro_title {get; set; }
        public HtmlString Intro_copy { get; set; }
    }
}
