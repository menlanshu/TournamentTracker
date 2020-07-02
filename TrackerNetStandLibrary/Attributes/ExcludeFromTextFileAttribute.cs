using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    class ExcludeFromTextFileAttribute : Attribute
    {
    }
}
