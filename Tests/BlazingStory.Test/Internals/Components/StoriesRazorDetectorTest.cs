﻿using System.Reflection;
using BlazingStory.Internals.Services;
using BlazingStory.Internals.Services.XmlDocComment;
using BlazingStory.Test._Fixtures;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace BlazingStory.Test.Internals.Components;

internal class StoriesRazorDetectorTest
{
    [Test]
    public async Task Detect_from_Empty_Test()
    {
        await using var host = new TestHost();
        var storiesStore = new StoriesStore();
        using var ctx = new Bunit.TestContext();
        ctx.RenderTree.Add<CascadingValue<IServiceProvider>>(p => p.Add(p => p.Value, host.Services));

        ctx.Services.AddSingleton(_ => Mock.Of<IXmlDocComment>());
        var cut = ctx.RenderComponent<BlazingStory.Internals.Components.StoriesRazorDetector>(builder => builder
            .Add(_ => _.Assemblies, Enumerable.Empty<Assembly>())
            .Add(_ => _.StoriesStore, storiesStore));

        storiesStore.StoryContainers.Count().Is(0);
    }

    [Test]
    public async Task Detect_Test()
    {
        // Given
        await using var host = new TestHost();
        var storiesStore = new StoriesStore();
        using var ctx = new Bunit.TestContext();
        ctx.RenderTree.Add<CascadingValue<IServiceProvider>>(p => p.Add(p => p.Value, host.Services));

        // When
        var cut = ctx.RenderComponent<BlazingStory.Internals.Components.StoriesRazorDetector>(builder => builder
            .Add(_ => _.Assemblies, new[] {
                typeof(BlazingStoryApp1.App).Assembly,
                typeof(BlazingStoryApp2.App).Assembly,
            })
            .Add(_ => _.StoriesStore, storiesStore));

        // Then
        storiesStore.StoryContainers.Count().Is(6);
    }
}
