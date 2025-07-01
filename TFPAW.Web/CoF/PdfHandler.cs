namespace TFPAW.Web.CoF
{
    public class PdfHandler : ValidatorHandle 
    {
        public override string Handle(string message, IFormFileCollection? files)
        {
            if (files?.Count == 0)
            {
                return "Debe seleccionar al menos un archivo";
            }

            return base.Handle(message, files);

        }
    }
}
