using SoftServe.ITAcademy.BackendDubbingProject.Administration.Core.DTOs;
using System.Collections.Generic;

namespace SoftServe.ITAcademy.BackendDubbingProject.WebApiTest
{
    public static class DullData
    {
        public static List<AudioDTO> GetAllAudioDTOs()
        {
            return new List<AudioDTO>()
            {
                new AudioDTO { Id = 1, FileName = "File1", OriginalText = "Text1", SpeechId = 1, LanguageId = 1},
                new AudioDTO { Id = 2, FileName = "File2", OriginalText = "Text2", SpeechId = 2, LanguageId = 2},
                new AudioDTO { Id = 3, FileName = "File3", OriginalText = "Text3", SpeechId = 3, LanguageId = 3},
                new AudioDTO { Id = 4, FileName = "File4", OriginalText = "Text4", SpeechId = 4, LanguageId = 4},
                new AudioDTO { Id = 5, FileName = "File5", OriginalText = "Text5", SpeechId = 5, LanguageId = 5},
            };
        }

        public static List<LanguageDTO> GetAllLanguageDTOs()
        {
            return new List<LanguageDTO>()
            {
                new LanguageDTO { Id = 1, Name = "English" },
                new LanguageDTO { Id = 2, Name = "Polish" },
                new LanguageDTO { Id = 3, Name = "Spanish" },
            };
        }

        public static List<PerformanceDTO> GetAllPerfomanceDTOs()
        {
            return new List<PerformanceDTO>()
            {
                new PerformanceDTO { Id = 1, Title = "Performance1", Description = "Description of performance1" },
                new PerformanceDTO { Id = 2, Title = "Performance2", Description = "Description of performance2" },
                new PerformanceDTO { Id = 3, Title = "Performance3", Description = "Description of performance3" },
            };
        }

        public static List<SpeechDTO> GetAllSpeechDTOs()
        {
            return new List<SpeechDTO>()
            {
                new SpeechDTO { Id = 1, Order = 1, Text = "Speech1", Duration = 120, PerformanceId = 1 },
                new SpeechDTO { Id = 2, Order = 2, Text = "Speech2", Duration = 120, PerformanceId = 1 },
                new SpeechDTO { Id = 3, Order = 3, Text = "Speech3", Duration = 120, PerformanceId = 1 },
            };
        }
    }
}
