namespace TFPAW.Web.CoF
{
    public class PdfFormatValidatorHandle : ValidatorHandle 
    {
        public override string Handle(string message, IFormFileCollection? files)
        {
            foreach (IFormFile file in files)
            {
                string extension = Path.GetExtension(file.FileName).ToUpperInvariant();


                if (extension != ".PDF")
                {
                    return "El tipo de archivo no es pdf";
                }

            }
            return base.Handle(message, files);
        }


    }
}
