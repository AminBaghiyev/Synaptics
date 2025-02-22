using RazorLight;
using System.Reflection;

namespace Synaptics.Infrastructure.Utilities;

public static class RazorViewRenderer
{
    static readonly RazorLightEngine _engine = new RazorLightEngineBuilder()
        .UseEmbeddedResourcesProject(Assembly.GetExecutingAssembly())
        .UseMemoryCachingProvider()
        .Build();

    public static async Task<string> RenderViewToStringAsync(string viewName, object model)
    {
        return await _engine.CompileRenderAsync($"Synaptics.Infrastructure.Templates.{viewName}.cshtml", model);
    }
}
