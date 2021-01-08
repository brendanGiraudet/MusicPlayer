namespace MusicPlayerApplication.Models
{
    public class ResponseModel<T>
    {
        public string ErrorMessage { get; set; }
        public bool HasError { get; set; }
        public T Content { get; set; }
    }
}
