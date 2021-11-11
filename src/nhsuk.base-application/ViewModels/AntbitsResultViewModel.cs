using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace nhsuk.base_application.ViewModels
{
    public sealed class AntbitsResultViewModel
    {
        public AntbitsResultViewModel()
        {
            accumulatedValues = new Dictionary<string, int>();
            results = new List<string>();
        }
        public Dictionary<string, int> accumulatedValues { get; set; } 

        public List<string> results { get; set; }
    }
}