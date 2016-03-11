using PhotoBlogApp.Core;
using PhotoBlogApp.Core.Models.Mentions;

namespace PhotoBlogApp.Models.Service
{
    public class MentionService : IMentionService
    {
        private readonly IMentionRepository mentionRepository;

        public MentionService(IMentionRepository mentionRepository)
        {
            this.mentionRepository = mentionRepository;
        }

        public void Add(string fio, string mentionText)
        {
            if (!string.IsNullOrEmpty(mentionText))
            {
                var f = string.IsNullOrEmpty(fio) ? "аноним" : fio;
                var mentionEntity = new Mention
                {
                    Fio = f,
                    Mention1 = mentionText
                };
                mentionRepository.Add(mentionEntity);
            }
        }
    }
}