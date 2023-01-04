using OpenAI;

namespace _4thIR.OpenAI
{
    public class ChatGPTInterface
    {
        public ChatGPTInterface()
        {
            this.OpenAIClient = new OpenAIClient(new OpenAIAuthentication(API_KEY));
        }

        private const string API_KEY = "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";
        public OpenAIClient OpenAIClient { get; set; }


    }
}
