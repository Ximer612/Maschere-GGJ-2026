using System.Collections;
using  System.collections.Generic;
using UnityEngine;

using UnityEngine.Localization.Settings;

public class LocaleSelector: MonoBehaviour

private void Start()
{
    int ID = PlayerPrefs.GetInt("Localekey", 0);
    ChangeLocale(ID);
}
{
    private bool active = false;
    public void ChangeLocale(int localeID)

    {
        if (active == true)
        return;
        StartCoroutine(SetLocale(localeID))
    }
    IEnumerator SetLocal(int _localeID)
{
    active = true;
    yield return LocaliztionSettings.InitializationOperation;
    LocaliztionSettings.SelectedLocale=LocaliztionSettings.AvailableLocales.Locales[_localeID];
    PlayerPrefs.SetInt("Localekey", _localeID);
    active = false;
   }
}

