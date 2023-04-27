using System.Collections.Generic;
using Apps.RemoteConfig;
using editor;
using NUnit.Framework;
using UnityEngine;

public class TestCollection
{
    public bool isConfiged = false;

    [Order(0)]
    [Test]
    public void TestCollectionGlobal()
    {
        HeadTemplateEditor.ResetAllData();

        Configurator configurator = new Configurator();
        Assert.IsNotNull(configurator);

        Assert.IsTrue(!configurator.IsReady);
        Dictionary<string, string> values1 = new Dictionary<string, string>();
        values1.Add("Tag", "v0.9.1");
        values1.Add("Speed", "10");
        values1.Add("Name", "Vov");

        configurator.OnConfigured += OnConfigured1;
        configurator.UpdateConfig(values1);
        configurator.SetReadyConfig();
        configurator.OnConfigured -= OnConfigured1;

        Assert.IsTrue(configurator.IsReady);
        Assert.IsTrue(isConfiged);

        Assert.IsTrue(configurator.TagConfig.Equals("v0.9.1"));
        Assert.IsTrue(configurator.Values["Name"] == "Vov");
        Assert.IsTrue(configurator.Values["Speed"] == ("10"));

        Assert.IsTrue(!configurator.Values.ContainsKey("Tag"));


        Assert.IsTrue(configurator.IsConfiguredSave);
        Dictionary<string, string> values2 = new Dictionary<string, string>();
        values2.Add("Tag", "v0.10.3");
        values2.Add("Rotation", "20");
        values2.Add("Name", "SISI");

        configurator.OnConfigured += OnConfigured2;
        configurator.UpdateConfig(values2);
        configurator.SetReadyConfig();
        configurator.OnConfigured -= OnConfigured2;

        Assert.IsTrue(configurator.TagConfig == "v0.10.3");

        Assert.IsTrue(configurator.Values["Rotation"] == "20");
        Assert.IsTrue(configurator.Values["Name"] == ("SISI"));

        Assert.IsTrue(!configurator.Values.ContainsKey("Speed"));
        Assert.IsTrue(!configurator.Values.ContainsKey("Tag"));


        Configurator configurator2 = new Configurator();
        Assert.IsNotNull(configurator2);

        Assert.IsTrue(configurator2.TagConfig == "v0.10.3");

        Assert.IsTrue(configurator2.Values["Rotation"] == "20");
        Assert.IsTrue(configurator2.Values["Name"] == ("SISI"));
    }

    private void OnConfigured1(IConfigCollection collection)
    {
        isConfiged = true;

        Assert.IsTrue(collection["Speed"] == ("10"));
        Assert.IsTrue(collection["Name"] == ("Vov"));
        Assert.IsTrue(!collection.ContainsKey("Tag"));
    }

    private void OnConfigured2(IConfigCollection collection)
    {
        isConfiged = true;

        Assert.IsTrue(collection["Rotation"] == "20");
        Assert.IsTrue(collection["Name"] == ("SISI"));

        Assert.IsTrue(!collection.ContainsKey("Speed"));
        Assert.IsTrue(!collection.ContainsKey("Tag"));
    }

    [Order(2)]
    [Test]
    public void TestCollectionUndefinedTag()
    {
        HeadTemplateEditor.ResetAllData();

        Configurator configurator = new Configurator();
        Assert.IsNotNull(configurator);
        Assert.IsTrue(!configurator.IsConfiguredSave);

        Assert.IsTrue(!configurator.IsReady);
        Dictionary<string, string> values1 = new Dictionary<string, string>();
        values1.Add("Speed", "10");
        values1.Add("Name", "Vov");

        configurator.UpdateConfig(values1);
        configurator.SetReadyConfig();

        Assert.IsTrue(configurator.IsReady);
        Assert.IsTrue(configurator.TagConfig == Constants.UndefinedTag);

        Dictionary<string, string> values2 = new Dictionary<string, string>();
        values2.Add("Wow", "52");
        values2.Add("Pop", "8000051");
        values2.Add("null", null);

        configurator.UpdateConfig(values2);
        configurator.SetReadyConfig();

        Assert.IsTrue(configurator.Values["Wow"] == "52");
        Assert.IsTrue(configurator.Values["Pop"] == "8000051");

        Assert.IsTrue(!configurator.Values.ContainsKey("Speed"));
        Assert.IsTrue(!configurator.Values.ContainsKey("Name"));
    }

    [Test]
    public void TestHasMultipleConfigCase1()
    {
        HeadTemplateEditor.ResetAllData();

        Configurator configurator = new Configurator(false);
        Assert.IsNotNull(configurator);

        Assert.IsTrue(!configurator.IsReady);
        Dictionary<string, string> values1 = new Dictionary<string, string>();
        values1.Add("Tag", "v0.9.1");
        values1.Add("Speed", "10");
        values1.Add("Name", "Vov");

        configurator.UpdateConfig(values1);
        configurator.SetReadyConfig();

        Assert.IsTrue(configurator.IsReady);

        Assert.IsTrue(configurator.TagConfig.Equals("v0.9.1"));
        Assert.IsTrue(configurator.Values["Name"] == "Vov");
        Assert.IsTrue(configurator.Values["Speed"] == ("10"));

        Assert.IsTrue(!configurator.Values.ContainsKey("Tag"));


        Assert.IsTrue(configurator.IsConfiguredSave);
        Dictionary<string, string> values2 = new Dictionary<string, string>();
        values2.Add("Tag", "v0.10.3");
        values2.Add("Rotation", "20");
        values2.Add("Name", "SISI");

        configurator.UpdateConfig(values2);
        configurator.SetReadyConfig();

        Assert.IsFalse(configurator.TagConfig == "v0.10.3");

        Assert.IsFalse(configurator.Values.ContainsKey("Rotation"));
        Assert.IsTrue(!(configurator.Values["Speed"] == ("SISI")));
    }

    [Test]
    public void TestHasMultipleConfigCase2()
    {
        HeadTemplateEditor.ResetAllData();

        Configurator configurator = new Configurator(false);
        Assert.IsNotNull(configurator);

        Assert.IsTrue(!configurator.IsReady);
        Dictionary<string, string> values1 = new Dictionary<string, string>();
        values1.Add(Constants.TagKey, Constants.UndefinedTag);

        configurator.UpdateConfig(values1);
        configurator.SetReadyConfig();

        Assert.IsTrue(configurator.IsReady);

        Assert.IsTrue(configurator.TagConfig.Equals(Constants.UndefinedTag));

        Assert.IsTrue(!configurator.Values.ContainsKey("Tag"));


        Assert.IsTrue(configurator.IsConfiguredSave);
        Dictionary<string, string> values2 = new Dictionary<string, string>();
        values2.Add("Tag", "v0.10.3");
        values2.Add("Rotation", "20");
        values2.Add("Name", "SISI");

        configurator.UpdateConfig(values2);
        configurator.SetReadyConfig();

        Assert.IsFalse(configurator.TagConfig == "v0.10.3");

        Assert.IsFalse(configurator.Values.ContainsKey("Rotation"));
        Assert.IsFalse(configurator.Values.ContainsKey("Name"));
    }
}
