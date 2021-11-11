using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace nhsuk.base_application.ViewModels
{
    public sealed class AntbitsQuestionViewModel
    {
        public string body { get; set; }

        public List<Dictionary<string, string>> answers { get; set; }

        public string answer { get; set; }
    }
}