using OpenAI;

namespace _4thIR.OpenAI
{
    public enum Engine { Ada, Babbage, Curie,Davinci}
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
