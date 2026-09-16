using ManagedShell.ShellFolders;
using RetroBar.Utilities;
using System.Collections.Generic;

namespace RetroBar.Extensions
{
    public static class QuickLaunchIconExtensions
    {
        /// <summary>
        /// Whether the user has manually chosen to always keep this Quick Launch shortcut in
        /// the overflow flyout, regardless of whether it would otherwise fit on the toolbar.
        /// </summary>
        public static bool IsAlwaysInOverflow(this ShellFile file)
        {
            return file != null && Settings.Instance.QuickLaunchHiddenItems.Contains(file.Path);
        }

        public static void SetAlwaysInOverflow(this ShellFile file, bool alwaysInOverflow)
        {
            if (file == null)
            {
                return;
            }

            var settings = new List<string>(Settings.Instance.QuickLaunchHiddenItems);
            bool changed;

            if (!alwaysInOverflow)
            {
                changed = settings.Remove(file.Path);
            }
            else if (!settings.Contains(file.Path))
            {
                settings.Add(file.Path);
                changed = true;
            }
            else
            {
                changed = false;
            }

            if (changed)
            {
                Settings.Instance.QuickLaunchHiddenItems = settings;
            }
        }
    }
}
