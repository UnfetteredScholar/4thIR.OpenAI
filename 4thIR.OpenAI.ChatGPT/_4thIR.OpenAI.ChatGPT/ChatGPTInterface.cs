using OpenAI;

namespace _4thIR.OpenAI
{
    public enum Engine { Ada, Babbage, Curie,Davinci}
    public class ChatGPTInterface
    {
        public ChatGPTInterface(string api)
        {
            this.OpenAIClient = new OpenAIClient(new OpenAIAuthentication(api));

        }
        public OpenAIClient OpenAIClient { get; set; }

    }
}
