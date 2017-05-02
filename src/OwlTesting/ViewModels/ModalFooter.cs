using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OwlTesting.ViewModels
{
    public class ModalFooter
    {
        public string SubmitButtonText { get; set; } = "Надіслати";
        public string CancelButtonText { get; set; } = "Відмінити";
        public string SubmitButtonID { get; set; } = "btn-submit";
        public string CancelButtonID { get; set; } = "btn-cancel";
        public bool OnlyCancelButton { get; set; }
    }
}
