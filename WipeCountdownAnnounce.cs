using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Oxide.Core;
using Oxide.Core.Plugins;
using Oxide.Core.Libraries.Covalence;

namespace Oxide.Plugins
{
    [Info("WipeCountdownAnnounce", "Agamemnon", "1.0.0")]
    [Description("Announces the time remaining until the next wipe to the server chat.")]
    class WipeCountdownAnnounce : RustPlugin
    {
        [PluginReference] private Plugin WipeCountdown;

        private ConfigData _configData;
        private bool _wipeCountdownLoaded = false;
        private Timer _announceTimer = null;
        private Timer _announceSixtyTimer = null;
        private Timer _announceThirtyTimer = null;
        private Timer _announceFifteenTimer = null;
        private Timer _announceTenTimer = null;
        private Timer _announceFiveTimer = null;
        private Timer _announceOneTimer = null;

        #region Oxide Hooks
        private void OnServerInitialized()
        {
            if (!LoadConfigVariables())
            {
                PrintError("ERROR: The config file is corrupt. Either fix or delete it and restart the plugin.");
                PrintError("ERROR: Unloading plugin.");
                Interface.Oxide.UnloadPlugin(this.Title);
                return;
            }

            if (WipeCountdown == null)
            {
                PrintWarning("The WipeCountdown plugin isn't loaded. Wipe announcements are suspended.");
                _wipeCountdownLoaded = false;
            }
            else
            {
                StartTimers();
                _wipeCountdownLoaded = true;
            }
        }

        private void OnPluginLoaded(Plugin name)
        {
            if (name.ToString() == "Oxide.Plugins.WipeCountdown")
            {
                PrintWarning("The WipeCountdown plugin has been loaded. Wipe announcements will resume.");
                StartTimers();
                _wipeCountdownLoaded = true;
            }
        }

        private void OnPluginUnloaded(Plugin name)
        {
            if (name.ToString() == "Oxide.Plugins.WipeCountdown")
            {
                PrintWarning("The WipeCountdown plugin has been unloaded. Wipe announcements are suspended.");
                StopTimers();
                _wipeCountdownLoaded = false;
            }
        }
        #endregion

        #region Helper Functions
        private void StartTimers()
        {
            if (_announceTimer == null)
            {
                _announceTimer = timer.Every(_configData.AnnounceInterval * 60, BroadcastAnnouncement);
            }

            if (_configData.CountdownEnabled)
            {
                long timeleft = GetCountdownSeconds();
                if (timeleft > 3600)
                {   
                    if (_announceSixtyTimer == null)
                    {
                        timeleft = GetCountdownSeconds();
                        _announceSixtyTimer = timer.Once(timeleft - 3600, () => BroadcastCountdown(60));
                    }
                }
                if (timeleft > 1800)
                {   
                    if (_announceThirtyTimer == null)
                    {
                        timeleft = GetCountdownSeconds();
                        _announceThirtyTimer = timer.Once(timeleft - 1800, () => BroadcastCountdown(30));
                    }
                }
                if (timeleft > 900)
                {   
                    if (_announceFifteenTimer == null)
                    {
                        timeleft = GetCountdownSeconds();
                        _announceFifteenTimer = timer.Once(timeleft - 900, () => BroadcastCountdown(15));
                    }
                }
                if (timeleft > 600)
                {   
                    if (_announceTenTimer == null)
                    {
                        timeleft = GetCountdownSeconds();
                        _announceTenTimer = timer.Once(timeleft - 600, () => BroadcastCountdown(10));
                    }
                }
                if (timeleft > 300)
                {   
                    if (_announceFiveTimer == null)
                    {
                        timeleft = GetCountdownSeconds();
                        _announceFiveTimer = timer.Once(timeleft - 300, () => BroadcastCountdown(5));
                    }
                }
                if (timeleft > 60)
                {   
                    if (_announceOneTimer == null)
                    {
                        timeleft = GetCountdownSeconds();
                        _announceOneTimer = timer.Once(timeleft - 60, () => BroadcastCountdown(1));
                    }
                }
            }
        }

        private void StopTimers()
        {
            if (_announceTimer != null)
            {
                _announceTimer.Destroy();
                _announceTimer = null;
            }
            if (_announceSixtyTimer != null)
            {
                _announceSixtyTimer.Destroy();
                _announceSixtyTimer = null;
            }
            if (_announceThirtyTimer != null)
            {
                _announceThirtyTimer.Destroy();
                _announceThirtyTimer = null;
            }
            if (_announceFifteenTimer != null)
            {
                _announceFifteenTimer.Destroy();
                _announceFifteenTimer = null;
            }
            if (_announceTenTimer != null)
            {
                _announceTenTimer.Destroy();
                _announceTenTimer = null;
            }
            if (_announceFiveTimer != null)
            {
                _announceFiveTimer.Destroy();
                _announceFiveTimer = null;
            }
            if (_announceOneTimer != null)
            {
                _announceOneTimer.Destroy();
                _announceOneTimer = null;
            }
        }

