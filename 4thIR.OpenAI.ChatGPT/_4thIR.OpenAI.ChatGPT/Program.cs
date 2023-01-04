using System;
using OpenAI;

namespace ChatGPT_Test
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            OpenAIClient client = new OpenAIClient(new OpenAIAuthentication("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"), Engine.Davinci);

            CompletionRequest completionRequest = new CompletionRequest();

            completionRequest.Prompt = "Elon Musk is";
            completionRequest.Temperature = 0.7;
            completionRequest.MaxTokens = 256;
            completionRequest.TopP = 1;
            completionRequest.FrequencyPenalty = 0;
            completionRequest.PresencePenalty = 0;
            completionRequest.BestOf = 1;

            var res = await client.CompletionEndpoint.CreateCompletionAsync(completionRequest,Engine.Davinci);
           // var res = await client.CompletionEndpoint.CreateCompletionAsync(new CompletionRequest("An example of a Hello World program in C++", temperature: 0.5, max_tokens:256,top_p:1, frequencyPenalty:0, presencePenalty:0, bestOf:1));
            //var res = await client.CompletionEndpoint.CreateCompletionAsync(new CompletionRequest(prompt: "An example of a Hello World program in C++", temperature: 0.5, max_tokens:50,top_p:1, frequencyPenalty:0, presencePenalty:0, bestOf:1));

            Console.WriteLine(res.ToString());

        }
    }
}