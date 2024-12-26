namespace CodeDetection
{
    public class ResponseModel
    {
        public string Message { get; set; } // Komunikat zwrotny
        public int StatusCode { get; set; } // Kod statusu HTTP
        public DateTime Timestamp { get; set; } // Znacznik czasu
    }
}
