namespace FullFridge.API.Models
{
    public class Result
    {
        public Result(int status, string message = "")
        {
            Status = status;
            Message = message;
        }

        public string Message { get; set; }
        public int Status { get; set; }

        public bool IsSuccess()
        {
            return
                Status == StatusCodes.Status200OK ||
                Status == StatusCodes.Status201Created ||
                Status == StatusCodes.Status202Accepted ||
                Status == StatusCodes.Status204NoContent;
        }
    }
}
