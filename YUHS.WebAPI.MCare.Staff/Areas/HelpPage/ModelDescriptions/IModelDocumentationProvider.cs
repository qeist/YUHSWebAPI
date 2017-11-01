using System;
using System.Reflection;

namespace YUHS.WebAPI.MCare.Staff.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}