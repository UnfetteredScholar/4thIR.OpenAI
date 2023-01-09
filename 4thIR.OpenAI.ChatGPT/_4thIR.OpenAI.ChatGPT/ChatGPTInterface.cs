using OpenAI.GPT3;
using OpenAI.GPT3.Managers;

namespace _4thIR.OpenAI
{
    public enum Engine { Ada, Babbage, CodeCushmanV1, CodeDavinciV1, CodeDavinciV2, CodeEditDavinciV1 , CodeSearchAdaCodeV1 , CodeSearchAdaTextV1 , CodeSearchBabbageCodeV1 , CodeSearchBabbageTextV1 , Curie , CurieInstructBeta , CurieSimilarityFast , Davinci , DavinciInstructBeta , TextAdaV1 , TextBabbageV1 , TextCurieV1 , TextDavinciV1 , TextDavinciV2, TextDavinciV3, TextEditDavinciV1 , TextSearchAdaDocV1 , TextSearchAdaQueryV1 , TextSearchBabbageDocV1 , TextSearchBabbageQueryV1 , TextSearchCurieDocV1 , TextSearchCurieQueryV1 , TextSearchDavinciDocV1 , TextSearchDavinciQueryV1 , TextSimilarityAdaV1 , TextSimilarityBabbageV1 , TextSimilarityCurieV1 , TextSimilarityDavinciV1 }

    public class ChatGPTInterface
    {
        public ChatGPTInterface()
        {
            this.OpenAIClient = new OpenAIService(new OpenAiOptions()
            {
                ApiKey=API_KEY
            });
        }

        private const string API_KEY = "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";
        public OpenAIService OpenAIClient { get; set; }
    }
}
