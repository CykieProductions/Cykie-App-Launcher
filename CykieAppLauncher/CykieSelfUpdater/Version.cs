﻿//This file is now compatible with .NET 5
using System;

namespace CykieAppLauncher.Models
{
    /*public record class ConfigurationInfo(string Name, string Version, string ZipPath, string LaunchPath,
    string VersionLink, string BuildLink)
    {
        public ConfigurationInfo(string[] info) : 
            this(info[0], info[1], info[2], info[3], info[4], info[5]){ }
    }*/

    [Serializable]
    public class ConfigurationInfo
    {
        public string Name, Version, ZipPath, LaunchPath, VersionLink, BuildLink;

        public static ConfigurationInfo Empty { get; } = new ConfigurationInfo(new string[6]);

        public ConfigurationInfo(string name, string version, string zipPath, string launchPath, string versionLink, string buildLink)
        {
            Name = name;
            Version = version;
            ZipPath = zipPath;
            LaunchPath = launchPath;
            VersionLink = versionLink;
            BuildLink = buildLink;
        }
        public ConfigurationInfo(string[] info)
        {
            Name = info[0];
            Version = info[1];
            ZipPath = info[2];
            LaunchPath = info[3];
            VersionLink = info[4];
            BuildLink = info[5];
        }

        public bool IsEmpty()
        {
            return string.IsNullOrWhiteSpace(Name) ||
                   string.IsNullOrWhiteSpace(Version) ||
                   string.IsNullOrWhiteSpace(ZipPath) ||
                   string.IsNullOrWhiteSpace(LaunchPath) ||
                   string.IsNullOrWhiteSpace(VersionLink) ||
                   string.IsNullOrWhiteSpace(BuildLink);
        }
    }

    public struct Version : IComparable<Version>
    {
        public static Version Invalid { get; } = new Version(-1, -1, -1);
        public static Version Default { get; } = new Version(0, 1, 0);

        short major, minor, patch;
        short? revision;

        public Version(Version? version = null)
        {
            Version info;
            if (version.HasValue)
                info = version.Value;
            else
                info = Default;

            major = info.major; 
            minor = info.minor; 
            patch = info.patch;
            revision = info.revision;
        }
        public Version(short major, short minor, short patch = 0, short? revision = null)
        {
            this.major = major;
            this.minor = minor;
            this.patch = patch;
            this.revision = revision;
        }
        public Version(string? versionStr)
        {
            if (versionStr == null)
            {
                major = Invalid.major;
                minor = Invalid.minor;
                patch = Invalid.patch;
                revision = Invalid.revision;
                return;
            }

            var parts = versionStr.Split('.');

            try
            {
                if (parts.Length >= 1)
                    major = short.Parse(parts[0]);
                else
                    major = 0;

                if (parts.Length >= 2)
                    minor = short.Parse(parts[1]);
                else
                    minor = 0;

                if (parts.Length >= 3)
                    patch = short.Parse(parts[2]);
                else
                    patch = 0;

                if (parts.Length > 3)
                    revision = short.Parse(parts[3]);
                else
                    revision = null;
            }
            catch (Exception)
            {
                major = Invalid.major;
                minor = Invalid.minor;
                patch = Invalid.patch;
                revision = Invalid.revision;
            }
        }

        public void Set(short inMajor, short inMinor, short inPatch = 0, short? inRevision = null)
        {
            major = inMajor;
            minor = inMinor;
            patch = inPatch;
            revision = inRevision;
        }
        public void Set(Version inVersion)
        {
            major = inVersion.major;
            minor = inVersion.minor;
            patch = inVersion.patch;
            revision = inVersion.revision;
        }

        public readonly bool IsValid()
        {
            var v = new Version(ToString());

            if (v.Equals(Invalid))
                return false;
            return true;
        }

        public override readonly string ToString()
        {
            var str = $"{major}.{minor}.{patch}";
            if (revision != null)
                str += $".{revision}";
            return str;
        }

        public readonly int CompareTo(Version other)
        {
            if (Equals(other)) return 0;

            //! Compare with decreasing importance

            if (major != other.major)
                return major > other.major ? 1 : -1;//Greater major means newer version

            //Majors are the same so compare minors
            if (minor != other.minor)
                return minor > other.minor ? 1 : -1;

            //Minors are the same so compare patches
            if (patch != other.patch)
                return patch > other.patch ? 1 : -1;

            if (revision == null)
                return -1;

            if (other.revision == null)
                return 1;

            return revision > other.revision ? 1 : -1;
        }

    }
}