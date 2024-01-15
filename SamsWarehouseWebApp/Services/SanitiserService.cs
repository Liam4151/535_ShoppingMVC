using Ganss.Xss;

namespace SamsWarehouseWebApp.Services
{
    public class SanitiserService
    {
        public HtmlSanitizer Sanitiser { get; set; }

        public SanitiserService()
        {
            if(Sanitiser == null)
            {
                Sanitiser = new HtmlSanitizer();
                Sanitiser.AllowDataAttributes = true;
                Sanitiser.AllowedAttributes.Add("class");
            }
        }
    }
}
