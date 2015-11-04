using Common;

namespace Mentor
{
    public class RegistrationEmail : Email
    {
        public Agency Agency { get; set; }
        public string BaseUrl { get; set; }

        public override string Subject
        {
            get { return "[Mentor] Agency Registration"; }
        }

        public override string Template
        {
            get
            {
                return @"
{{Agency.Name}}
{{BaseUrl}}/Agency/EditAgency/{{Agency.Id}}
                ";
            }
        }

        public override object Data
        {
            get { return this; }
        }
    };
}