        private void BroadcastAnnouncement()
        {
            if (_wipeCountdownLoaded)
            {
                long timeleft = GetCountdownSeconds();
                TimeSpan timeleftFormatted = TimeSpan.FromSeconds(timeleft);
                int days = timeleftFormatted.Days;
                int hours = timeleftFormatted.Hours;
                int minutes = timeleftFormatted.Minutes;
                int seconds = timeleftFormatted.Seconds;

                if (days >= 0 && hours >= 0 && minutes >= 0 && seconds >= 0)
                {
                    if (timeleft > 86400)
                    {   
                        Server.Broadcast(Lang("StandardMessage",
                            new KeyValuePair<string, string>("days", days.ToString()),
                            new KeyValuePair<string, string>("hours", hours.ToString()),
                            new KeyValuePair<string, string>("minutes", minutes.ToString()),
                            new KeyValuePair<string, string>("seconds", seconds.ToString())), _configData.ChatIcon);
                    }
                    else if (timeleft <= 3660 && _configData.CountdownEnabled)
                    {
                        // Do nothing
                    }
                    else if (timeleft < 3600)
                    {
                        Server.Broadcast(Lang("FinalHourMessage",
                            new KeyValuePair<string, string>("minutes", minutes.ToString())), _configData.ChatIcon);
                    }
                    else
                    {
                        Server.Broadcast(Lang("FinalDayMessage",
                            new KeyValuePair<string, string>("days", days.ToString()),
                            new KeyValuePair<string, string>("hours", hours.ToString()),
                            new KeyValuePair<string, string>("minutes", minutes.ToString()),
                            new KeyValuePair<string, string>("seconds", seconds.ToString())), _configData.ChatIcon);
                    }
                }
            }
        }

        private void BroadcastCountdown(int minutes)
        {
            if (_wipeCountdownLoaded)
            {
                Server.Broadcast(Lang("FinalHourMessage",
                    new KeyValuePair<string, string>("minutes", minutes.ToString())), _configData.ChatIcon);
            }
        }
        #endregion

        #region Plugin API Calls
        private long GetCountdownSeconds()
        {
            long seconds = (long)WipeCountdown?.Call("GetCountdownSeconds_API");
            return seconds;
        }

        private string GetCountdownFormatted()
        {
            string formatted = (string)WipeCountdown?.Call("GetCountdownFormated_API");
            return formatted;
        }
        #endregion

        #region Configuration
        private class ConfigData
        {
            [JsonProperty(PropertyName = "Announcement Interval (in Minutes)")]
            public float AnnounceInterval = 20.0f;

            [JsonProperty(PropertyName = "Countdown Final Hour (60, 30, 15, 10, 5, 1 minutes)")]
            public bool CountdownEnabled = true;

            [JsonProperty(PropertyName = "Chat Icon (SteamID)")]
            public ulong ChatIcon = 76561199034048902;
        }

        private bool LoadConfigVariables()
        {
            try
            {
                _configData = Config.ReadObject<ConfigData>();
            }
            catch
            {
                return false;
            }

            SaveConfig(_configData);
            return true;
        }

        protected override void LoadDefaultConfig()
        {
            PrintWarning("Creating new config file.");
            _configData = new ConfigData();
            SaveConfig(_configData);
        }

        private void SaveConfig(ConfigData config)
        {
            Config.WriteObject(config, true);
        }
        #endregion

        #region Language
        protected override void LoadDefaultMessages()
        {
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["StandardMessage"] = "Next wipe will occur in <color=#ffa500>{days} days</color>, <color=#ffa500>{hours} hours</color>.",
                ["FinalDayMessage"] = "Next wipe will occur in <color=#ffa500>{hours} hours</color>, <color=#ffa500>{minutes} minutes</color>.",
                ["FinalHourMessage"] = "Next wipe will occur in <color=#ffa500>{minutes} minutes</color>."

            }, this);
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["StandardMessage"] = "Le prochain wipe aura lieu dans <color=#ffa500>{days} jours</color>, <color=#ffa500>{hours} heures</color>.",
                ["FinalDayMessage"] = "Le prochain wipe aura lieu dans <color=#ffa500>{hours} heures</color>, <color=#ffa500>{minutes} minutes</color>.",
                ["FinalHourMessage"] = "Le prochain wipe aura lieu dans <color=#ffa500>{minutes} minutes</color>."
            }, this, "fr");
        }

        private string Lang(string key) => string.Format(lang.GetMessage(key, this));
        private string Lang(string key, params KeyValuePair<string, string>[] replacements)
        {
            var message = lang.GetMessage(key, this);

            foreach (var replacement in replacements)
                message = message.Replace($"{{{replacement.Key}}}", replacement.Value);

            return message;
        }
        #endregion
    }
}
