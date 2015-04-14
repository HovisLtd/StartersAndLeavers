using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Hovis.Web.Base.Models
{
    [MetadataType(typeof(StarterMetaData))]
    public partial class t_New_Starter_Details
    {
    }

    [MetadataType(typeof(RequestMetaData))]
    public partial class t_Servicedesk_Contact
    {
    }

    [MetadataType(typeof(LeaversMetaData))]
    public partial class t_Leaver_Details
    {
    }

    [MetadataType(typeof(ActiveDirectoryMetaData))]
    public partial class v_AD_Users_Lookup
    {
    }
}