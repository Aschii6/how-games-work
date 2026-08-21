using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;

namespace HordeSurvival.Core.Localization;

/// <summary>
/// Manages localization settings for the game, including retrieving supported cultures and setting the current culture for localization.
/// </summary>
internal class LocalizationManager
{
    public const string DEFAULT_CULTURE_CODE = "";

    /// <summary>
    /// Retrieves a list of supported cultures based on available language resources in the game.
    /// </summary>
    public static List<CultureInfo> GetSupportedCultures()
    {
        var supportedCultures = new List<CultureInfo>();

        var assembly = Assembly.GetExecutingAssembly();
        var resourceManager = new ResourceManager("HordeSurvival.Core.Localization.Resources", assembly);

        CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
        foreach (CultureInfo culture in cultures)
        {
            try
            {
                var resourceSet = resourceManager.GetResourceSet(culture, true, false);
                if (resourceSet != null)
                {
                    supportedCultures.Add(culture);
                }
            }
            catch (MissingManifestResourceException)
            {
                // No .resx exists for this culture.
            }
        }

        // The invariant culture represents the default (non-localized) resources.
        supportedCultures.Add(CultureInfo.InvariantCulture);

        return supportedCultures;
    }

    /// <summary>
    /// Sets the current culture and UI culture of the game for the current thread.
    /// </summary>
    public static void SetCulture(string cultureCode)
    {
        if (cultureCode is null)
            throw new ArgumentNullException(nameof(cultureCode), "A culture code must be provided.");

        // An empty code resolves to CultureInfo.InvariantCulture, the neutral/default resources.
        var culture = new CultureInfo(cultureCode);

        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
    }
}
