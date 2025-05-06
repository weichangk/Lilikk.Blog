using Furion;
using System.Reflection;

namespace Blog.Admin.Web.Entry;

public class SingleFilePublish : ISingleFilePublish
{
    public Assembly[] IncludeAssemblies()
    {
        return Array.Empty<Assembly>();
    }

    public string[] IncludeAssemblyNames()
    {
        return new[]
        {
            "Blog.Admin.Application",
            "Blog.Admin.Core",
            "Blog.Admin.Web.Core"
        };
    }
}