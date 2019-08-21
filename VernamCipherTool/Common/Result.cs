using VernamCipherTool.Enums;
using VernamCipherTool.Extensions;

public class Result
{
    public Result(string message, Operation op)
    {
        ResultMessage = message;
        Type = op.GetDescription();
    }

    public string ResultMessage { get; set; }
    public string Type { get; set; }
}