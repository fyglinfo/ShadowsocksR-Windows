﻿using System;
using System.IO;
using Microsoft.Win32;
using Shadowsocks.Util;

namespace Shadowsocks.Controller
{
    class AutoStartup
    {
        static string Key = "ShadowsocksR_" + Directory.GetCurrentDirectory().GetDeterministicHashCode();
        static string RegistryRunPath = @"Software\Microsoft\Windows\CurrentVersion\Run";

        public static bool Set(bool enabled)
        {
            RegistryKey runKey = null;
            try
            {
                string path = Utils.GetExecutablePath();
                runKey = Utils.OpenRegKey(RegistryRunPath, true);
                if (enabled)
                {
                    runKey.SetValue(Key, path);
                }
                else
                {
                    runKey.DeleteValue(Key);
                }
                runKey.Close();
                return true;
            }
            catch //(Exception e)
            {
                //Logging.LogUsefulException(e);
                return Utils.RunAsAdmin("--setautorun") == 0;
            }
            finally
            {
                if (runKey != null)
                {
                    try
                    {
                        runKey.Close();
                    }
                    catch (Exception e)
                    {
                        Logging.LogUsefulException(e);
                    }
                }
            }
        }

        public static bool Switch()
        {
            bool enabled = !Check();
            RegistryKey runKey = null;
            try
            {
                string path = Utils.GetExecutablePath();
                runKey = Utils.OpenRegKey(RegistryRunPath, true);
                if (enabled)
                {
                    runKey.SetValue(Key, path);
                }
                else
                {
                    runKey.DeleteValue(Key);
                }
                runKey.Close();
                return true;
            }
            catch (Exception e)
            {
                Logging.LogUsefulException(e);
                return false;
            }
            finally
            {
                if (runKey != null)
                {
                    try
                    {
                        runKey.Close();
                    }
                    catch (Exception e)
                    {
                        Logging.LogUsefulException(e);
                    }
                }
            }
        }

        public static bool Check()
        {
            RegistryKey runKey = null;
            try
            {
                runKey = Utils.OpenRegKey(RegistryRunPath, false);
                string[] runList = runKey.GetValueNames();
                runKey.Close();
                foreach (string item in runList)
                {
                    if (item.Equals(Key))
                        return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Logging.LogUsefulException(e);
                return false;
            }
            finally
            {
                if (runKey != null)
                {
                    try
                    {
                        runKey.Close();
                    }
                    catch (Exception e)
                    {
                        Logging.LogUsefulException(e);
                    }
                }
            }
        }
    }
}
