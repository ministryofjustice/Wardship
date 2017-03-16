
namespace Wardship.Models
{
    public class ErrorModel
    {
        public ErrorModel() { }         //empty constructor
        public ErrorModel(int errcode)  //error code constructor
        {
            ErrorCode = errcode;
            switch (errcode)
            {
                case 1:
                    ErrorMessage = "File not returned from wrd.SCDCollectionXML";
                    break;
                case 2:
                    ErrorMessage = "Unspecified error in SCD26CollectionXML";
                    break;
                default:
                    ErrorCode = -1;
                    break;
            }
        }

        public string ErrorMessage { get; set; }
        public int ErrorCode { get; private set; }
    }
}
