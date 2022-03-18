using LPUQa.MODEL.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPUQa.MODEL.QuestionModel
{
    public class QuestionModel : CommonpropertiesModel
    {
        public string PostName { get; set; }
        public string ShareType { get; set; }
        public string PostDescription { get; set; }
    }
    public class GlobalQuestionModel :  QuestionModel    
    {
        public string PostimagePath { get; set; }
        public string AnswerPath { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public int ShareCount { get; set; }
        public bool Ishidden { get; set; }
        public bool IsDeleted { get; set; }
        public string ReasonForRemove { get; set; }
        public Guid PostId{ get; set; }
    }
    public class ShareTypeModel
    {
        public string ShareType { get; set; }
        public string ShareUID { get; set; }
    }
}
