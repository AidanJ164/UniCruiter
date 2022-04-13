using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniCruiter.Models.Identity;

namespace UniCruiter.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public DateTime EnteredOn { get; set; }
        public string Text { get; set; }
        public string EnteredBy
        {
            get
            {
                if (ApplicationUser != null)
                {
                    if (!(String.IsNullOrEmpty(ApplicationUser.FirstName) || String.IsNullOrEmpty(ApplicationUser.LastName)))
                    {
                        return string.Concat(ApplicationUser.FirstName, " ", ApplicationUser.LastName);
                    }
                    else
                        return ApplicationUser.UserName;
                }
                else
                    return "Not Set";
            }
        }

        public virtual Student Student { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
