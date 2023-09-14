using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationBrowserPanel : Panel
{
    public FormationBrowser FormationBrowser;

    public override void Configure()
    {
        base.Configure();
        FormationBrowser.SetAddress(new Address("App.FormationInventory"));
    }

    public override void Refresh()
    {
        base.Refresh();
        FormationBrowser.Refresh();
    }
}
