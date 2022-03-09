using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace GenerateCodle;

public class Function
{
    
    /// <summary>
    /// Generates new Codle Boolean Statement And Returns it.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    
    public object FunctionHandler(ILambdaContext context)
    {
        return new { puzzle = GenerateCodle.Generator.GenerateAnswer() };
    }
}
