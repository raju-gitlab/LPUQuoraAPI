using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPUQa.MODEL.Master
{
    public class CommonModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class GenderModel
    {
        public string Gender { get; set; }
        public string GenderUId { get; set; }
    }
    public class CommonpropertiesModel : CommonModel
    {
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }
    }
}
